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
    public class ProveedorServices
    {
        private readonly IProveedor _repo;

        public ProveedorServices(IProveedor reposi)
        {
            _repo = reposi;
        }

        public async Task<IEnumerable<DTOProveedor>> ListarProveedor()
        {
            var Lista = await _repo.ListarProveedorAsync();

            return Lista.Select(li => new DTOProveedor
            {
                IdProveedor = li.IdProveedor,
                NumeroRUC = li.NumeroRUC,
                RazonSocial = li.RazonSocial,
                NombreContacto = li.NombreContacto,
                Telefono = li.Telefono,
                Correo = li.Correo,
                Direccion = li.Direccion,
                Activo = li.Activo,
                FechaRegistro = li.FechaRegistro
            });
        }

        public async Task NuevoProveedor(DTOProveedor dt)
        {
            var np = new Proveedor
            {
                IdProveedor = dt.IdProveedor,
                NumeroRUC = dt.NumeroRUC,
                RazonSocial = dt.RazonSocial,
                NombreContacto = dt.NombreContacto,
                Telefono = dt.Telefono,
                Correo = dt.Correo,
                Direccion = dt.Direccion,
                Activo = dt.Activo,
                FechaRegistro = dt.FechaRegistro
            };
            await _repo.NuevoProveedorAsync(np);
        }

        public async Task EditarProveedor(DTOProveedor dt)
        {
            var ep = new Proveedor
            {
                IdProveedor = dt.IdProveedor,
                NumeroRUC = dt.NumeroRUC,
                RazonSocial = dt.RazonSocial,
                NombreContacto = dt.NombreContacto,
                Telefono = dt.Telefono,
                Correo = dt.Correo,
                Direccion = dt.Direccion,
                Activo = dt.Activo,
                FechaRegistro = dt.FechaRegistro
            };
            await _repo.EditarProveedorAsync(ep);
        }

        public async Task EliminarProveedor(int id)
        {
            await _repo.EliminarProveedorAsync(id);
        }

        public async Task<DTOProveedor?> ObtenerPorId(int id)
        {
            var lista = await ListarProveedor();
            return lista.FirstOrDefault(p => p.IdProveedor == id);
        }
    }
}
