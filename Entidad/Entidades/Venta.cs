using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class Venta
    {
        public int IdVenta { get; set; }
        public required string NumeroFactura { get; set; }
        public int IdCliente { get; set; }
        public int IdUsuarioVendedor { get; set; }
        public DateTime FechaVenta { get; set; }
        public required string TipoVenta { get; set; }
        public required string MetodoPago { get; set; }
        public decimal PorcentajeImpuesto { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Impuesto { get; set; }
        public decimal TotalVenta { get; set; }
        public decimal MontoRecibido { get; set; }
        public decimal Vuelto { get; set; }
        public required string Estado { get; set; }
        public int? IdUsuarioAutorizaAnulacion { get; set; }
        public DateTime? FechaAnulacion { get; set; }
        public string? MotivoAnulacion { get; set; }
    }
}
