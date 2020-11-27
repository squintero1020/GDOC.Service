using GDOCService.DataAccess.DataContext;
using GDOCService.DataAccess.Models;
using GDOCService.Rules.BusinessObjects;
using GDOCService.Rules.Repositories;
using Microsoft.Extensions.Logging;
using SharedService.Responses.Response;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GDOCService.Rules.Services
{
    public class StoreService : IStoreService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<StoreService> _log;
        public StoreService(HttpClient httpClient, ILogger<StoreService> log) =>
            (_httpClient, _log) = (httpClient ?? throw new ArgumentNullException(nameof(httpClient)), log ?? throw new ArgumentNullException(nameof(log)));

        public async Task<PetitionResponse> GetByCompany(GDOCContext context, int companyid)
        {
            BOStores bOStores = new BOStores(context);
            return await bOStores.GetByCompany(companyid);
        }

        public async Task<PetitionResponse> GetByID(GDOCContext context, int companyid,int storeid)
        {
            _log.LogWarning("Ya voy hacer la consulta del GetByID para la prueba {fulano}","Juan");
            BOStores bOStores = new BOStores(context);
            return await bOStores.GetByID(companyid,storeid);
        }
        public async Task<PetitionResponse> Add(GDOCContext context, int companyid, Stores store)
        {
            BOStores bOStores = new BOStores(context, store);
            return await bOStores.Add();
        }

        public async Task<PetitionResponse> Update(GDOCContext context, int companyid, int storeid, Stores store)
        {
            BOStores bOStores = new BOStores(context, store);
            return await bOStores.Update();
        }

        public async Task<PetitionResponse> Delete(GDOCContext context, int companyid, int storeid)
        {
            BOStores bOStores = new BOStores(context);
            return await bOStores.Delete(companyid, storeid);
        }

    }
}
