using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTOs
{
    public class DTOSCompra
    {
        public int IdCompra { get; set; }

        [StringLength(50)]
        [Display(Name = "Número de Documento")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public required string NumeroDocumento { get; set; }

        [Display(Name = "Proveedor")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public int IdProveedor { get; set; }

        [Display(Name = "Usuario de Registro")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public int IdUsuarioRegistro { get; set; }

        [Display(Name = "Usuario de Recepción")]
        public int? IdUsuarioRecepcion { get; set; }

        [Display(Name = "Fecha de Compra")]
        [DataType(DataType.DateTime)]
        public DateTime FechaCompra { get; set; }

        [Display(Name = "Fecha de Recepción")]
        [DataType(DataType.DateTime)]
        public DateTime? FechaRecepcion { get; set; }

        [Display(Name = "Total de Compra")]
        [Range(0, double.MaxValue, ErrorMessage = "El total no puede ser negativo")]
        public decimal TotalCompra { get; set; }

        [StringLength(20)]
        [Display(Name = "Estado")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public string Estado { get; set; } = "Pendiente";

        [StringLength(255)]
        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }

        [Display(Name = "Usuario Anulación")]
        public int? IdUsuarioAnulacion { get; set; }

        [Display(Name = "Fecha de Anulación")]
        [DataType(DataType.DateTime)]
        public DateTime? FechaAnulacion { get; set; }

        [StringLength(255)]
        [Display(Name = "Motivo de Anulación")]
        public string? MotivoAnulacion { get; set; }
    }
}
