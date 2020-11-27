using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace SharedService.Functions
{
    public class PetitionEpicor
    {
        public object PostRest(IConfiguration _config,string Method, string Inst, string Type, string entity, out string Error)
        {
            object obj = new object();
            Error = string.Empty;
            try
            {
                var client = new RestSharp.RestClient(_config.GetConnectionString("UrlConnectEpicor") + Method + Inst);
                var request = new RestSharp.RestRequest(RestSharp.Method.POST);
                request.AddHeader("authorization", "Basic " + Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", _config["UserEpicor"], _config["PassEpicor"]))));
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", entity, RestSharp.ParameterType.RequestBody);
                var response = client.Execute(request);
                if (response.StatusCode.ToString() != "OK")
                    Error = response.Content;
                
                obj = JObject.Parse(response.Content).SelectToken(Type, false);
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
            return obj;
        }
    }
}
