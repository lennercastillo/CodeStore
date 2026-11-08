using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTOs
{
    public class DTOsClientes
    {
        public int IdCliente { get; set; }

        [StringLength(25)]
        [Display(Name = "Numero De Identificacion")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public required string NumeroIdentificacion { get; set; }

        [StringLength(100)]
        [Display(Name = "Nombres")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public required string Nombres { get; set; }

        [StringLength(100)]
        [Display(Name = "Apellidos")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public required string Apellidos { get; set; }

        [StringLength(20)]
        [Display(Name = "Telefono")]
        public string? Telefono { get; set; }

        [StringLength(120)]
        [EmailAddress(ErrorMessage = "El correo no tiene un formato valido")]
        [Display(Name = "Correo")]
        public string? Correo { get; set; }

        [StringLength(250)]
        [Display(Name = "Direccion")]
        public string? Direccion { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;

        [Display(Name = "Fecha De Registro")]
        [DataType(DataType.DateTime)]
        public DateTime FechaRegistro { get; set; }
    }
}
