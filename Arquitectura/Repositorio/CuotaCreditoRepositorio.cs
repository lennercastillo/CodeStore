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
    public class CuotaCreditoRepositorio : ICuotaCredito
    {
        private readonly BD _cone;

        public CuotaCreditoRepositorio(BD cone)
        {
            _cone = cone;
        }

        public async Task EditarCuotaCreditoAsync(CuotaCredito cuota)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("ActualizarCuotaCredito", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdCuota", cuota.IdCuota));
                cmd.Parameters.Add(new SqlParameter("@IdCredito", cuota.IdCredito));
                cmd.Parameters.Add(new SqlParameter("@NumeroCuota", cuota.NumeroCuota));
                cmd.Parameters.Add(new SqlParameter("@FechaVencimiento", cuota.FechaVencimiento));
                cmd.Parameters.Add(new SqlParameter("@MontoCuota", cuota.MontoCuota));
                cmd.Parameters.Add(new SqlParameter("@RecargoMora", cuota.RecargoMora));
                cmd.Parameters.Add(new SqlParameter("@MoraAbonada", cuota.MoraAbonada));
                cmd.Parameters.Add(new SqlParameter("@CapitalInteresAbonado", cuota.CapitalInteresAbonado));
                cmd.Parameters.Add(new SqlParameter("@Estado", cuota.Estado));

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task EliminarCuotaCreditoAsync(int Id)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("EliminarCuotaCredito", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdCuota", Id));

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<CuotaCredito>> ListarCuotaCreditoAsync()
        {
            var list = new List<CuotaCredito>();
            using var con = _cone.GetConnection();
            await con.OpenAsync();

            try
            {
                using var cmd = new SqlCommand("ListarCuotasCredito", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using var dr = await cmd.ExecuteReaderAsync();

                while (await dr.ReadAsync())
                {
                    var c = new CuotaCredito
                    {
                        IdCuota = Convert.ToInt32(dr["IdCuota"]),
                        IdCredito = Convert.ToInt32(dr["IdCredito"]),
                        NumeroCuota = Convert.ToInt32(dr["NumeroCuota"]),
                        FechaVencimiento = Convert.ToDateTime(dr["FechaVencimiento"]),
                        MontoCuota = Convert.ToDecimal(dr["MontoCuota"]),
                        RecargoMora = Convert.ToDecimal(dr["RecargoMora"]),
                        MoraAbonada = Convert.ToDecimal(dr["MoraAbonada"]),
                        CapitalInteresAbonado = Convert.ToDecimal(dr["CapitalInteresAbonado"]),
                        MontoAbonado = Convert.ToDecimal(dr["MontoAbonado"]),
                        SaldoCuota = Convert.ToDecimal(dr["SaldoCuota"]),
                        Estado = dr["Estado"].ToString()!
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

        public async Task NuevoCuotaCreditoAsync(CuotaCredito cuota)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("InsertarCuotaCredito", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdCredito", cuota.IdCredito));
                cmd.Parameters.Add(new SqlParameter("@NumeroCuota", cuota.NumeroCuota));
                cmd.Parameters.Add(new SqlParameter("@FechaVencimiento", cuota.FechaVencimiento));
                cmd.Parameters.Add(new SqlParameter("@MontoCuota", cuota.MontoCuota));
                cmd.Parameters.Add(new SqlParameter("@RecargoMora", cuota.RecargoMora));
                cmd.Parameters.Add(new SqlParameter("@MoraAbonada", cuota.MoraAbonada));
                cmd.Parameters.Add(new SqlParameter("@CapitalInteresAbonado", cuota.CapitalInteresAbonado));
                cmd.Parameters.Add(new SqlParameter("@Estado", cuota.Estado));

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<(bool Success, string Message)> AplicarMoraAsync(int idCuota, decimal recargoMora)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("AplicarMoraCuota", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdCuota", idCuota));
                cmd.Parameters.Add(new SqlParameter("@RecargoMora", recargoMora));

                await cmd.ExecuteNonQueryAsync();
                return (true, $"Recargo por mora actualizado a $ {recargoMora:N2}.");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
