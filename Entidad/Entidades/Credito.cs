using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class Credito
    {
        public int IdCredito { get; set; }
        public int IdVenta { get; set; }
        public decimal MontoVentaTotal { get; set; }
        public decimal PrimaInicial { get; set; }
        public decimal MontoFinanciado { get; set; }
        public required string FrecuenciaPago { get; set; }
        public int CantidadCuotas { get; set; }
        public decimal PorcentajeInteres { get; set; }
        public decimal MontoInteresesPactados { get; set; }
        public decimal MontoTotalCredito { get; set; }
        public decimal SaldoPendiente { get; set; }
        public required string Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
