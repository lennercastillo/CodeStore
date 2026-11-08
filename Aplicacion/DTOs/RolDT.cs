using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTOs
{
    public class RolDT
    {

        public int IdRol { get; set; } 


        [StringLength(10)]
        [Display(Name = "Codigo")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public required string CodigoRol { get; set; }

        [StringLength(50)]
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public required string NombreRol { get; set; }

        [StringLength(255)]
        [Display(Name = "Descripcion")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public required string Descripcion { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;

        [Display(Name = "Fecha De Creacion")]
        [DataType(DataType.DateTime)]
        public DateTime FechaCreacion { get; set; }
    }
}
