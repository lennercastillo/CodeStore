using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class Compra
    {
        public int IdCompra { get; set; }
        public required string NumeroDocumento { get; set; }
        public int IdProveedor { get; set; }
        public int IdUsuarioRegistro { get; set; }
        public int? IdUsuarioRecepcion { get; set; }
        public DateTime FechaCompra { get; set; }
        public DateTime? FechaRecepcion { get; set; }
        public decimal TotalCompra { get; set; }
        public required string Estado { get; set; }
        public string? Observaciones { get; set; }
        public int? IdUsuarioAnulacion { get; set; }
        public DateTime? FechaAnulacion { get; set; }
        public string? MotivoAnulacion { get; set; }
    }
}
