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
    public class ProveedorRepositorio : IProveedor
    {
        private readonly BD _Cone;

        public ProveedorRepositorio(BD cone)
        {
            _Cone = cone;
        }

        public async Task EditarProveedorAsync(Proveedor proveedor)
        {
            try
            {
                using var con = _Cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("ActualizarProveedor", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdProveedor", proveedor.IdProveedor));
                cmd.Parameters.Add(new SqlParameter("@NumeroRUC", proveedor.NumeroRUC));
                cmd.Parameters.Add(new SqlParameter("@RazonSocial", proveedor.RazonSocial));
                cmd.Parameters.Add(new SqlParameter("@NombreContacto", proveedor.NombreContacto == null ? DBNull.Value : proveedor.NombreContacto));
                cmd.Parameters.Add(new SqlParameter("@Telefono", proveedor.Telefono == null ? DBNull.Value : proveedor.Telefono));
                cmd.Parameters.Add(new SqlParameter("@Correo", proveedor.Correo == null ? DBNull.Value : proveedor.Correo));
                cmd.Parameters.Add(new SqlParameter("@Direccion", proveedor.Direccion == null ? DBNull.Value : proveedor.Direccion));
                cmd.Parameters.Add(new SqlParameter("@Activo", proveedor.Activo));

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task EliminarProveedorAsync(int Id)
        {
            try
            {
                using var con = _Cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("EliminarProveedor", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdProveedor", Id));

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Proveedor>> ListarProveedorAsync()
        {
            var List = new List<Proveedor>();
            using var con = _Cone.GetConnection();
            await con.OpenAsync();

            try
            {
                using var cmd = new SqlCommand("ListarProveedores", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using var Dr = await cmd.ExecuteReaderAsync();

                while (await Dr.ReadAsync())
                {
                    var LP = new Proveedor
                    {
                        IdProveedor = Convert.ToInt32(Dr["IdProveedor"]),
                        NumeroRUC = Dr["NumeroRUC"].ToString()!,
                        RazonSocial = Dr["RazonSocial"].ToString()!,
                        NombreContacto = Dr["NombreContacto"] == DBNull.Value ? null : Dr["NombreContacto"].ToString(),
                        Telefono = Dr["Telefono"] == DBNull.Value ? null : Dr["Telefono"].ToString(),
                        Correo = Dr["Correo"] == DBNull.Value ? null : Dr["Correo"].ToString(),
                        Direccion = Dr["Direccion"] == DBNull.Value ? null : Dr["Direccion"].ToString(),
                        Activo = Convert.ToBoolean(Dr["Activo"]),
                        FechaRegistro = Convert.ToDateTime(Dr["FechaRegistro"])
                    };
                    List.Add(LP);
                }
                return List;
            }
            catch
            {
                throw;
            }
        }

        public async Task NuevoProveedorAsync(Proveedor proveedor)
        {
            try
            {
                using var con = _Cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("InsertarProveedor", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@NumeroRUC", proveedor.NumeroRUC));
                cmd.Parameters.Add(new SqlParameter("@RazonSocial", proveedor.RazonSocial));
                cmd.Parameters.Add(new SqlParameter("@NombreContacto", proveedor.NombreContacto == null ? DBNull.Value : proveedor.NombreContacto));
                cmd.Parameters.Add(new SqlParameter("@Telefono", proveedor.Telefono == null ? DBNull.Value : proveedor.Telefono));
                cmd.Parameters.Add(new SqlParameter("@Correo", proveedor.Correo == null ? DBNull.Value : proveedor.Correo));
                cmd.Parameters.Add(new SqlParameter("@Direccion", proveedor.Direccion == null ? DBNull.Value : proveedor.Direccion));
                cmd.Parameters.Add(new SqlParameter("@Activo", proveedor.Activo));

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
