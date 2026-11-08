using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class Kardex
    {
        public int IdMovimiento { get; set; }
        public int IdProducto { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public required string TipoMovimiento { get; set; }
        public int Cantidad { get; set; }
        public int StockAnterior { get; set; }
        public int StockNuevo { get; set; }
        public decimal CostoUnitario { get; set; }
        public string? DocumentoOrigen { get; set; }
        public int? IdReferenciaOrigen { get; set; }
        public string? Justificacion { get; set; }
        public int IdUsuarioResponsable { get; set; }
    }
}
