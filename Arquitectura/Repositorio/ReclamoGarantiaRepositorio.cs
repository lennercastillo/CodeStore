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
    public class ReclamoGarantiaRepositorio : IReclamoGarantia
    {
        private readonly BD _cone;

        public ReclamoGarantiaRepositorio(BD cone)
        {
            _cone = cone;
        }

        public async Task EditarReclamoGarantiaAsync(ReclamoGarantia reclamo)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("ActualizarReclamoGarantia", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdGarantia", reclamo.IdGarantia));
                cmd.Parameters.Add(new SqlParameter("@NumeroReclamo", reclamo.NumeroReclamo));
                cmd.Parameters.Add(new SqlParameter("@IdDetalleVenta", reclamo.IdDetalleVenta));
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioTecnico", reclamo.IdUsuarioTecnico));
                cmd.Parameters.Add(new SqlParameter("@FechaReclamo", reclamo.FechaReclamo == default ? DBNull.Value : reclamo.FechaReclamo));
                cmd.Parameters.Add(new SqlParameter("@MotivoFalla", reclamo.MotivoFalla));
                cmd.Parameters.Add(new SqlParameter("@EstadoRevision", reclamo.EstadoRevision));
                cmd.Parameters.Add(new SqlParameter("@IdSerieVendida", reclamo.IdSerieVendida == null ? DBNull.Value : reclamo.IdSerieVendida));
                cmd.Parameters.Add(new SqlParameter("@AccionTomada", reclamo.AccionTomada == null ? DBNull.Value : reclamo.AccionTomada));
                cmd.Parameters.Add(new SqlParameter("@DescripcionResolucion", reclamo.DescripcionResolucion == null ? DBNull.Value : reclamo.DescripcionResolucion));
                cmd.Parameters.Add(new SqlParameter("@FechaResolucion", reclamo.FechaResolucion == null ? DBNull.Value : reclamo.FechaResolucion));

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task EliminarReclamoGarantiaAsync(int Id)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("EliminarReclamoGarantia", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdGarantia", Id));

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<ReclamoGarantia>> ListarReclamoGarantiaAsync()
        {
            var list = new List<ReclamoGarantia>();
            using var con = _cone.GetConnection();
            await con.OpenAsync();

            try
            {
                using var cmd = new SqlCommand("ListarReclamosGarantia", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using var dr = await cmd.ExecuteReaderAsync();

                while (await dr.ReadAsync())
                {
                    var r = new ReclamoGarantia
                    {
                        IdGarantia = Convert.ToInt32(dr["IdGarantia"]),
                        NumeroReclamo = dr["NumeroReclamo"].ToString()!,
                        IdDetalleVenta = Convert.ToInt32(dr["IdDetalleVenta"]),
                        IdSerieVendida = dr["IdSerieVendida"] == DBNull.Value ? null : Convert.ToInt32(dr["IdSerieVendida"]),
                        IdUsuarioTecnico = Convert.ToInt32(dr["IdUsuarioTecnico"]),
                        FechaReclamo = Convert.ToDateTime(dr["FechaReclamo"]),
                        MotivoFalla = dr["MotivoFalla"].ToString()!,
                        EstadoRevision = dr["EstadoRevision"].ToString()!,
                        AccionTomada = dr["AccionTomada"] == DBNull.Value ? null : dr["AccionTomada"].ToString(),
                        DescripcionResolucion = dr["DescripcionResolucion"] == DBNull.Value ? null : dr["DescripcionResolucion"].ToString(),
                        FechaResolucion = dr["FechaResolucion"] == DBNull.Value ? null : Convert.ToDateTime(dr["FechaResolucion"])
                    };
                    list.Add(r);
                }
                return list;
            }
            catch
            {
                throw;
            }
        }

        public async Task NuevoReclamoGarantiaAsync(ReclamoGarantia reclamo)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("InsertarReclamoGarantia", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@NumeroReclamo", reclamo.NumeroReclamo));
                cmd.Parameters.Add(new SqlParameter("@IdDetalleVenta", reclamo.IdDetalleVenta));
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioTecnico", reclamo.IdUsuarioTecnico));
                cmd.Parameters.Add(new SqlParameter("@MotivoFalla", reclamo.MotivoFalla));
                cmd.Parameters.Add(new SqlParameter("@IdSerieVendida", reclamo.IdSerieVendida == null ? DBNull.Value : reclamo.IdSerieVendida));
                cmd.Parameters.Add(new SqlParameter("@FechaReclamo", reclamo.FechaReclamo == default ? DBNull.Value : reclamo.FechaReclamo));
                cmd.Parameters.Add(new SqlParameter("@EstadoRevision", reclamo.EstadoRevision));
                cmd.Parameters.Add(new SqlParameter("@AccionTomada", reclamo.AccionTomada == null ? DBNull.Value : reclamo.AccionTomada));
                cmd.Parameters.Add(new SqlParameter("@DescripcionResolucion", reclamo.DescripcionResolucion == null ? DBNull.Value : reclamo.DescripcionResolucion));
                cmd.Parameters.Add(new SqlParameter("@FechaResolucion", reclamo.FechaResolucion == null ? DBNull.Value : reclamo.FechaResolucion));

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
