using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class DetalleVentaSerie
    {
        public int IdSerieVendida { get; set; }
        public int IdDetalleVenta { get; set; }
        public required string NumeroSerie { get; set; }
        public required string EstadoSerie { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
