using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTOs
{
    public class DTOSOrdenReparacion
    {
        public int IdOrdenReparacion { get; set; }

        [StringLength(30)]
        [Display(Name = "Número de Orden")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public required string NumeroOrden { get; set; }

        [Display(Name = "Garantía")]
        public int? IdGarantia { get; set; }

        [Display(Name = "Cliente")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public int IdCliente { get; set; }

        [Display(Name = "Usuario Técnico")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public int IdUsuarioTecnico { get; set; }

        [StringLength(150)]
        [Display(Name = "Descripción del Equipo")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public required string DescripcionEquipo { get; set; }

        [StringLength(80)]
        [Display(Name = "Marca")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public required string Marca { get; set; }

        [StringLength(80)]
        [Display(Name = "Modelo")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public required string Modelo { get; set; }

        [StringLength(100)]
        [Display(Name = "Número de Serie")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public required string NumeroSerie { get; set; }

        [StringLength(500)]
        [Display(Name = "Diagnóstico Técnico")]
        public string? DiagnosticoTecnico { get; set; }

        [StringLength(30)]
        [Display(Name = "Estado del Equipo")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public string EstadoEquipo { get; set; } = "Recibido";

        [Display(Name = "Costo de Reparación")]
        [Range(0, double.MaxValue, ErrorMessage = "El costo no puede ser negativo")]
        public decimal CostoReparacion { get; set; }

        [Display(Name = "Fecha de Ingreso")]
        [DataType(DataType.DateTime)]
        public DateTime FechaIngreso { get; set; }

        [Display(Name = "Fecha de Finalización")]
        [DataType(DataType.DateTime)]
        public DateTime? FechaFinalizacion { get; set; }

        [Display(Name = "Fecha de Entrega")]
        [DataType(DataType.DateTime)]
        public DateTime? FechaEntrega { get; set; }

        [StringLength(500)]
        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }
    }
}
