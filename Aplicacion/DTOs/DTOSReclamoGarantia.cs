using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTOs
{
    public class DTOSReclamoGarantia
    {
        public int IdGarantia { get; set; }

        [StringLength(30)]
        [Display(Name = "Número de Reclamo")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public required string NumeroReclamo { get; set; }

        [Display(Name = "Detalle de Venta")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public int IdDetalleVenta { get; set; }

        [Display(Name = "Serie Vendida")]
        public int? IdSerieVendida { get; set; }

        [Display(Name = "Usuario Técnico")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public int IdUsuarioTecnico { get; set; }

        [Display(Name = "Fecha de Reclamo")]
        [DataType(DataType.DateTime)]
        public DateTime FechaReclamo { get; set; }

        [StringLength(500)]
        [Display(Name = "Motivo de Falla")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public required string MotivoFalla { get; set; }

        [StringLength(30)]
        [Display(Name = "Estado de Revisión")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public string EstadoRevision { get; set; } = "Recibido";

        [StringLength(40)]
        [Display(Name = "Acción Tomada")]
        public string? AccionTomada { get; set; }

        [StringLength(500)]
        [Display(Name = "Descripción de Resolución")]
        public string? DescripcionResolucion { get; set; }

        [Display(Name = "Fecha de Resolución")]
        [DataType(DataType.DateTime)]
        public DateTime? FechaResolucion { get; set; }
    }
}
