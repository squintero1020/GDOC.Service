using GDOCService.API.Infraestructure.Polly;
using GDOCService.Rules.Repositories;
using GDOCService.Rules.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Microsoft.Extensions.DependencyInjection
{
    public static class IControllerCollectionExtentions
    {
        public static IServiceCollection AddStoresService(this IServiceCollection services) =>
            services.
                AddHttpClient<IStoreService, StoreService>()
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler((serviceProvider, request) => RetryPolicy.GetPolicyWithJitterStrategy<StoreService>(serviceProvider))
                .Services;

        public static IServiceCollection AddUserFileEpicorService(this IServiceCollection services) =>
       services.
           AddHttpClient<IUserFileEpicorService, UserFileEpicorService>()
           .SetHandlerLifetime(TimeSpan.FromMinutes(5))
           .AddPolicyHandler((serviceProvider, request) => RetryPolicy.GetPolicyWithJitterStrategy<UserFileEpicorService>(serviceProvider))
           .Services;

        public static IServiceCollection AddPanesService(this IServiceCollection services) =>
       services.
           AddHttpClient<IPanesService, PanesService>()
           .SetHandlerLifetime(TimeSpan.FromMinutes(5))
           .AddPolicyHandler((serviceProvider, request) => RetryPolicy.GetPolicyWithJitterStrategy<PanesService>(serviceProvider))
           .Services;
    }
}
