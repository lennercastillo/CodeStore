using Aplicacion.DTOs;
using Aplicacion.Interface;
using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Aplicacion.Services
{
    public class UsuarioServices
    {

        private readonly IUsuario _repo;

        public UsuarioServices(IUsuario reposi)
        {
            _repo = reposi;
        }

        public async Task<IEnumerable<DTOSUsuario>> ListaRoles()
        {
            var Lista = await _repo.ListarUsuarioAsync();

            return Lista.Select(li => new DTOSUsuario

            {
                IdUsuario = li.IdUsuario,
                IdRol = li.IdRol,
                Nombres = li.Nombres,
                Apellidos = li.Apellidos,
                Correo = li.Correo,
                Telefono = li.Telefono,
                Activo = li.Activo,
                FechaCreacion = li.FechaCreacion
            });
        }

        public async Task NuevoUsuario(DTOSUsuario dt)
        {
            if (string.IsNullOrWhiteSpace(dt.Contraseña))
                throw new ArgumentException("La contraseña es obligatoria al crear un usuario.");

            var nu = new Usuario
            {
                IdUsuario = dt.IdUsuario,
                IdRol = dt.IdRol,
                Nombres = dt.Nombres,
                Apellidos = dt.Apellidos,
                Correo = dt.Correo,
                Contraseña = string.IsNullOrWhiteSpace(dt.Contraseña) ? null : GenerarHash(dt.Contraseña),
                Telefono = dt.Telefono,
                Activo = dt.Activo,
                FechaCreacion = dt.FechaCreacion
            };
            await _repo.NuevoUsuarioAsync(nu);
        }


        public async Task EditarUsuario(DTOSUsuario dt)
        {

            var nr = new Usuario
            {
                IdUsuario = dt.IdUsuario,
                IdRol = dt.IdRol,
                Nombres = dt.Nombres,
                Apellidos = dt.Apellidos,
                Correo = dt.Correo,
                Contraseña = string.IsNullOrWhiteSpace(dt.Contraseña) ? null : GenerarHash(dt.Contraseña),
                Telefono = dt.Telefono,
                Activo = dt.Activo,
                FechaCreacion = dt.FechaCreacion
            };
            await _repo.EditarUsuarioAsync(nr);
        }


        public async Task Eliminarusuario(int id)
        {
            await _repo.EliminarUsuarioAsync(id);
        }

        public async Task<DTOSUsuario?> ObtenerPorId(int id)
        {
            var u = await _repo.ObtenerPorIdAsync(id);
            if (u == null) return null;

            return new DTOSUsuario
            {
                IdUsuario = u.IdUsuario,
                IdRol = u.IdRol,
                Nombres = u.Nombres,
                Apellidos = u.Apellidos,
                Correo = u.Correo,
                Telefono = u.Telefono,
                Activo = u.Activo,
                FechaCreacion = u.FechaCreacion
            };
        }

        public async Task<DTOSUsuario?> ValidarLogin(string correo, string password)
        {
            if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(password))
                return null;

            var hash = GenerarHash(password);
            var u = await _repo.ValidarCredencialesAsync(correo.Trim(), hash);
            if (u == null) return null;

            return new DTOSUsuario
            {
                IdUsuario = u.IdUsuario,
                IdRol = u.IdRol,
                Nombres = u.Nombres,
                Apellidos = u.Apellidos,
                Correo = u.Correo,
                Telefono = u.Telefono,
                Activo = u.Activo,
                FechaCreacion = u.FechaCreacion
            };
        }

        private static string GenerarHash(string password)
        {
            return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(password)));
        }

    }
}
