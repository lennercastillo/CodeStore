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
    public class CreditoRepositorio : ICredito
    {
        private readonly BD _cone;

        public CreditoRepositorio(BD cone)
        {
            _cone = cone;
        }

        public async Task EditarCreditoAsync(Credito credito)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("ActualizarCredito", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdCredito", credito.IdCredito));
                cmd.Parameters.Add(new SqlParameter("@IdVenta", credito.IdVenta));
                cmd.Parameters.Add(new SqlParameter("@MontoVentaTotal", credito.MontoVentaTotal));
                cmd.Parameters.Add(new SqlParameter("@PrimaInicial", credito.PrimaInicial));
                cmd.Parameters.Add(new SqlParameter("@MontoFinanciado", credito.MontoFinanciado));
                cmd.Parameters.Add(new SqlParameter("@FrecuenciaPago", credito.FrecuenciaPago));
                cmd.Parameters.Add(new SqlParameter("@CantidadCuotas", credito.CantidadCuotas));
                cmd.Parameters.Add(new SqlParameter("@PorcentajeInteres", credito.PorcentajeInteres));
                cmd.Parameters.Add(new SqlParameter("@MontoInteresesPactados", credito.MontoInteresesPactados));
                cmd.Parameters.Add(new SqlParameter("@MontoTotalCredito", credito.MontoTotalCredito));
                cmd.Parameters.Add(new SqlParameter("@SaldoPendiente", credito.SaldoPendiente));
                cmd.Parameters.Add(new SqlParameter("@Estado", credito.Estado));

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task EliminarCreditoAsync(int Id)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("EliminarCredito", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdCredito", Id));

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<Credito>> ListarCreditoAsync()
        {
            var list = new List<Credito>();
            using var con = _cone.GetConnection();
            await con.OpenAsync();

            try
            {
                using var cmd = new SqlCommand("ListarCreditos", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using var dr = await cmd.ExecuteReaderAsync();

                while (await dr.ReadAsync())
                {
                    var c = new Credito
                    {
                        IdCredito = Convert.ToInt32(dr["IdCredito"]),
                        IdVenta = Convert.ToInt32(dr["IdVenta"]),
                        MontoVentaTotal = Convert.ToDecimal(dr["MontoVentaTotal"]),
                        PrimaInicial = Convert.ToDecimal(dr["PrimaInicial"]),
                        MontoFinanciado = Convert.ToDecimal(dr["MontoFinanciado"]),
                        FrecuenciaPago = dr["FrecuenciaPago"].ToString()!,
                        CantidadCuotas = Convert.ToInt32(dr["CantidadCuotas"]),
                        PorcentajeInteres = Convert.ToDecimal(dr["PorcentajeInteres"]),
                        MontoInteresesPactados = Convert.ToDecimal(dr["MontoInteresesPactados"]),
                        MontoTotalCredito = Convert.ToDecimal(dr["MontoTotalCredito"]),
                        SaldoPendiente = Convert.ToDecimal(dr["SaldoPendiente"]),
                        Estado = dr["Estado"].ToString()!,
                        FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"])
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

        public async Task NuevoCreditoAsync(Credito credito)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("InsertarCredito", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdVenta", credito.IdVenta));
                cmd.Parameters.Add(new SqlParameter("@MontoVentaTotal", credito.MontoVentaTotal));
                cmd.Parameters.Add(new SqlParameter("@MontoFinanciado", credito.MontoFinanciado));
                cmd.Parameters.Add(new SqlParameter("@FrecuenciaPago", credito.FrecuenciaPago));
                cmd.Parameters.Add(new SqlParameter("@CantidadCuotas", credito.CantidadCuotas));
                cmd.Parameters.Add(new SqlParameter("@MontoTotalCredito", credito.MontoTotalCredito));
                cmd.Parameters.Add(new SqlParameter("@SaldoPendiente", credito.SaldoPendiente));
                cmd.Parameters.Add(new SqlParameter("@PrimaInicial", credito.PrimaInicial));
                cmd.Parameters.Add(new SqlParameter("@PorcentajeInteres", credito.PorcentajeInteres));
                cmd.Parameters.Add(new SqlParameter("@MontoInteresesPactados", credito.MontoInteresesPactados));
                cmd.Parameters.Add(new SqlParameter("@Estado", credito.Estado));

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
