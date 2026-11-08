using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTOs
{
    public class DTOsDetalleCompra
    {
        public int IdDetalleCompra { get; set; }

        [Display(Name = "Compra")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public int IdCompra { get; set; }

        [Display(Name = "Producto")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public int IdProducto { get; set; }

        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }

        [Display(Name = "Costo Unitario")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El costo unitario debe ser mayor a 0")]
        public decimal CostoUnitario { get; set; }

        [Display(Name = "Subtotal")]
        public decimal Subtotal { get; set; }
    }
}
