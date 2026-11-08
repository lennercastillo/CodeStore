using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTOs
{
    public class DTOSKardex
    {
        public int IdMovimiento { get; set; }

        [Display(Name = "Producto")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public int IdProducto { get; set; }

        [Display(Name = "Fecha del Movimiento")]
        [DataType(DataType.DateTime)]
        public DateTime FechaMovimiento { get; set; }

        [StringLength(40)]
        [Display(Name = "Tipo de Movimiento")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public required string TipoMovimiento { get; set; }

        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }

        [Display(Name = "Stock Anterior")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock anterior no puede ser negativo")]
        public int StockAnterior { get; set; }

        [Display(Name = "Stock Nuevo")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock nuevo no puede ser negativo")]
        public int StockNuevo { get; set; }

        [Display(Name = "Costo Unitario")]
        [Range(0, double.MaxValue, ErrorMessage = "El costo unitario no puede ser negativo")]
        public decimal CostoUnitario { get; set; }

        [StringLength(50)]
        [Display(Name = "Documento de Origen")]
        public string? DocumentoOrigen { get; set; }

        [Display(Name = "Referencia de Origen")]
        public int? IdReferenciaOrigen { get; set; }

        [StringLength(255)]
        [Display(Name = "Justificación")]
        public string? Justificacion { get; set; }

        [Display(Name = "Usuario Responsable")]
        [Required(ErrorMessage = "Es Obligatorio llenar Este Espacio")]
        public int IdUsuarioResponsable { get; set; }
    }
}
