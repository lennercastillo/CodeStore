using System.ComponentModel.DataAnnotations;

namespace Presentacion.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El correo electrónico es requerido")]
        [EmailAddress(ErrorMessage = "Ingrese un correo electrónico válido")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [DataType(DataType.Password)]
        public string Contraseña { get; set; } = string.Empty;

        public bool Recordarme { get; set; } = false;

        public string? ReturnUrl { get; set; }
    }
}

