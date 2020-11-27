
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GDOCService.DataAccess.DataContext;
using GDOCService.DataAccess.Models;
using SharedService.Responses.Response;

namespace GDOCService.Rules.Repositories
{
    public interface IStoreService
    {
        Task<PetitionResponse> GetByCompany(GDOCContext context,int companyid);
        Task<PetitionResponse> GetByID(GDOCContext context, int companyid, int storeid);
        Task<PetitionResponse> Add(GDOCContext context, int companyid, Stores store);
        Task<PetitionResponse> Update(GDOCContext context, int companyid, int storeid, Stores stores);
        Task<PetitionResponse> Delete(GDOCContext context, int companyid, int storeid);
    }
}
