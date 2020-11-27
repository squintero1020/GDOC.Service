using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GDOCService.DataAccess.Models
{
    [Table("Guns")]
    public class Guns
    {
        public int Gunsid { get; set; }
        public string Name { get; set; }
        public bool Inactive { get; set; }

        public string Miguel { get; set; }
    }
}
