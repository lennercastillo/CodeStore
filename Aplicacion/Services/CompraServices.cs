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
    public class CompraServices
    {
        private readonly ICompra _repo;

        public CompraServices(ICompra reposi)
        {
            _repo = reposi;
        }

        public async Task<IEnumerable<DTOSCompra>> ListarCompra()
        {
            var Lista = await _repo.ListarCompraAsync();

            return Lista.Select(li => new DTOSCompra
            {
                IdCompra = li.IdCompra,
                NumeroDocumento = li.NumeroDocumento,
                IdProveedor = li.IdProveedor,
                IdUsuarioRegistro = li.IdUsuarioRegistro,
                IdUsuarioRecepcion = li.IdUsuarioRecepcion,
                FechaCompra = li.FechaCompra,
                FechaRecepcion = li.FechaRecepcion,
                TotalCompra = li.TotalCompra,
                Estado = li.Estado,
                Observaciones = li.Observaciones,
                IdUsuarioAnulacion = li.IdUsuarioAnulacion,
                FechaAnulacion = li.FechaAnulacion,
                MotivoAnulacion = li.MotivoAnulacion
            });
        }

        public async Task NuevoCompra(DTOSCompra dt)
        {
            var nc = new Compra
            {
                IdCompra = dt.IdCompra,
                NumeroDocumento = dt.NumeroDocumento,
                IdProveedor = dt.IdProveedor,
                IdUsuarioRegistro = dt.IdUsuarioRegistro,
                IdUsuarioRecepcion = dt.IdUsuarioRecepcion,
                FechaCompra = dt.FechaCompra,
                FechaRecepcion = dt.FechaRecepcion,
                TotalCompra = dt.TotalCompra,
                Estado = dt.Estado,
                Observaciones = dt.Observaciones,
                IdUsuarioAnulacion = dt.IdUsuarioAnulacion,
                FechaAnulacion = dt.FechaAnulacion,
                MotivoAnulacion = dt.MotivoAnulacion
            };
            await _repo.NuevoCompraAsync(nc);
        }

        public async Task EditarCompra(DTOSCompra dt)
        {
            var ec = new Compra
            {
                IdCompra = dt.IdCompra,
                NumeroDocumento = dt.NumeroDocumento,
                IdProveedor = dt.IdProveedor,
                IdUsuarioRegistro = dt.IdUsuarioRegistro,
                IdUsuarioRecepcion = dt.IdUsuarioRecepcion,
                FechaCompra = dt.FechaCompra,
                FechaRecepcion = dt.FechaRecepcion,
                TotalCompra = dt.TotalCompra,
                Estado = dt.Estado,
                Observaciones = dt.Observaciones,
                IdUsuarioAnulacion = dt.IdUsuarioAnulacion,
                FechaAnulacion = dt.FechaAnulacion,
                MotivoAnulacion = dt.MotivoAnulacion
            };
            await _repo.EditarCompraAsync(ec);
        }

        public async Task EliminarCompra(int id)
        {
            await _repo.EliminarCompraAsync(id);
        }
    }
}
