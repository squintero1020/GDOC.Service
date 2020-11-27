using GDOCService.API.Infraestructure.Hubs;
using GDOCService.API.Infraestructure.Middleware;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GDOCService.API.Api
{
    public static class Configuration
    {
        readonly static string SpecificOrigins = "_specificOrigins";

        public static IServiceCollection ConfigureServices(IServiceCollection services, IConfiguration Configuration, IWebHostEnvironment environment)
        {

            return services
                .AddCustomMvc()
                .AddCustomMiddlewares()
                .AddCustomHealthChecks(Configuration)
                .AddCustomProblemDetails(environment)
                .AddCustomIntegrations(Configuration)
                .AddEventBus(Configuration)
                .AddCustomApiBehaviour()
                #region SERVICIO DE CONTROLADORES
                .AddStoresService()
                .AddUserFileEpicorService()
                .AddPanesService()
                #endregion

                .AddCustomHttContext()
                .AddHttpContextAccessor()
                .AddCors(c => {
                    c.AddPolicy(name: SpecificOrigins, builder =>
                    {
                        builder
                       .AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod();
                    });
                })
                .AddSwaggerGen(c => {
                    c.DescribeAllEnumsAsStrings();
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Capa de negocios", Version = "v1.0.0" });
                    c.TagActionsBy(api => new[] { api.GroupName });
                    c.DocInclusionPredicate((name, api) => true);
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });

                })
                .AddSignalR()
                .Services;

        }

        public static IApplicationBuilder Configure(
            IApplicationBuilder app,
            Func<IApplicationBuilder, IApplicationBuilder> configureHost)
        {
            return configureHost(app)
                .UseProblemDetails()
                .UseRouting()
                .UseMiddleware<ErrorMiddleware>()
                .UseCustomHealthchecks()
                .UseCors(SpecificOrigins)
                .UseSwagger(c => {
                    c.SerializeAsV2 = true;
                })
                .UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Capa de negocios");
                    c.OAuthClientId("posingswaggerui");
                    c.OAuthAppName("POS Swagger UI");
                    c.RoutePrefix = string.Empty;

                })
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                            name: "default",
                            pattern: "{controller=Home}/{action=Index}/{id?}");
                    endpoints.MapRazorPages();

                    endpoints.MapHub<DiagnosticsHub>("/diagnostics-hub");
                    endpoints.MapHealthChecksUI();
                });  
        }
    }
}
