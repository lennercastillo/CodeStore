using System.ComponentModel.DataAnnotations;

namespace Aplicacion.DTOs
{
    public class VentaItemDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un producto válido")]
        public int IdProducto { get; set; }

        public string? Nombre { get; set; }
        public string? CodigoSKU { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser mayor o igual a 0")]
        public decimal PrecioUnitario { get; set; }

        public decimal Subtotal => Cantidad * PrecioUnitario;

        public bool RequiereSerie { get; set; } = false;
        public List<string>? Series { get; set; } = new();
    }

    public class VentaRegistroDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un cliente válido")]
        public int IdCliente { get; set; }

        [StringLength(30, ErrorMessage = "El número de factura no puede exceder 30 caracteres")]
        public string? NumeroFactura { get; set; }

        [Required]
        public string TipoVenta { get; set; } = "Contado"; // "Contado" o "Credito"

        [Required]
        public string MetodoPago { get; set; } = "Efectivo"; // "Efectivo", "Tarjeta", "Transferencia", "Mixto", "Credito"

        [Range(0, 100, ErrorMessage = "El impuesto debe estar entre 0% y 100%")]
        public decimal PorcentajeImpuesto { get; set; } = 15.00m;

        [Range(0, double.MaxValue, ErrorMessage = "El monto recibido no puede ser negativo")]
        public decimal MontoRecibido { get; set; }

        [StringLength(255)]
        public string? Observaciones { get; set; }

        [MinLength(1, ErrorMessage = "Debe agregar al menos un producto a la venta")]
        public List<VentaItemDTO> Items { get; set; } = new();

        // Campos para Crédito
        [Range(0, double.MaxValue, ErrorMessage = "La prima inicial no puede ser negativa")]
        public decimal PrimaInicial { get; set; } = 0;

        [Range(0, 120, ErrorMessage = "La cantidad de cuotas debe estar entre 1 y 120")]
        public int CantidadCuotas { get; set; } = 1;

        public string? FrecuenciaPago { get; set; } = "Mensual"; // "Semanal", "Quincenal", "Mensual"

        [Range(0, 100, ErrorMessage = "El porcentaje de interés no puede ser negativo")]
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
