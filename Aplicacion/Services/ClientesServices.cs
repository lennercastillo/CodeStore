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
    public class Clientes_Services
    {
        private readonly IClientes _repo;

        public Clientes_Services(IClientes reposi)
        {
            _repo = reposi;
        }

        public async Task<IEnumerable<DTOsClientes>> ListaClientes ()
        {
            var Lista = await _repo.ListarClienteAsync();

            return Lista.Select(li => new DTOsClientes

            {
                IdCliente= li.IdCliente,
                NumeroIdentificacion= li.NumeroIdentificacion,
                Nombres= li.Nombres,
                Apellidos= li.Apellidos,
                Telefono= li.Telefono,
                Correo= li.Correo,
                Direccion= li.Direccion,
                Activo= li.Activo,
                FechaRegistro= li.FechaRegistro
            });
        }

        public async Task NuevoClientes(DTOsClientes dt)
        {

            var nr = new Clientes
            {
                IdCliente = dt.IdCliente,
                NumeroIdentificacion = dt.NumeroIdentificacion,
                Nombres = dt.Nombres,
                Apellidos = dt.Apellidos,
                Telefono = dt.Telefono,
                Correo = dt.Correo,
                Direccion = dt.Direccion,
                Activo = dt.Activo,
                FechaRegistro = dt.FechaRegistro
            };
            await _repo.NuevoClienteAsync(nr);
        }


        public async Task EditarClientes(DTOsClientes dt)
        {

            var nr = new Clientes
            {
                IdCliente = dt.IdCliente,
                NumeroIdentificacion = dt.NumeroIdentificacion,
                Nombres = dt.Nombres,
                Apellidos = dt.Apellidos,
                Telefono = dt.Telefono,
                Correo = dt.Correo,
                Direccion = dt.Direccion,
                Activo = dt.Activo,
                FechaRegistro = dt.FechaRegistro
            };
            await _repo.EditarClienteAsync(nr);
        }


        public async Task Eliminarclientes(int id)
        {
            await _repo.EliminarClienteAsync(id);
        }

    }
}
