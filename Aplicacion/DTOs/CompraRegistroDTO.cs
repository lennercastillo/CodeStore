using System.ComponentModel.DataAnnotations;

namespace Aplicacion.DTOs
{
    public class CompraItemDTO
    {
        [Required]
        public int IdProducto { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El costo unitario debe ser mayor a 0")]
        public decimal CostoUnitario { get; set; }
    }

    public class CompraRegistroDTO
    {
        [Required(ErrorMessage = "Debe seleccionar un proveedor")]
        public int IdProveedor { get; set; }

        public string? NumeroDocumento { get; set; }
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

