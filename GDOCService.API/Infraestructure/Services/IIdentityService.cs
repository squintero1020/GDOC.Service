using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GDOCService.API.Infraestructure.Services
{
    public interface IIdentityService
    {
        string GetUserIdentity();

        string GetUserName();
    }
}
