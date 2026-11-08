using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTOs
{
    public class DTOSCredito
    {
        public int IdCredito { get; set; }

        [Display(Name = "Venta")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public int IdVenta { get; set; }

        [Display(Name = "Monto Venta Total")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
        public decimal MontoVentaTotal { get; set; }

        [Display(Name = "Prima Inicial")]
        [Range(0, double.MaxValue, ErrorMessage = "La prima no puede ser negativa")]
        public decimal PrimaInicial { get; set; }

        [Display(Name = "Monto Financiado")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto financiado debe ser mayor a 0")]
        public decimal MontoFinanciado { get; set; }

        [StringLength(20)]
        [Display(Name = "Frecuencia de Pago")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public required string FrecuenciaPago { get; set; }

        [Display(Name = "Cantidad de Cuotas")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad de cuotas debe ser mayor a 0")]
        public int CantidadCuotas { get; set; }

        [Display(Name = "Porcentaje de Interés")]
        [Range(0, 100, ErrorMessage = "El interés debe estar entre 0 y 100")]
        public decimal PorcentajeInteres { get; set; }

        [Display(Name = "Monto Intereses Pactados")]
        [Range(0, double.MaxValue, ErrorMessage = "El valor no puede ser negativo")]
        public decimal MontoInteresesPactados { get; set; }

        [Display(Name = "Monto Total Crédito")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto total debe ser mayor a 0")]
        public decimal MontoTotalCredito { get; set; }

        [Display(Name = "Saldo Pendiente")]
        [Range(0, double.MaxValue, ErrorMessage = "El saldo no puede ser negativo")]
        public decimal SaldoPendiente { get; set; }

        [StringLength(20)]
        [Display(Name = "Estado")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public string Estado { get; set; } = "Activo";

        [Display(Name = "Fecha de Registro")]
        [DataType(DataType.DateTime)]
        public DateTime FechaRegistro { get; set; }
    }
}
