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
    public class KardexRepositorio : IKardex
    {
        private readonly BD _cone;

        public KardexRepositorio(BD cone)
        {
            _cone = cone;
        }

        public async Task EditarKardexAsync(Kardex kardex)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("ActualizarKardex", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdMovimiento", kardex.IdMovimiento));
                cmd.Parameters.Add(new SqlParameter("@IdProducto", kardex.IdProducto));
                cmd.Parameters.Add(new SqlParameter("@FechaMovimiento", kardex.FechaMovimiento));
                cmd.Parameters.Add(new SqlParameter("@TipoMovimiento", kardex.TipoMovimiento));
                cmd.Parameters.Add(new SqlParameter("@Cantidad", kardex.Cantidad));
                cmd.Parameters.Add(new SqlParameter("@StockAnterior", kardex.StockAnterior));
                cmd.Parameters.Add(new SqlParameter("@StockNuevo", kardex.StockNuevo));
                cmd.Parameters.Add(new SqlParameter("@CostoUnitario", kardex.CostoUnitario));
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioResponsable", kardex.IdUsuarioResponsable));
                cmd.Parameters.Add(new SqlParameter("@DocumentoOrigen", (object?)kardex.DocumentoOrigen ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@IdReferenciaOrigen", (object?)kardex.IdReferenciaOrigen ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@Justificacion", (object?)kardex.Justificacion ?? DBNull.Value));

                await cmd.ExecuteReaderAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task EliminarKardexAsync(int Id)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("EliminarKardex", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdMovimiento", Id));

                await cmd.ExecuteReaderAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Kardex>> ListarKardexAsync(int? idProducto = null)
        {
            var list = new List<Kardex>();
            using var con = _cone.GetConnection();
            await con.OpenAsync();

            try
            {
                using var cmd = new SqlCommand("ListarKardex", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdMovimiento", DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@IdProducto", (object?)idProducto ?? DBNull.Value));

                using var dr = await cmd.ExecuteReaderAsync();

                while (await dr.ReadAsync())
                {
                    var k = new Kardex
                    {
                        IdMovimiento = Convert.ToInt32(dr["IdMovimiento"]),
                        IdProducto = Convert.ToInt32(dr["IdProducto"]),
                        FechaMovimiento = Convert.ToDateTime(dr["FechaMovimiento"]),
                        TipoMovimiento = dr["TipoMovimiento"].ToString()!,
                        Cantidad = Convert.ToInt32(dr["Cantidad"]),
                        StockAnterior = Convert.ToInt32(dr["StockAnterior"]),
                        StockNuevo = Convert.ToInt32(dr["StockNuevo"]),
                        CostoUnitario = Convert.ToDecimal(dr["CostoUnitario"]),
                        DocumentoOrigen = dr["DocumentoOrigen"] == DBNull.Value ? null : dr["DocumentoOrigen"].ToString(),
                        IdReferenciaOrigen = dr["IdReferenciaOrigen"] == DBNull.Value ? null : Convert.ToInt32(dr["IdReferenciaOrigen"]),
                        Justificacion = dr["Justificacion"] == DBNull.Value ? null : dr["Justificacion"].ToString(),
                        IdUsuarioResponsable = Convert.ToInt32(dr["IdUsuarioResponsable"])
                    };
                    list.Add(k);
                }
                return list;
            }
            catch
            {
                throw;
            }
        }

        public async Task NuevoKardexAsync(Kardex kardex)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("InsertarKardex", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdProducto", kardex.IdProducto));
                cmd.Parameters.Add(new SqlParameter("@TipoMovimiento", kardex.TipoMovimiento));
                cmd.Parameters.Add(new SqlParameter("@Cantidad", kardex.Cantidad));
                cmd.Parameters.Add(new SqlParameter("@StockAnterior", kardex.StockAnterior));
                cmd.Parameters.Add(new SqlParameter("@StockNuevo", kardex.StockNuevo));
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioResponsable", kardex.IdUsuarioResponsable));
                cmd.Parameters.Add(new SqlParameter("@FechaMovimiento", (object?)kardex.FechaMovimiento == null || kardex.FechaMovimiento == default ? DBNull.Value : kardex.FechaMovimiento));
                cmd.Parameters.Add(new SqlParameter("@CostoUnitario", kardex.CostoUnitario));
                cmd.Parameters.Add(new SqlParameter("@DocumentoOrigen", (object?)kardex.DocumentoOrigen ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@IdReferenciaOrigen", (object?)kardex.IdReferenciaOrigen ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@Justificacion", (object?)kardex.Justificacion ?? DBNull.Value));

                await cmd.ExecuteReaderAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
