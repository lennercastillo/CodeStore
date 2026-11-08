using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTOs
{
    public class DTOSVenta
    {
        public int IdVenta { get; set; }

        [StringLength(30)]
        [Display(Name = "Número de Factura")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public required string NumeroFactura { get; set; }

        [Display(Name = "Cliente")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public int IdCliente { get; set; }

        [Display(Name = "Usuario Vendedor")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public int IdUsuarioVendedor { get; set; }

        [Display(Name = "Fecha de Venta")]
        [DataType(DataType.DateTime)]
        public DateTime FechaVenta { get; set; }

        [StringLength(20)]
        [Display(Name = "Tipo de Venta")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public required string TipoVenta { get; set; }

        [StringLength(30)]
        [Display(Name = "Método de Pago")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public required string MetodoPago { get; set; }

        [Display(Name = "Porcentaje Impuesto")]
        [Range(0, 100, ErrorMessage = "El porcentaje debe estar entre 0 y 100")]
        public decimal PorcentajeImpuesto { get; set; } = 15.00m;

        [Display(Name = "Subtotal")]
        [Range(0, double.MaxValue, ErrorMessage = "El subtotal no puede ser negativo")]
        public decimal Subtotal { get; set; }

        [Display(Name = "Impuesto")]
        [Range(0, double.MaxValue, ErrorMessage = "El impuesto no puede ser negativo")]
        public decimal Impuesto { get; set; }

        [Display(Name = "Total Venta")]
        [Range(0, double.MaxValue, ErrorMessage = "El total no puede ser negativo")]
        public decimal TotalVenta { get; set; }

        [Display(Name = "Monto Recibido")]
        [Range(0, double.MaxValue, ErrorMessage = "El monto recibido no puede ser negativo")]
        public decimal MontoRecibido { get; set; }

        [Display(Name = "Vuelto")]
        [Range(0, double.MaxValue, ErrorMessage = "El vuelto no puede ser negativo")]
        public decimal Vuelto { get; set; }

        [StringLength(20)]
        [Display(Name = "Estado")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public string Estado { get; set; } = "Pendiente";

        [Display(Name = "Usuario Autoriza Anulación")]
        public int? IdUsuarioAutorizaAnulacion { get; set; }

        [Display(Name = "Fecha de Anulación")]
        [DataType(DataType.DateTime)]
        public DateTime? FechaAnulacion { get; set; }

        [StringLength(255)]
        [Display(Name = "Motivo de Anulación")]
        public string? MotivoAnulacion { get; set; }
    }
}
