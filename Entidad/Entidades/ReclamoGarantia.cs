using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class ReclamoGarantia
    {
        public int IdGarantia { get; set; }
        public required string NumeroReclamo { get; set; }
        public int IdDetalleVenta { get; set; }
        public int? IdSerieVendida { get; set; }
        public int IdUsuarioTecnico { get; set; }
        public DateTime FechaReclamo { get; set; }
        public required string MotivoFalla { get; set; }
        public required string EstadoRevision { get; set; }
        public string? AccionTomada { get; set; }
        public string? DescripcionResolucion { get; set; }
        public DateTime? FechaResolucion { get; set; }
    }
}
