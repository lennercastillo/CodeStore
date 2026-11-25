using System.ComponentModel.DataAnnotations;

namespace Aplicacion.DTOs
{
    public class CompraItemDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un producto válido")]
        public int IdProducto { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El costo unitario debe ser mayor a 0")]
        public decimal CostoUnitario { get; set; }
    }

    public class CompraRegistroDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un proveedor válido")]
        public int IdProveedor { get; set; }

        [StringLength(50, ErrorMessage = "El número de documento no puede exceder 50 caracteres")]
        public string? NumeroDocumento { get; set; }

        [StringLength(255, ErrorMessage = "Las observaciones no pueden exceder 255 caracteres")]
        public string? Observaciones { get; set; }

        public bool RecibirInmediatamente { get; set; } = true;

        [MinLength(1, ErrorMessage = "Debe agregar al menos un producto a la compra")]
        public List<CompraItemDTO> Items { get; set; } = new();
    }

    public class CompraDetalleCompletoDTO
    {
        public DTOSCompra Compra { get; set; } = null!;
        public DTOProveedor? Proveedor { get; set; }
        public List<CompraItemDetalleDTO> Detalles { get; set; } = new();
    }

    public class CompraItemDetalleDTO
    {
        public DTOsDetalleCompra Detalle { get; set; } = null!;
        public DTOSProductos? Producto { get; set; }
    }
}

