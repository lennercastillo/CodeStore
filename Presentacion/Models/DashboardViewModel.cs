using Aplicacion.DTOs;

namespace Presentacion.Models
{
    public class DashboardViewModel
    {
        public decimal VentasDelMes { get; set; }
        public int CantidadVentasMes { get; set; }
        public int TotalClientesActivos { get; set; }
        public int EquiposEnTaller { get; set; }
        public int ProductosStockBajo { get; set; }
        public decimal CarteraCreditosPendiente { get; set; }

        public List<DTOSVenta> UltimasVentas { get; set; } = new();
        public Dictionary<int, string> ClientesNombres { get; set; } = new();
        public List<DTOSProductos> ProductosCriticos { get; set; } = new();
        public List<DTOSOrdenReparacion> OrdenesRecientes { get; set; } = new();
    }
}

