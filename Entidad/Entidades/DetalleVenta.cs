using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class DetalleVenta
    {
        public int IdDetalleVenta { get; set; }
        public int IdVenta { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal CostoUnitarioHistorico { get; set; }
        public decimal Subtotal { get; set; }
        public int MesesGarantiaHistorico { get; set; }
        public DateTime? FechaFinGarantia { get; set; }
    }
}
