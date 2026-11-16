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
    public class DetalleCompraServices
    {
        private readonly IDetalleCompra _repo;

        public DetalleCompraServices(IDetalleCompra reposi)
        {
            _repo = reposi;
        }

        public async Task<IEnumerable<DTOsDetalleCompra>> ListarDetalleCompra()
        {
            var Lista = await _repo.ListarDetalleCompraAsync();

            return Lista.Select(li => new DTOsDetalleCompra
            {
                IdDetalleCompra = li.IdDetalleCompra,
                IdCompra = li.IdCompra,
                IdProducto = li.IdProducto,
                Cantidad = li.Cantidad,
                CostoUnitario = li.CostoUnitario,
                Subtotal = li.Subtotal
            });
        }

        public async Task NuevoDetalleCompra(DTOsDetalleCompra dt)
        {
            var nd = new DetalleCompra
            {
                IdDetalleCompra = dt.IdDetalleCompra,
                IdCompra = dt.IdCompra,
                IdProducto = dt.IdProducto,
                Cantidad = dt.Cantidad,
                CostoUnitario = dt.CostoUnitario,
                Subtotal = dt.Subtotal
            };
            await _repo.NuevoDetalleCompraAsync(nd);
        }

        public async Task EditarDetalleCompra(DTOsDetalleCompra dt)
        {
            var ed = new DetalleCompra
            {
                IdDetalleCompra = dt.IdDetalleCompra,
                IdCompra = dt.IdCompra,
                IdProducto = dt.IdProducto,
                Cantidad = dt.Cantidad,
                CostoUnitario = dt.CostoUnitario,
                Subtotal = dt.Subtotal
            };
            await _repo.EditarDetalleCompraAsync(ed);
        }

        public async Task EliminarDetalleCompra(int id)
        {
            await _repo.EliminarDetalleCompraAsync(id);
        }
    }
}
