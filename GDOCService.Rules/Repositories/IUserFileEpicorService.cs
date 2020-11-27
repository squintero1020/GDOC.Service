using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SharedService.Responses.Response;

namespace GDOCService.Rules.Repositories
{
    public interface IUserFileEpicorService
    {
        Task<PetitionResponse> GetRows(string Where);
    }
}
