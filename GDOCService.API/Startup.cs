

namespace GDOCService.API
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using AutoMapper;
    using FluentValidation;
    using FluentValidation.AspNetCore;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class Startup
    {
        public IConfiguration Configuration { get; }
        private IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            ServicePointManager.ServerCertificateValidationCallback += (senderC, cert, chain, sslPolicyErrors) => true;

            Api.Configuration.ConfigureServices(services, Configuration, Environment)
                .AddEntityFrameworkCore(Configuration)
                .AddCustomAuthorization(Configuration)
                .AddHostingDiagnosticHandler();

            var container = new ContainerBuilder();

            //container.RegisterModule(new MediatorModule());
            container.Populate(services);

            return new AutofacServiceProvider(container.Build());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory logger)
        {
            Api.Configuration.Configure(app, host =>
            {
                return host
                    .UseDefaultFiles()
                    .UseStaticFiles()
                    .UseHeaderDiagnostics()
                    .UseConfigurationEventBus();
            });

            logger.AddFile("Logs/GDOCService-{Date}.txt");
        }
    }
}
