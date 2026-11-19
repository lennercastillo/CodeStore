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
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioRecepcion", compra.IdUsuarioRecepcion == null ? DBNull.Value : compra.IdUsuarioRecepcion));
                cmd.Parameters.Add(new SqlParameter("@FechaRecepcion", compra.FechaRecepcion == null ? DBNull.Value : compra.FechaRecepcion));
                cmd.Parameters.Add(new SqlParameter("@Observaciones", compra.Observaciones == null ? DBNull.Value : compra.Observaciones));
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioAnulacion", compra.IdUsuarioAnulacion == null ? DBNull.Value : compra.IdUsuarioAnulacion));
                cmd.Parameters.Add(new SqlParameter("@FechaAnulacion", compra.FechaAnulacion == null ? DBNull.Value : compra.FechaAnulacion));
                cmd.Parameters.Add(new SqlParameter("@MotivoAnulacion", compra.MotivoAnulacion == null ? DBNull.Value : compra.MotivoAnulacion));

                await cmd.ExecuteNonQueryAsync();
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

                await cmd.ExecuteNonQueryAsync();
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
                cmd.Parameters.Add(new SqlParameter("@FechaCompra", compra.FechaCompra));
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioRecepcion", compra.IdUsuarioRecepcion == null ? DBNull.Value : compra.IdUsuarioRecepcion));
                cmd.Parameters.Add(new SqlParameter("@FechaRecepcion", compra.FechaRecepcion == null ? DBNull.Value : compra.FechaRecepcion));
                cmd.Parameters.Add(new SqlParameter("@TotalCompra", compra.TotalCompra));
                cmd.Parameters.Add(new SqlParameter("@Estado", compra.Estado));
                cmd.Parameters.Add(new SqlParameter("@Observaciones", compra.Observaciones == null ? DBNull.Value : compra.Observaciones));
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioAnulacion", compra.IdUsuarioAnulacion == null ? DBNull.Value : compra.IdUsuarioAnulacion));
                cmd.Parameters.Add(new SqlParameter("@FechaAnulacion", compra.FechaAnulacion == null ? DBNull.Value : compra.FechaAnulacion));
                cmd.Parameters.Add(new SqlParameter("@MotivoAnulacion", compra.MotivoAnulacion == null ? DBNull.Value : compra.MotivoAnulacion));

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Compra?> ObtenerPorIdAsync(int id)
        {
            var lista = await ListarCompraAsync();
            return lista.FirstOrDefault(c => c.IdCompra == id);
        }

        public async Task<(bool Success, string Message, int IdCompra, string NumeroDocumento)> RegistrarCompraTransaccionalAsync(
            Compra compra,
            List<DetalleCompra> detalles,
            bool recibirInmediatamente)
        {
            using var con = _cone.GetConnection();
            await con.OpenAsync();
            using var tran = con.BeginTransaction();

            try
            {
                int idCompraGenerado = 0;
                string numeroDocumentoGenerado = string.Empty;

                using (var insertCmd = new SqlCommand("InsertarCompra", con, tran))
                {
                    insertCmd.CommandType = CommandType.StoredProcedure;
                    insertCmd.Parameters.Add(new SqlParameter("@NumeroDocumento", string.IsNullOrWhiteSpace(compra.NumeroDocumento) ? DBNull.Value : compra.NumeroDocumento));
                    insertCmd.Parameters.Add(new SqlParameter("@IdProveedor", compra.IdProveedor));
                    insertCmd.Parameters.Add(new SqlParameter("@IdUsuarioRegistro", compra.IdUsuarioRegistro));
                    insertCmd.Parameters.Add(new SqlParameter("@FechaCompra", compra.FechaCompra == default ? DBNull.Value : compra.FechaCompra));
                    insertCmd.Parameters.Add(new SqlParameter("@TotalCompra", compra.TotalCompra));
                    insertCmd.Parameters.Add(new SqlParameter("@Estado", "Pendiente"));
                    insertCmd.Parameters.Add(new SqlParameter("@Observaciones", (object?)compra.Observaciones ?? DBNull.Value));

                    using var rdr = await insertCmd.ExecuteReaderAsync();
                    if (await rdr.ReadAsync())
                    {
                        idCompraGenerado = Convert.ToInt32(rdr["IdCompra"]);
                        numeroDocumentoGenerado = rdr["NumeroDocumento"].ToString()!;
                    }
                }

                foreach (var item in detalles)
                {
                    using var detCmd = new SqlCommand("InsertarDetalleCompra", con, tran);
                    detCmd.CommandType = CommandType.StoredProcedure;
                    detCmd.Parameters.Add(new SqlParameter("@IdCompra", idCompraGenerado));
                    detCmd.Parameters.Add(new SqlParameter("@IdProducto", item.IdProducto));
                    detCmd.Parameters.Add(new SqlParameter("@Cantidad", item.Cantidad));
                    detCmd.Parameters.Add(new SqlParameter("@CostoUnitario", item.CostoUnitario));
                    await detCmd.ExecuteNonQueryAsync();
                }

                if (recibirInmediatamente)
                {
                    using var recCmd = new SqlCommand("ConfirmarRecepcionCompra", con, tran);
                    recCmd.CommandType = CommandType.StoredProcedure;
                    recCmd.Parameters.Add(new SqlParameter("@IdCompra", idCompraGenerado));
                    recCmd.Parameters.Add(new SqlParameter("@IdUsuarioRecepcion", compra.IdUsuarioRegistro));
                    await recCmd.ExecuteNonQueryAsync();
                }

                await tran.CommitAsync();
                return (
                    true,
                    recibirInmediatamente ? "Compra registrada y recibida en inventario." : "Orden de compra registrada como Pendiente.",
                    idCompraGenerado,
                    numeroDocumentoGenerado);
            }
            catch (Exception ex)
            {
                try { await tran.RollbackAsync(); } catch { }
                return (false, ex.Message, 0, string.Empty);
            }
        }

        public async Task<(bool Success, string Message)> ConfirmarRecepcionAsync(int idCompra, int idUsuarioRecepcion)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("ConfirmarRecepcionCompra", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdCompra", idCompra));
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioRecepcion", idUsuarioRecepcion));

                await cmd.ExecuteNonQueryAsync();
                return (true, "Recepción confirmada e inventario actualizado correctamente.");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool Success, string Message)> AnularCompraAsync(int idCompra, int idUsuarioAnulacion, string motivo)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("AnularCompra", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdCompra", idCompra));
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioAnulacion", idUsuarioAnulacion));
                cmd.Parameters.Add(new SqlParameter("@MotivoAnulacion", motivo));

                await cmd.ExecuteNonQueryAsync();
                return (true, "Compra anulada correctamente.");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
