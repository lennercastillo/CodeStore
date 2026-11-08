using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTOs
{
    public class DTOProveedor
    {
        public int IdProveedor { get; set; }

        [StringLength(25)]
        [Display(Name = "RUC")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public required string NumeroRUC { get; set; }

        [StringLength(150)]
        [Display(Name = "Razón Social")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public required string RazonSocial { get; set; }

        [StringLength(100)]
        [Display(Name = "Nombre de Contacto")]
        public string? NombreContacto { get; set; }

        [StringLength(20)]
        [Display(Name = "Teléfono")]
        public string? Telefono { get; set; }

        [StringLength(120)]
        [EmailAddress(ErrorMessage = "El correo no tiene un formato válido")]
        [Display(Name = "Correo")]
        public string? Correo { get; set; }

        [StringLength(250)]
        [Display(Name = "Dirección")]
        public string? Direccion { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;

        [Display(Name = "Fecha De Registro")]
        [DataType(DataType.DateTime)]
        public DateTime FechaRegistro { get; set; }
    }
}
