using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class OrdenReparacion
    {
        public int IdOrdenReparacion { get; set; }
        public required string NumeroOrden { get; set; }
        public int? IdGarantia { get; set; }
        public int IdCliente { get; set; }
        public int IdUsuarioTecnico { get; set; }
        public required string DescripcionEquipo { get; set; }
        public required string Marca { get; set; }
        public required string Modelo { get; set; }
        public required string NumeroSerie { get; set; }
        public string? DiagnosticoTecnico { get; set; }
        public required string EstadoEquipo { get; set; }
        public decimal CostoReparacion { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime? FechaFinalizacion { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public string? Observaciones { get; set; }
    }
}
