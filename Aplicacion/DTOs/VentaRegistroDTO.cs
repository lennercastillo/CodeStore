using System.ComponentModel.DataAnnotations;

namespace Aplicacion.DTOs
{
    public class VentaItemDTO
    {
        [Required]
        public int IdProducto { get; set; }

        public string Nombre { get; set; } = string.Empty;
        public string CodigoSKU { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser mayor o igual a 0")]
        public decimal PrecioUnitario { get; set; }

        public decimal Subtotal => Cantidad * PrecioUnitario;

        public bool RequiereSerie { get; set; } = false;
        public List<string> Series { get; set; } = new();
    }

    public class VentaRegistroDTO
    {
        [Required(ErrorMessage = "Debe seleccionar un cliente")]
        public int IdCliente { get; set; }

        public string? NumeroFactura { get; set; }

        [Required]
        public string TipoVenta { get; set; } = "Contado"; // "Contado" o "Credito"

        [Required]
        public string MetodoPago { get; set; } = "Efectivo"; // "Efectivo", "Tarjeta", "Transferencia", "Credito"

        [Range(0, 100)]
        public decimal PorcentajeImpuesto { get; set; } = 15.00m;

        [Range(0, double.MaxValue)]
        public decimal MontoRecibido { get; set; }

        public string? Observaciones { get; set; }

        [MinLength(1, ErrorMessage = "Debe agregar al menos un producto a la venta")]
        public List<VentaItemDTO> Items { get; set; } = new();

        // Campos para Crédito
        public decimal PrimaInicial { get; set; } = 0;
        public int CantidadCuotas { get; set; } = 1;
        public string FrecuenciaPago { get; set; } = "Mensual"; // "Semanal", "Quincenal", "Mensual"
        public decimal PorcentajeInteres { get; set; } = 0;
    }

    public class VentaDetalleCompletoDTO
    {
        public DTOSVenta Venta { get; set; } = null!;
        public DTOsClientes? Cliente { get; set; }
        public DTOSUsuario? Vendedor { get; set; }
        public List<ItemDetalleCompletoDTO> Detalles { get; set; } = new();
        public DTOSCredito? Credito { get; set; }
        public List<DTOSCuotaCredito> Cuotas { get; set; } = new();
    }

    public class ItemDetalleCompletoDTO
    {
        public DTOSDetalleVenta Detalle { get; set; } = null!;
        public DTOSProductos? Producto { get; set; }
        public List<string> Series { get; set; } = new();
    }
}
