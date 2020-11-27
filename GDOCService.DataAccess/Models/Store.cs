using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GDOCService.DataAccess.Models
{
    [Table("Store")]
    public partial class Stores
    {
        /* Llave principal Autoincremental para todas las compañias. */
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StoreId { get; set; }

        /* Llave principal que indica el id de la compañia. */
        [Key]
        [Column(Order = 1)]
        public int CompanyID { get; set; }

        /* Variable que indica el Código de la tienda. */
        [Key]
        [Column(Order = 2)]
        [StringLength(20, ErrorMessage = "La longitud máxima de la tienda son 20 carácteres")]
        public string StoreCode { get; set; }

        /* Variable que indica el nombre de la tienda. */
        [Required(ErrorMessage ="El campo nombre lo necesito"),StringLength(50, ErrorMessage = "La longitud máxima del nombre de la tienda son 50 carácteres")]
        public string Name { get; set; }

        /* Variable que indica el Código del pais. */
        public string CountryID { get; set; }

        /* Variable que indica el Código del departamento. */
        public string StateID { get; set; }

        /* Variable que indica el Código de la ciudad. */
        public string CityID { get; set; }

        /* Variable que indica la dirección de la tienda. */
        [StringLength(100, ErrorMessage = "La longitud máxima de la dirección son 100 carácteres")]
        public string Address { get; set; }

        /* Variable que indica el teléfono de la tienda. */
        [StringLength(50, ErrorMessage = "La longitud máxima del telefono son 50 carácteres")]
        public string PhoneNum { get; set; }

        /* Variable que indica el correo de la tienda. */
        [StringLength(100, ErrorMessage = "La longitud máxima del correo electrónico son 100 carácteres")]
        public string EMailAddress { get; set; }

        #region SysControlFields

        /* Variable que indica si el registro está Activo o Inactivo. */
        public bool Inactive { get; set; }

        /* Variable que indica la fecha en la que se crea el registro. */
        public DateTime CreatedAt { get; set; } = new DateTime();

        /* Variable que indica la fecha en la que se modifica el registro. */
        public DateTime ModifyDate { get; set; } = new DateTime();

        /* Variable que indica el usuario que modifica el registro. */
        public string ModifiedBy { get; set; }

        /* Variable que indica el usuario que crea el registro. */
        public string CreatedBy { get; set; }

        #endregion

        [NotMapped]
        public string NombreAMostrar { get; set; }
    }
}
