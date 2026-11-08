using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class CuotaCredito
    {
        public int IdCuota { get; set; }
        public int IdCredito { get; set; }
        public int NumeroCuota { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public decimal MontoCuota { get; set; }
        public decimal RecargoMora { get; set; }
        public decimal MoraAbonada { get; set; }
        public decimal CapitalInteresAbonado { get; set; }
        public decimal MontoAbonado { get; set; }
        public decimal SaldoCuota { get; set; }
        public required string Estado { get; set; }
    }
}
