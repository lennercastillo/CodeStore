using Aplicacion.DTOs;
using Aplicacion.Interface;
using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Services
{
    public class KardexServices
    {
        private readonly IKardex _repo;

        public KardexServices(IKardex reposi)
        {
            _repo = reposi;
        }

        public async Task<IEnumerable<DTOSKardex>> ListarKardex()
        {
            var Lista = await _repo.ListarKardexAsync();

            return Lista.Select(li => new DTOSKardex
            {
                IdMovimiento = li.IdMovimiento,
                IdProducto = li.IdProducto,
                FechaMovimiento = li.FechaMovimiento,
                TipoMovimiento = li.TipoMovimiento,
                Cantidad = li.Cantidad,
                StockAnterior = li.StockAnterior,
                StockNuevo = li.StockNuevo,
                CostoUnitario = li.CostoUnitario,
                DocumentoOrigen = li.DocumentoOrigen,
                IdReferenciaOrigen = li.IdReferenciaOrigen,
                Justificacion = li.Justificacion,
                IdUsuarioResponsable = li.IdUsuarioResponsable
            });
        }

        public async Task NuevoKardex(DTOSKardex dt)
        {
            var nk = new Kardex
            {
                IdMovimiento = dt.IdMovimiento,
                IdProducto = dt.IdProducto,
                FechaMovimiento = dt.FechaMovimiento,
                TipoMovimiento = dt.TipoMovimiento,
                Cantidad = dt.Cantidad,
                StockAnterior = dt.StockAnterior,
                StockNuevo = dt.StockNuevo,
                CostoUnitario = dt.CostoUnitario,
                DocumentoOrigen = dt.DocumentoOrigen,
                IdReferenciaOrigen = dt.IdReferenciaOrigen,
                Justificacion = dt.Justificacion,
                IdUsuarioResponsable = dt.IdUsuarioResponsable
            };
            await _repo.NuevoKardexAsync(nk);
        }

        public async Task EditarKardex(DTOSKardex dt)
        {
            var ek = new Kardex
            {
                IdMovimiento = dt.IdMovimiento,
                IdProducto = dt.IdProducto,
                FechaMovimiento = dt.FechaMovimiento,
                TipoMovimiento = dt.TipoMovimiento,
                Cantidad = dt.Cantidad,
                StockAnterior = dt.StockAnterior,
                StockNuevo = dt.StockNuevo,
                CostoUnitario = dt.CostoUnitario,
                DocumentoOrigen = dt.DocumentoOrigen,
                IdReferenciaOrigen = dt.IdReferenciaOrigen,
                Justificacion = dt.Justificacion,
                IdUsuarioResponsable = dt.IdUsuarioResponsable
            };
            await _repo.EditarKardexAsync(ek);
        }

        public async Task EliminarKardex(int id)
        {
            await _repo.EliminarKardexAsync(id);
        }

        public async Task<(bool Success, string Message)> RegistrarAjuste(
            int idProducto,
            string tipoMovimiento,
            int cantidad,
            string justificacion,
            int idUsuario)
        {
            if (idProducto <= 0)
                return (false, "Debe seleccionar un producto válido.");
            if (cantidad <= 0)
                return (false, "La cantidad debe ser mayor a cero.");
            if (idUsuario <= 0)
                return (false, "Usuario inválido.");
            if (string.IsNullOrWhiteSpace(justificacion))
                return (false, "La justificación del ajuste es obligatoria.");

            string? tipoNorm = new[] { "Ajuste de Entrada", "Ajuste de Salida", "Baja de Inventario", "Salida por Reemplazo Garantia" }
                .FirstOrDefault(t => string.Equals(t, tipoMovimiento?.Trim(), StringComparison.OrdinalIgnoreCase));
            if (tipoNorm == null)
                return (false, "Tipo de movimiento no permitido para ajuste manual.");

            string justLimpia = justificacion.Trim();
            if (justLimpia.Length > 255)
                justLimpia = justLimpia[..255];

            return await _repo.RegistrarAjusteInventarioAsync(idProducto, tipoNorm, cantidad, justLimpia, idUsuario);
        }
    }
}
