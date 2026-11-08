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
    public class VentaServices
    {
        private readonly IVenta _repo;

        public VentaServices(IVenta reposi)
        {
            _repo = reposi;
        }

        public async Task<IEnumerable<DTOSVenta>> ListarVenta()
        {
            var Lista = await _repo.ListarVentaAsync();

            return Lista.Select(li => new DTOSVenta
            {
                IdVenta = li.IdVenta,
                NumeroFactura = li.NumeroFactura,
                IdCliente = li.IdCliente,
                IdUsuarioVendedor = li.IdUsuarioVendedor,
                FechaVenta = li.FechaVenta,
                TipoVenta = li.TipoVenta,
                MetodoPago = li.MetodoPago,
                PorcentajeImpuesto = li.PorcentajeImpuesto,
                Subtotal = li.Subtotal,
                Impuesto = li.Impuesto,
                TotalVenta = li.TotalVenta,
                MontoRecibido = li.MontoRecibido,
                Vuelto = li.Vuelto,
                Estado = li.Estado,
                IdUsuarioAutorizaAnulacion = li.IdUsuarioAutorizaAnulacion,
                FechaAnulacion = li.FechaAnulacion,
                MotivoAnulacion = li.MotivoAnulacion
            });
        }

        public async Task NuevoVenta(DTOSVenta dt)
        {
            var nv = new Venta
            {
                IdVenta = dt.IdVenta,
                NumeroFactura = dt.NumeroFactura,
                IdCliente = dt.IdCliente,
                IdUsuarioVendedor = dt.IdUsuarioVendedor,
                FechaVenta = dt.FechaVenta,
                TipoVenta = dt.TipoVenta,
                MetodoPago = dt.MetodoPago,
                PorcentajeImpuesto = dt.PorcentajeImpuesto,
                Subtotal = dt.Subtotal,
                Impuesto = dt.Impuesto,
                TotalVenta = dt.TotalVenta,
                MontoRecibido = dt.MontoRecibido,
                Vuelto = dt.Vuelto,
                Estado = dt.Estado,
                IdUsuarioAutorizaAnulacion = dt.IdUsuarioAutorizaAnulacion,
                FechaAnulacion = dt.FechaAnulacion,
                MotivoAnulacion = dt.MotivoAnulacion
            };
            await _repo.NuevoVentaAsync(nv);
        }

        public async Task EditarVenta(DTOSVenta dt)
        {
            var ev = new Venta
            {
                IdVenta = dt.IdVenta,
                NumeroFactura = dt.NumeroFactura,
                IdCliente = dt.IdCliente,
                IdUsuarioVendedor = dt.IdUsuarioVendedor,
                FechaVenta = dt.FechaVenta,
                TipoVenta = dt.TipoVenta,
                MetodoPago = dt.MetodoPago,
                PorcentajeImpuesto = dt.PorcentajeImpuesto,
                Subtotal = dt.Subtotal,
                Impuesto = dt.Impuesto,
                TotalVenta = dt.TotalVenta,
                MontoRecibido = dt.MontoRecibido,
                Vuelto = dt.Vuelto,
                Estado = dt.Estado,
                IdUsuarioAutorizaAnulacion = dt.IdUsuarioAutorizaAnulacion,
                FechaAnulacion = dt.FechaAnulacion,
                MotivoAnulacion = dt.MotivoAnulacion
            };
            await _repo.EditarVentaAsync(ev);
        }

        public async Task EliminarVenta(int id)
        {
            await _repo.EliminarVentaAsync(id);
        }
    }
}
