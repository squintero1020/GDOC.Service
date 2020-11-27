using System;
using System.Collections.Generic;
using System.Text;

namespace SharedService.Models
{
    public class UserFileGetRows
    {
        public string whereClauseUserFile { get; set; }
        public string whereClauseUserComp { get; set; }
        public string whereClauseUserCompExt { get; set; }
        public int pageSize { get; set; }
        public int absolutePage { get; set; }
    }
}
