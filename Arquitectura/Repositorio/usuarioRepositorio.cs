using Aplicacion.Interface;
using Dominio.Entidades;
using Estructura.database;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estructura.Repositorio
{
    public class usuarioRepositorio : IUsuario
    {
        private readonly BD _Data;

        public usuarioRepositorio(BD Data)
        {
            _Data = Data;
        }

        public async Task<IEnumerable<Usuario>> ListarUsuarioAsync()
        {
            var List = new List<Usuario>();
            using var con = _Data.GetConnection();
            await con.OpenAsync();

            try
            {
                using var cmd = new SqlCommand("ListarUsuarios", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using var Dr = await cmd.ExecuteReaderAsync();

                while (await Dr.ReadAsync())
                {
                    var LU = new Usuario
                    {
                        IdUsuario = Convert.ToInt32(Dr["IdUsuario"]),
                        IdRol = Convert.ToInt32(Dr["IdRol"]),
                        Nombres = Dr["Nombres"].ToString()!,
                        Apellidos = Dr["Apellidos"].ToString()!,
                        Correo = Dr["Correo"].ToString()!,
                        Telefono = Dr["Telefono"].ToString()!,
                        Activo = Convert.ToBoolean(Dr["Activo"]),
                        FechaCreacion = Convert.ToDateTime(Dr["FechaCreacion"])
                    };
                    List.Add(LU);
                }
                return List;
            }
            catch
            {
                throw;
            }
        }

        public async Task NuevoUsuarioAsync(Usuario usuario)
        {
            try
            {
                using var con = _Data.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("InsertarUsuario", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdRol", usuario.IdRol));
                cmd.Parameters.Add(new SqlParameter("@Nombres", usuario.Nombres));
                cmd.Parameters.Add(new SqlParameter("@Apellidos", usuario.Apellidos));
                cmd.Parameters.Add(new SqlParameter("@Correo", usuario.Correo));
                cmd.Parameters.Add(new SqlParameter("@PasswordHash", usuario.Contraseña == null ? DBNull.Value : usuario.Contraseña));
                cmd.Parameters.Add(new SqlParameter("@Telefono", usuario.Telefono == null ? DBNull.Value : usuario.Telefono));
                cmd.Parameters.Add(new SqlParameter("@Activo", usuario.Activo));

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task EditarUsuarioAsync(Usuario usuario)
        {
            try
            {
                using var con = _Data.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("ActualizarUsuario", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdUsuario", usuario.IdUsuario));
                cmd.Parameters.Add(new SqlParameter("@IdRol", usuario.IdRol));
                cmd.Parameters.Add(new SqlParameter("@Nombres", usuario.Nombres));
                cmd.Parameters.Add(new SqlParameter("@Apellidos", usuario.Apellidos));
                cmd.Parameters.Add(new SqlParameter("@Correo", usuario.Correo));
                cmd.Parameters.Add(new SqlParameter("@Activo", usuario.Activo));
                cmd.Parameters.Add(new SqlParameter("@PasswordHash", usuario.Contraseña == null ? DBNull.Value : usuario.Contraseña));
                cmd.Parameters.Add(new SqlParameter("@Telefono", usuario.Telefono == null ? DBNull.Value : usuario.Telefono));

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task EliminarUsuarioAsync(int Id)
        {
            try
            {
                using var con = _Data.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("EliminarUsuario", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdUsuario", Id));

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Usuario?> ValidarCredencialesAsync(string correo, string passwordHash)
        {
            using var con = _Data.GetConnection();
            await con.OpenAsync();
            using var cmd = new SqlCommand(@"
                SELECT IdUsuario, IdRol, Nombres, Apellidos, Correo, Telefono, Activo, FechaCreacion
                FROM Usuarios
                WHERE Correo = @Correo AND PasswordHash = @PasswordHash AND Activo = 1", con);
            cmd.Parameters.Add(new SqlParameter("@Correo", correo));
            cmd.Parameters.Add(new SqlParameter("@PasswordHash", passwordHash));

            using var dr = await cmd.ExecuteReaderAsync();
            if (await dr.ReadAsync())
            {
                return new Usuario
                {
                    IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                    IdRol = Convert.ToInt32(dr["IdRol"]),
                    Nombres = dr["Nombres"].ToString()!,
                    Apellidos = dr["Apellidos"].ToString()!,
                    Correo = dr["Correo"].ToString()!,
                    Telefono = dr["Telefono"] == DBNull.Value ? null : dr["Telefono"].ToString(),
                    Activo = Convert.ToBoolean(dr["Activo"]),
                    FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"])
                };
            }
            return null;
        }

        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            var lista = await ListarUsuarioAsync();
            return lista.FirstOrDefault(u => u.IdUsuario == id);
        }
    }
}
