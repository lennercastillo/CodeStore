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
    public class DetalleVentaSerieRepositorio : IDetalleVentaSerie
    {
        private readonly BD _cone;

        public DetalleVentaSerieRepositorio(BD cone)
        {
            _cone = cone;
        }

        public async Task EditarDetalleVentaSerieAsync(DetalleVentaSerie serie)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("ActualizarDetalleVentaSerie", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdSerieVendida", serie.IdSerieVendida));
                cmd.Parameters.Add(new SqlParameter("@IdDetalleVenta", serie.IdDetalleVenta));
                cmd.Parameters.Add(new SqlParameter("@NumeroSerie", serie.NumeroSerie));
                cmd.Parameters.Add(new SqlParameter("@EstadoSerie", serie.EstadoSerie));

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task EliminarDetalleVentaSerieAsync(int Id)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("EliminarDetalleVentaSerie", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdSerieVendida", Id));

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<DetalleVentaSerie>> ListarDetalleVentaSerieAsync()
        {
            var list = new List<DetalleVentaSerie>();
            using var con = _cone.GetConnection();
            await con.OpenAsync();

            try
            {
                using var cmd = new SqlCommand("ListarDetalleVentaSeries", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using var dr = await cmd.ExecuteReaderAsync();

                while (await dr.ReadAsync())
                {
                    var s = new DetalleVentaSerie
                    {
                        IdSerieVendida = Convert.ToInt32(dr["IdSerieVendida"]),
                        IdDetalleVenta = Convert.ToInt32(dr["IdDetalleVenta"]),
                        NumeroSerie = dr["NumeroSerie"].ToString()!,
                        EstadoSerie = dr["EstadoSerie"].ToString()!,
                        FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"])
                    };
                    list.Add(s);
                }
                return list;
            }
            catch
            {
                throw;
            }
        }

        public async Task NuevoDetalleVentaSerieAsync(DetalleVentaSerie serie)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("InsertarDetalleVentaSerie", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdDetalleVenta", serie.IdDetalleVenta));
                cmd.Parameters.Add(new SqlParameter("@NumeroSerie", serie.NumeroSerie));
                cmd.Parameters.Add(new SqlParameter("@EstadoSerie", serie.EstadoSerie));

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
