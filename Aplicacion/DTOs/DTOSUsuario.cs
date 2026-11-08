using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTOs
{
    public class DTOSUsuario
    {
        public int IdUsuario { get; set; }

        [Display(Name = "Rol")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public int IdRol { get; set; }

        [StringLength(100)]
        [Display(Name = "Nombres")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public required string Nombres { get; set; }

        [StringLength(100)]
        [Display(Name = "Apellidos")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public required string Apellidos { get; set; }

        [StringLength(120)]
        [EmailAddress(ErrorMessage = "El correo no tiene un formato valido")]
        [Display(Name = "Correo")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public required string Correo { get; set; }

        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contrasena debe tener al menos 6 caracteres")]
        [DataType(DataType.Password)]
        [Display(Name = "Contrasena")]
        public string? Contraseña { get; set; }

        [StringLength(20)]
        [Display(Name = "Telefono")]
        public string? Telefono { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;

        [Display(Name = "Fecha De Creacion")]
        [DataType(DataType.DateTime)]
        public DateTime FechaCreacion { get; set; }
    }
}
