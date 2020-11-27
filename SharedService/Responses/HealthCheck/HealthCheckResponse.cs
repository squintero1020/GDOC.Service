using System;
using System.Collections.Generic;
using System.Text;

namespace SharedService.Responses.HealthCheck
{
    public class HealthCheckResponse
    {
        public string status { get; set; }
        public IEnumerable<HealthCheck> Checks { get; set; }
        public TimeSpan Duration { get; set; }

    }
}
