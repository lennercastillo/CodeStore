using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTOs
{
    public class DTOSDetalleVentaSerie
    {
        public int IdSerieVendida { get; set; }

        [Display(Name = "Detalle de Venta")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public int IdDetalleVenta { get; set; }

        [StringLength(100)]
        [Display(Name = "Número de Serie")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public required string NumeroSerie { get; set; }

        [StringLength(30)]
        [Display(Name = "Estado de Serie")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public string EstadoSerie { get; set; } = "Vendido";

        [Display(Name = "Fecha de Registro")]
        [DataType(DataType.DateTime)]
        public DateTime FechaRegistro { get; set; }
    }
}
