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
    public class CompraRepositorio : ICompra
    {
        private readonly BD _cone;

        public CompraRepositorio(BD cone)
        {
            _cone = cone;
        }

        public async Task EditarCompraAsync(Compra compra)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("ActualizarCompra", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdCompra", compra.IdCompra));
                cmd.Parameters.Add(new SqlParameter("@NumeroDocumento", compra.NumeroDocumento));
                cmd.Parameters.Add(new SqlParameter("@IdProveedor", compra.IdProveedor));
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioRegistro", compra.IdUsuarioRegistro));
                cmd.Parameters.Add(new SqlParameter("@FechaCompra", compra.FechaCompra));
                cmd.Parameters.Add(new SqlParameter("@TotalCompra", compra.TotalCompra));
                cmd.Parameters.Add(new SqlParameter("@Estado", compra.Estado));
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioRecepcion", (object?)compra.IdUsuarioRecepcion ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@FechaRecepcion", (object?)compra.FechaRecepcion ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@Observaciones", (object?)compra.Observaciones ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioAnulacion", (object?)compra.IdUsuarioAnulacion ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@FechaAnulacion", (object?)compra.FechaAnulacion ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@MotivoAnulacion", (object?)compra.MotivoAnulacion ?? DBNull.Value));

                await cmd.ExecuteReaderAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task EliminarCompraAsync(int Id)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("EliminarCompra", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdCompra", Id));

                await cmd.ExecuteReaderAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Compra>> ListarCompraAsync()
        {
            var list = new List<Compra>();
            using var con = _cone.GetConnection();
            await con.OpenAsync();

            try
            {
                using var cmd = new SqlCommand("ListarCompras", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using var dr = await cmd.ExecuteReaderAsync();

                while (await dr.ReadAsync())
                {
                    var c = new Compra
                    {
                        IdCompra = Convert.ToInt32(dr["IdCompra"]),
                        NumeroDocumento = dr["NumeroDocumento"].ToString()!,
                        IdProveedor = Convert.ToInt32(dr["IdProveedor"]),
                        IdUsuarioRegistro = Convert.ToInt32(dr["IdUsuarioRegistro"]),
                        IdUsuarioRecepcion = dr["IdUsuarioRecepcion"] == DBNull.Value ? null : Convert.ToInt32(dr["IdUsuarioRecepcion"]),
                        FechaCompra = Convert.ToDateTime(dr["FechaCompra"]),
                        FechaRecepcion = dr["FechaRecepcion"] == DBNull.Value ? null : Convert.ToDateTime(dr["FechaRecepcion"]),
                        TotalCompra = Convert.ToDecimal(dr["TotalCompra"]),
                        Estado = dr["Estado"].ToString()!,
                        Observaciones = dr["Observaciones"] == DBNull.Value ? null : dr["Observaciones"].ToString(),
                        IdUsuarioAnulacion = dr["IdUsuarioAnulacion"] == DBNull.Value ? null : Convert.ToInt32(dr["IdUsuarioAnulacion"]),
                        FechaAnulacion = dr["FechaAnulacion"] == DBNull.Value ? null : Convert.ToDateTime(dr["FechaAnulacion"]),
                        MotivoAnulacion = dr["MotivoAnulacion"] == DBNull.Value ? null : dr["MotivoAnulacion"].ToString()
                    };
                    list.Add(c);
                }
                return list;
            }
            catch
            {
                throw;
            }
        }

        public async Task NuevoCompraAsync(Compra compra)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("InsertarCompra", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@NumeroDocumento", compra.NumeroDocumento));
                cmd.Parameters.Add(new SqlParameter("@IdProveedor", compra.IdProveedor));
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioRegistro", compra.IdUsuarioRegistro));
                cmd.Parameters.Add(new SqlParameter("@FechaCompra", (object?)compra.FechaCompra == null || compra.FechaCompra == default ? DBNull.Value : compra.FechaCompra));
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioRecepcion", (object?)compra.IdUsuarioRecepcion ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@FechaRecepcion", (object?)compra.FechaRecepcion ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@TotalCompra", compra.TotalCompra));
                cmd.Parameters.Add(new SqlParameter("@Estado", compra.Estado));
                cmd.Parameters.Add(new SqlParameter("@Observaciones", (object?)compra.Observaciones ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioAnulacion", (object?)compra.IdUsuarioAnulacion ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@FechaAnulacion", (object?)compra.FechaAnulacion ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@MotivoAnulacion", (object?)compra.MotivoAnulacion ?? DBNull.Value));

                await cmd.ExecuteReaderAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
