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
    }
}
