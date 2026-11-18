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
    public class DetalleVentaServices
    {
        private readonly IDetalleVenta _repo;

        public DetalleVentaServices(IDetalleVenta reposi)
        {
            _repo = reposi;
        }

        public async Task<IEnumerable<DTOSDetalleVenta>> ListarDetalleVenta()
        {
            var Lista = await _repo.ListarDetalleVentaAsync();

            return Lista.Select(li => new DTOSDetalleVenta
            {
                IdDetalleVenta = li.IdDetalleVenta,
                IdVenta = li.IdVenta,
                IdProducto = li.IdProducto,
                Cantidad = li.Cantidad,
                PrecioUnitario = li.PrecioUnitario,
                CostoUnitarioHistorico = li.CostoUnitarioHistorico,
                Subtotal = li.Subtotal,
                MesesGarantiaHistorico = li.MesesGarantiaHistorico,
                FechaFinGarantia = li.FechaFinGarantia
            });
        }

        public async Task NuevoDetalleVenta(DTOSDetalleVenta dt)
        {
            var nd = new DetalleVenta
            {
                IdDetalleVenta = dt.IdDetalleVenta,
                IdVenta = dt.IdVenta,
                IdProducto = dt.IdProducto,
                Cantidad = dt.Cantidad,
                PrecioUnitario = dt.PrecioUnitario,
                CostoUnitarioHistorico = dt.CostoUnitarioHistorico,
                Subtotal = dt.Subtotal,
                MesesGarantiaHistorico = dt.MesesGarantiaHistorico,
                FechaFinGarantia = dt.FechaFinGarantia
            };
            await _repo.NuevoDetalleVentaAsync(nd);
        }

        public async Task EditarDetalleVenta(DTOSDetalleVenta dt)
        {
            var ed = new DetalleVenta
            {
                IdDetalleVenta = dt.IdDetalleVenta,
                IdVenta = dt.IdVenta,
                IdProducto = dt.IdProducto,
                Cantidad = dt.Cantidad,
                PrecioUnitario = dt.PrecioUnitario,
                CostoUnitarioHistorico = dt.CostoUnitarioHistorico,
                Subtotal = dt.Subtotal,
                MesesGarantiaHistorico = dt.MesesGarantiaHistorico,
                FechaFinGarantia = dt.FechaFinGarantia
            };
            await _repo.EditarDetalleVentaAsync(ed);
        }

        public async Task EliminarDetalleVenta(int id)
        {
            await _repo.EliminarDetalleVentaAsync(id);
        }
    }
}
