using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class Productos
    {
        public int IdProducto { get; set; }
        public required string CodigoSKU { get; set; }
        public required string Nombre { get; set; }
        public required string Marca { get; set; }
        public required string Modelo { get; set; }
        public required string TipoEquipo { get; set; }
        public string? Descripcion { get; set; }
        public decimal PrecioVenta { get; set; }
        public decimal UltimoCostoCompra { get; set; }
        public decimal CostoPromedio { get; set; }
        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
        public int MesesGarantia { get; set; }
        public bool RequiereSerie { get; set; }
        public int? IdProveedorPreferido { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
