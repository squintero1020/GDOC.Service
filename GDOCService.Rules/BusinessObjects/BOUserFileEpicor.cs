using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Ice.Tablesets;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharedService.Functions;
using SharedService.Models;
using SharedService.Responses.Response;

namespace GDOCService.Rules.BusinessObjects
{
    public class BOUserFileEpicor
    {
        PetitionEpicor cn = new PetitionEpicor();

        public IConfiguration _IConfiguration { get; set; }

        public BOUserFileEpicor(IConfiguration Configuration) => _IConfiguration = Configuration;


        public async Task<PetitionResponse> GetRows(string Where)
        {
            try
            {


                UserFileGetRows getRows = new UserFileGetRows { 
                    whereClauseUserFile = Where,
                    whereClauseUserComp = "",
                    whereClauseUserCompExt = "",
                    pageSize = 0,
                    absolutePage = 0
                };

                string Error = string.Empty;
                var response = (cn.PostRest(_IConfiguration, "Erp.BO.UserFileSvc/", "GetRows", "returnObj", JsonConvert.SerializeObject(getRows), out Error) as JObject);

                if (response != null)
                {
                    var result = response.ToObject<DataSet>();

                    return new PetitionResponse
                    {
                        success = true,
                        message = "Consulta de usuarios realizada con éxito!",
                        module = "UserFileEpicor",
                        URL = "api/UserFileEpicor/GetRows",
                        result = JsonConvert.SerializeObject(result)
                    };
                }
                else {
                    return new PetitionResponse
                    {
                        success = true,
                        message = "No es posible consultar los usuarios",
                        module = "UserFileEpicor",
                        URL = "api/UserFileEpicor/GetRows",
                        result = null
                    };
                }

            }
            catch (Exception ex)
            {
                return new PetitionResponse
                {
                    success = false,
                    message = "No es posible consultar los usuarios: " + ex.Message,
                    module = "UserFileEpicor",
                    URL = "api/UserFileEpicor/GetRows",
                    result = null
                };
            }
        }
    }
}
