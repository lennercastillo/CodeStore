using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTOs
{
    public class DTOSAbono
    {
        public int IdAbono { get; set; }

        [StringLength(30)]
        [Display(Name = "Número de Recibo")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public required string NumeroRecibo { get; set; }

        [Display(Name = "Cuota")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public int IdCuota { get; set; }

        [Display(Name = "Usuario Registro")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public int IdUsuarioRegistro { get; set; }

        [Display(Name = "Fecha de Abono")]
        [DataType(DataType.DateTime)]
        public DateTime FechaAbono { get; set; }

        [Display(Name = "Monto de Abono")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
        public decimal MontoAbono { get; set; }

        [Display(Name = "Monto Aplicado a Mora")]
        [Range(0, double.MaxValue, ErrorMessage = "El monto no puede ser negativo")]
        public decimal MontoAplicadoMora { get; set; }

        [Display(Name = "Monto Aplicado a Capital")]
        [Range(0, double.MaxValue, ErrorMessage = "El monto no puede ser negativo")]
        public decimal MontoAplicadoCapital { get; set; }

        [StringLength(30)]
        [Display(Name = "Método de Pago")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public required string MetodoPago { get; set; }

        [StringLength(255)]
        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }

        [StringLength(20)]
        [Display(Name = "Estado")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public string Estado { get; set; } = "Activo";

        [Display(Name = "Usuario Anulación")]
        public int? IdUsuarioAnulacion { get; set; }

        [Display(Name = "Fecha de Anulación")]
        [DataType(DataType.DateTime)]
        public DateTime? FechaAnulacion { get; set; }

        [StringLength(255)]
        [Display(Name = "Motivo de Anulación")]
        public string? MotivoAnulacion { get; set; }
    }
}
