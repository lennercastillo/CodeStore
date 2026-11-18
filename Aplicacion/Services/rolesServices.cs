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
    public class rolesServices
    {
        private readonly IRol _repo;

        public rolesServices(IRol reposi)
        {
            _repo = reposi;
        }

        public async Task<IEnumerable<RolDT>> ListaRoles()
        {
            var Lista = await _repo.ListarRolAsync();

            return Lista.Select(li => new RolDT

            {
                IdRol = li.IdRol,
                CodigoRol = li.CodigoRol,
                NombreRol = li.NombreRol,
                Descripcion = li.Descripcion,
                Activo = li.Activo,
                FechaCreacion = li.FechaCreacion
            });
        }

        public async Task NuevoRol(RolDT dt)
        {

            var nr = new Roles
            {
                IdRol = dt.IdRol,
                CodigoRol = dt.CodigoRol,
                NombreRol = dt.NombreRol,
                Descripcion = dt.Descripcion,
                Activo = dt.Activo,
                FechaCreacion = dt.FechaCreacion
            };
            await _repo.NuevoRolAsync(nr);
        }


        public async Task EditarRol(RolDT dt)
        {

            var nr = new Roles
            {
                IdRol = dt.IdRol,
                CodigoRol = dt.CodigoRol,
                NombreRol = dt.NombreRol,
                Descripcion = dt.Descripcion,
                Activo = dt.Activo,
                FechaCreacion = dt.FechaCreacion
            };
            await _repo.EditarRolAsync(nr);
        }


        public async Task EliminarRoles(int id)
        {
            await _repo.EliminarRolAsync(id);
        }

        public async Task<RolDT?> ObtenerPorId(int id)
        {
            var lista = await ListaRoles();
            return lista.FirstOrDefault(r => r.IdRol == id);
        }
    }
}
