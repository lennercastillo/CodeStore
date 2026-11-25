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
    public class AbonoRepositorio : IAbono
    {
        private readonly BD _cone;

        public AbonoRepositorio(BD cone)
        {
            _cone = cone;
        }

        public async Task EditarAbonoAsync(Abono abono)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("ActualizarAbono", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdAbono", abono.IdAbono));
                cmd.Parameters.Add(new SqlParameter("@NumeroRecibo", abono.NumeroRecibo));
                cmd.Parameters.Add(new SqlParameter("@IdCuota", abono.IdCuota));
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioRegistro", abono.IdUsuarioRegistro));
                cmd.Parameters.Add(new SqlParameter("@FechaAbono", abono.FechaAbono == default ? DateTime.Now : abono.FechaAbono));
                cmd.Parameters.Add(new SqlParameter("@MontoAbono", abono.MontoAbono));
                cmd.Parameters.Add(new SqlParameter("@MontoAplicadoMora", abono.MontoAplicadoMora));
                cmd.Parameters.Add(new SqlParameter("@MontoAplicadoCapital", abono.MontoAplicadoCapital));
                cmd.Parameters.Add(new SqlParameter("@MetodoPago", abono.MetodoPago));
                cmd.Parameters.Add(new SqlParameter("@Estado", abono.Estado));
                cmd.Parameters.Add(new SqlParameter("@Observaciones", abono.Observaciones == null ? DBNull.Value : abono.Observaciones));
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioAnulacion", abono.IdUsuarioAnulacion == null ? DBNull.Value : abono.IdUsuarioAnulacion));
                cmd.Parameters.Add(new SqlParameter("@FechaAnulacion", abono.FechaAnulacion == null ? DBNull.Value : abono.FechaAnulacion));
                cmd.Parameters.Add(new SqlParameter("@MotivoAnulacion", abono.MotivoAnulacion == null ? DBNull.Value : abono.MotivoAnulacion));

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task EliminarAbonoAsync(int Id)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("EliminarAbono", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdAbono", Id));

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<Abono>> ListarAbonoAsync()
        {
            var list = new List<Abono>();
            using var con = _cone.GetConnection();
            await con.OpenAsync();

            try
            {
                using var cmd = new SqlCommand("ListarAbonos", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using var dr = await cmd.ExecuteReaderAsync();

                while (await dr.ReadAsync())
                {
                    var a = new Abono
                    {
                        IdAbono = Convert.ToInt32(dr["IdAbono"]),
                        NumeroRecibo = dr["NumeroRecibo"].ToString()!,
                        IdCuota = Convert.ToInt32(dr["IdCuota"]),
                        IdUsuarioRegistro = Convert.ToInt32(dr["IdUsuarioRegistro"]),
                        FechaAbono = Convert.ToDateTime(dr["FechaAbono"]),
                        MontoAbono = Convert.ToDecimal(dr["MontoAbono"]),
                        MontoAplicadoMora = Convert.ToDecimal(dr["MontoAplicadoMora"]),
                        MontoAplicadoCapital = Convert.ToDecimal(dr["MontoAplicadoCapital"]),
                        MetodoPago = dr["MetodoPago"].ToString()!,
                        Observaciones = dr["Observaciones"] == DBNull.Value ? null : dr["Observaciones"].ToString(),
                        Estado = dr["Estado"].ToString()!,
                        IdUsuarioAnulacion = dr["IdUsuarioAnulacion"] == DBNull.Value ? null : Convert.ToInt32(dr["IdUsuarioAnulacion"]),
                        FechaAnulacion = dr["FechaAnulacion"] == DBNull.Value ? null : Convert.ToDateTime(dr["FechaAnulacion"]),
                        MotivoAnulacion = dr["MotivoAnulacion"] == DBNull.Value ? null : dr["MotivoAnulacion"].ToString()
                    };
                    list.Add(a);
                }
                return list;
            }
            catch
            {
                throw;
            }
        }

        public async Task NuevoAbonoAsync(Abono abono)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("InsertarAbono", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@NumeroRecibo", abono.NumeroRecibo));
                cmd.Parameters.Add(new SqlParameter("@IdCuota", abono.IdCuota));
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioRegistro", abono.IdUsuarioRegistro));
                cmd.Parameters.Add(new SqlParameter("@MontoAbono", abono.MontoAbono));
                cmd.Parameters.Add(new SqlParameter("@MetodoPago", abono.MetodoPago));
                cmd.Parameters.Add(new SqlParameter("@FechaAbono", abono.FechaAbono == default ? DateTime.Now : abono.FechaAbono));
                cmd.Parameters.Add(new SqlParameter("@MontoAplicadoMora", abono.MontoAplicadoMora));
                cmd.Parameters.Add(new SqlParameter("@MontoAplicadoCapital", abono.MontoAplicadoCapital));
                cmd.Parameters.Add(new SqlParameter("@Observaciones", abono.Observaciones == null ? DBNull.Value : abono.Observaciones));
                cmd.Parameters.Add(new SqlParameter("@Estado", abono.Estado));
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioAnulacion", abono.IdUsuarioAnulacion == null ? DBNull.Value : abono.IdUsuarioAnulacion));
                cmd.Parameters.Add(new SqlParameter("@FechaAnulacion", abono.FechaAnulacion == null ? DBNull.Value : abono.FechaAnulacion));
                cmd.Parameters.Add(new SqlParameter("@MotivoAnulacion", abono.MotivoAnulacion == null ? DBNull.Value : abono.MotivoAnulacion));

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<(bool Success, string Message, int IdAbono, string NumeroRecibo)> RegistrarAbonoTransaccionalAsync(
            int idCuota,
            decimal montoAbono,
            string metodoPago,
            string? observaciones,
            int idUsuarioRegistro)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("RegistrarAbono", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdCuota", idCuota));
                cmd.Parameters.Add(new SqlParameter("@MontoAbono", montoAbono));
                cmd.Parameters.Add(new SqlParameter("@MetodoPago", metodoPago));
                cmd.Parameters.Add(new SqlParameter("@Observaciones", (object?)observaciones ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioRegistro", idUsuarioRegistro));

                int idAbono = 0;
                string numeroRecibo = string.Empty;

                using var rdr = await cmd.ExecuteReaderAsync();
                if (await rdr.ReadAsync())
                {
                    idAbono = Convert.ToInt32(rdr["IdAbono"]);
                    numeroRecibo = rdr["NumeroRecibo"].ToString()!;
                }

                return (true, $"Abono registrado con Ã©xito. Recibo {numeroRecibo}.", idAbono, numeroRecibo);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, 0, string.Empty);
            }
        }

        public async Task<(bool Success, string Message)> AnularAbonoTransaccionalAsync(int idAbono, int idUsuarioAnulacion, string motivo)
        {
            if (string.IsNullOrWhiteSpace(motivo))
                return (false, "Debe especificar el motivo de anulaciÃ³n.");

            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("AnularAbono", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdAbono", idAbono));
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioAnulacion", idUsuarioAnulacion));
                cmd.Parameters.Add(new SqlParameter("@MotivoAnulacion", motivo.Trim()));

                string numeroRecibo = string.Empty;
                using var rdr = await cmd.ExecuteReaderAsync();
                if (await rdr.ReadAsync())
                {
                    numeroRecibo = rdr["NumeroRecibo"].ToString()!;
                }

                return (true, $"Recibo {numeroRecibo} anulado y saldo revertido correctamente.");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
