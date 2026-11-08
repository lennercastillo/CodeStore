using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTOs
{
    public class DTOSDetalleVenta
    {
        public int IdDetalleVenta { get; set; }

        [Display(Name = "Venta")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public int IdVenta { get; set; }

        [Display(Name = "Producto")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public int IdProducto { get; set; }

        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }

        [Display(Name = "Precio Unitario")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        [Range(0, double.MaxValue, ErrorMessage = "El precio unitario no puede ser negativo")]
        public decimal PrecioUnitario { get; set; }

        [Display(Name = "Costo Unitario Histórico")]
        [Range(0, double.MaxValue, ErrorMessage = "El costo histórico no puede ser negativo")]
        public decimal CostoUnitarioHistorico { get; set; }

        [Display(Name = "Subtotal")]
        public decimal Subtotal { get; set; }

        [Display(Name = "Meses de Garantía")]
        [Range(0, int.MaxValue, ErrorMessage = "Los meses no pueden ser negativos")]
        public int MesesGarantiaHistorico { get; set; }

        [Display(Name = "Fin de Garantía")]
        [DataType(DataType.Date)]
        public DateTime? FechaFinGarantia { get; set; }
    }
}
