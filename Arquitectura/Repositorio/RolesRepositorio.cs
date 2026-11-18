using Aplicacion.DTOs;
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
    public class RolesRepositorio : IRol
    {
        private readonly BD _Data;

        public RolesRepositorio(BD Data)
        {
            _Data = Data;
        }

        public async Task EditarRolAsync(Roles rol)
        {
            try
            {
                using var con = _Data.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("ActualizarRol", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdRol", rol.IdRol));
                cmd.Parameters.Add(new SqlParameter("@CodigoRol", rol.CodigoRol));
                cmd.Parameters.Add(new SqlParameter("@NombreRol", rol.NombreRol));
                cmd.Parameters.Add(new SqlParameter("@Descripcion", rol.Descripcion == null ? DBNull.Value : rol.Descripcion));
                cmd.Parameters.Add(new SqlParameter("@Activo", rol.Activo));

                await cmd.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task EliminarRolAsync(int Id)
        {
            try
            {
                using var con = _Data.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("EliminarRol", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdRol",Id));

                await cmd.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Roles>> ListarRolAsync()
        {
            var List = new List<Roles>();
            using var con = _Data.GetConnection();
            await con.OpenAsync();

            try
            {
                using var cmd = new SqlCommand("ListarRoles", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using var Dr = await cmd.ExecuteReaderAsync();

                while (await Dr.ReadAsync())
                {
                    var LR = new Roles
                    {
                        IdRol = Convert.ToInt32(Dr["IdRol"]),
                        CodigoRol = Dr["CodigoRol"].ToString()!,
                        NombreRol = Dr["NombreRol"].ToString()!,
                        Descripcion = Dr["Descripcion"].ToString()!,
                        Activo = Convert.ToBoolean(Dr["Activo"]),
                        FechaCreacion = Convert.ToDateTime(Dr["FechaCreacion"])
                    };
                    List.Add(LR);
                }
                return List;
            }
            catch
            {

                throw;
            }

        }

        public async Task NuevoRolAsync(Roles rol)
        {
            try
            {
                using var con = _Data.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("InsertarRol", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CodigoRol", rol.CodigoRol));
                cmd.Parameters.Add(new SqlParameter("@NombreRol", rol.NombreRol));
                cmd.Parameters.Add(new SqlParameter("@Descripcion", rol.Descripcion == null ? DBNull.Value : rol.Descripcion));
                cmd.Parameters.Add(new SqlParameter("@Activo", rol.Activo));

                await cmd.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
