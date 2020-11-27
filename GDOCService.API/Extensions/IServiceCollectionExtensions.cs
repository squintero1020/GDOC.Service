using Autofac;
using Autofac.Extensions.DependencyInjection;
using EventBus;
using EventBus.Abstractions;
using EventBusRabbitMQ;
using GDOCService.API.Diagnostics;
using GDOCService.API.Infraestructure.Middleware;
using GDOCService.DataAccess.DataContext;
using Hellang.Middleware.ProblemDetails;
using IntegrationEventLog.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {

        public static IServiceCollection AddCustomMvc(this IServiceCollection services) =>
            services
                .AddMvc()
                .AddApplicationPart(typeof(IServiceCollectionExtensions).Assembly)
                .Services;

        public static IServiceCollection AddCustomMiddlewares(this IServiceCollection services) =>
            services
                .AddSingleton<ErrorMiddleware>();


        public static IServiceCollection AddCustomProblemDetails(this IServiceCollection services, IWebHostEnvironment environment) =>
           services
               .AddProblemDetails(configure =>
               {
                   configure.IncludeExceptionDetails = (ctx, ex) =>
                   {
                       var env = ctx.RequestServices.GetRequiredService<IHostEnvironment>();
                       return env.IsDevelopment() || env.IsStaging();
                   };
               });

        public static IServiceCollection AddCustomApiBehaviour(this IServiceCollection services)
        {

            return services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = false;
                options.SuppressInferBindingSourcesForParameters = false;

                options.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Instance = context.HttpContext.Request.Path,
                        Status = StatusCodes.Status400BadRequest,
                        Type = $"https://httpstatuses.com/400",
                        Detail = "Please refer to the errors property for additional details."
                    };
                    return new BadRequestObjectResult(problemDetails)
                    {
                        ContentTypes =
                         {
                                "application/problem+json",
                                "application/problem+xml"
                         }
                    };
                };
            });
        }

        public static IServiceCollection AddEntityFrameworkCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<GDOCContext>(options =>
            {
                options.UseMySQL(configuration.GetConnectionString("Default"));
            });

            return services;
        }

        public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            var hcBuilder = services.AddHealthChecks();
            hcBuilder.AddDbContextCheck<GDOCContext>();

            hcBuilder
                    .AddRabbitMQ(
                        $"amqp://{configuration["EventBusConnection"]}",
                        name: "ordering-rabbitmqbus-check",
                        tags: new string[] { "rabbitmqbus" });

            return services;
        }

        public static IServiceCollection AddHostingDiagnosticHandler(this IServiceCollection services)
        {
            return services.AddHostedService<HostingDiagnosticHandler>();
        }

        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            
            return services;
        }

        public static IServiceCollection AddCustomHttContext(this IServiceCollection services) 
        {
            return services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static IServiceCollection AddCustomIntegrations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(
                sp => (DbConnection c) => new IntegrationEventLogService(c));



            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();


                var factory = new ConnectionFactory()
                {
                    HostName = configuration["EventBusConnection"],
                    DispatchConsumersAsync = true
                };

                if (!string.IsNullOrEmpty(configuration["EventBusUserName"]))
                {
                    factory.UserName = configuration["EventBusUserName"];
                }

                if (!string.IsNullOrEmpty(configuration["EventBusPassword"]))
                {
                    factory.Password = configuration["EventBusPassword"];
                }

                var retryCount = 5;
                if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
                {
                    retryCount = int.Parse(configuration["EventBusRetryCount"]);
                }

                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
            });
            

            return services;
        }

        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            var subscriptionClientName = configuration["SubscriptionClientName"];
            services.AddSingleton<IEventBus, EventBusRabbitMQ.EventBusRabbitMQ>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ.EventBusRabbitMQ>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                var retryCount = 5;
                if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
                {
                    retryCount = int.Parse(configuration["EventBusRetryCount"]);
                }

                return new EventBusRabbitMQ.EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, subscriptionClientName, retryCount);
            });
            
            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            return services;
        }            
    }
}
