using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTOs
{
    public class DTOSCuotaCredito
    {
        public int IdCuota { get; set; }

        [Display(Name = "Crédito")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public int IdCredito { get; set; }

        [Display(Name = "Número de Cuota")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        [Range(1, int.MaxValue, ErrorMessage = "El número de cuota debe ser mayor a 0")]
        public int NumeroCuota { get; set; }

        [Display(Name = "Fecha de Vencimiento")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        [DataType(DataType.Date)]
        public DateTime FechaVencimiento { get; set; }

        [Display(Name = "Monto de Cuota")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
        public decimal MontoCuota { get; set; }

        [Display(Name = "Recargo por Mora")]
        [Range(0, double.MaxValue, ErrorMessage = "El recargo no puede ser negativo")]
        public decimal RecargoMora { get; set; }

        [Display(Name = "Mora Abonada")]
        [Range(0, double.MaxValue, ErrorMessage = "La mora abonada no puede ser negativa")]
        public decimal MoraAbonada { get; set; }

        [Display(Name = "Capital/Interés Abonado")]
        [Range(0, double.MaxValue, ErrorMessage = "El valor abonado no puede ser negativo")]
        public decimal CapitalInteresAbonado { get; set; }

        [Display(Name = "Monto Abonado")]
        public decimal MontoAbonado { get; set; }

        [Display(Name = "Saldo de Cuota")]
        public decimal SaldoCuota { get; set; }

        [StringLength(20)]
        [Display(Name = "Estado")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public string Estado { get; set; } = "Pendiente";
    }
}
