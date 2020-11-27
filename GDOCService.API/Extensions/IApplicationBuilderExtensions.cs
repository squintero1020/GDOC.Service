using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;
using EventBus.Abstractions;
using SharedService.Responses.HealthCheck;

namespace Microsoft.AspNetCore.Builder
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCustomHealthchecks(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) => {
                    context.Response.ContentType = "application/json";

                    var response = new HealthCheckResponse
                    {
                        status = report.Status.ToString(),
                        Checks = report.Entries.Select(x => new HealthCheck
                        {
                            Componenet = x.Key,
                            status = x.Value.Status.ToString(),
                            Description = x.Value.Description
                        }),
                        Duration = report.TotalDuration
                    };
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(response));

                }
            })
            ;
            app.UseHealthChecksUI();

            return app.UseHealthChecks("/ready", new HealthCheckOptions
            {
                Predicate = registration => registration.Tags.Contains("dependencies")
            });
        }

        public static IApplicationBuilder UseHeaderDiagnostics(this IApplicationBuilder app)
        {
            var listener = app.ApplicationServices.GetService<DiagnosticListener>();

            if (listener.IsEnabled())
            {
                return app.Use((context, next) =>
                {
                    var headers = string.Join("|", context.Request.Headers.Values.Select(h => h.ToString()));
                    listener.Write("GDOCService.API.Diagnostics.Headers", new { Headers = headers, HttpContext = context });
                    return next();
                });
            }

            return app;

        }

        public static IApplicationBuilder UseConfigurationEventBus(this IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            return app;
        }
    }
}
