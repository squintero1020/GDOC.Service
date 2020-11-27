using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GDOCService.Rules.BusinessObjects;
using GDOCService.Rules.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SharedService.Responses.Response;

namespace GDOCService.Rules.Services
{
    public class UserFileEpicorService : IUserFileEpicorService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<UserFileEpicorService> _log;
        private readonly IConfiguration _IConfiguration;

        public UserFileEpicorService(HttpClient httpClient, ILogger<UserFileEpicorService> log, IConfiguration Configuration) =>
            (_httpClient, _log, _IConfiguration) = 
            (httpClient ?? throw new ArgumentNullException(nameof(httpClient)), 
                log ?? throw new ArgumentNullException(nameof(log)), 
                    Configuration ?? throw new ArgumentNullException(nameof(Configuration)));

        public async Task<PetitionResponse> GetRows(string Where)
        {

            BOUserFileEpicor boUser = new BOUserFileEpicor(_IConfiguration);
            return await boUser.GetRows(Where);
        }
    }
}
