using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class Abono
    {
        public int IdAbono { get; set; }
        public required string NumeroRecibo { get; set; }
        public int IdCuota { get; set; }
        public int IdUsuarioRegistro { get; set; }
        public DateTime FechaAbono { get; set; }
        public decimal MontoAbono { get; set; }
        public decimal MontoAplicadoMora { get; set; }
        public decimal MontoAplicadoCapital { get; set; }
        public required string MetodoPago { get; set; }
        public string? Observaciones { get; set; }
        public required string Estado { get; set; }
        public int? IdUsuarioAnulacion { get; set; }
        public DateTime? FechaAnulacion { get; set; }
        public string? MotivoAnulacion { get; set; }
    }
}
