using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GDOCService.DataAccess.Models;
using SharedService.Responses.Response;

namespace GDOCService.Rules.Repositories
{
    public interface IPanesService
    {
        Task<PetitionResponse> Get();
        Task<PetitionResponse> GetByID(string Name);
        Task<PetitionResponse> Add(Panes panes);
        Task<PetitionResponse> Update(Panes panes);
    }
}
