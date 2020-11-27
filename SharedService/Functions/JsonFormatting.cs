using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SharedService.Functions
{
    public class JsonFormatting
    {
        public static string Formated(object obj)
        {
            return JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
        }
    }
}
