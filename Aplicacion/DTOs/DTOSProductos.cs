using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTOs
{
    public class DTOSProductos
    {
        public int IdProducto { get; set; }

        [StringLength(50)]
        [Display(Name = "Código SKU")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public required string CodigoSKU { get; set; }

        [StringLength(150)]
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public required string Nombre { get; set; }

        [StringLength(80)]
        [Display(Name = "Marca")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public required string Marca { get; set; }

        [StringLength(80)]
        [Display(Name = "Modelo")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public required string Modelo { get; set; }

        [StringLength(80)]
        [Display(Name = "Tipo de Equipo")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public required string TipoEquipo { get; set; }

        [StringLength(255)]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Display(Name = "Precio de Venta")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        [Range(0, double.MaxValue, ErrorMessage = "El valor no puede ser negativo")]
        public decimal PrecioVenta { get; set; }

        [Display(Name = "Último Costo de Compra")]
        [Range(0, double.MaxValue, ErrorMessage = "El valor no puede ser negativo")]
        public decimal UltimoCostoCompra { get; set; }

        [Display(Name = "Costo Promedio")]
        [Range(0, double.MaxValue, ErrorMessage = "El valor no puede ser negativo")]
        public decimal CostoPromedio { get; set; }

        [Display(Name = "Stock Actual")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int StockActual { get; set; }

        [Display(Name = "Stock Mínimo")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock mínimo no puede ser negativo")]
        public int StockMinimo { get; set; } = 1;

        [Display(Name = "Meses de Garantía")]
        [Range(0, int.MaxValue, ErrorMessage = "Los meses de garantía no pueden ser negativos")]
        public int MesesGarantia { get; set; }

        [Display(Name = "Requiere Serie")]
        public bool RequiereSerie { get; set; } = false;

        [Display(Name = "Proveedor Preferido")]
        public int? IdProveedorPreferido { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;

        [Display(Name = "Fecha de Registro")]
        [DataType(DataType.DateTime)]
        public DateTime FechaRegistro { get; set; }
    }
}
