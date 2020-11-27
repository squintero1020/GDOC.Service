using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GDOCService.DataAccess.Models
{
    public class Panes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PanId { get; set; }
        [Required(ErrorMessage ="Requerido")]
        public string Name { get; set; }
        public string flour { get; set; }
    }
}
