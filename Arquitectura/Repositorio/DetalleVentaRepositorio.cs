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
    public class DetalleVentaRepositorio : IDetalleVenta
    {
        private readonly BD _cone;

        public DetalleVentaRepositorio(BD cone)
        {
            _cone = cone;
        }

        public async Task EditarDetalleVentaAsync(DetalleVenta detalle)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("ActualizarDetalleVenta", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdDetalleVenta", detalle.IdDetalleVenta));
                cmd.Parameters.Add(new SqlParameter("@IdVenta", detalle.IdVenta));
                cmd.Parameters.Add(new SqlParameter("@IdProducto", detalle.IdProducto));
                cmd.Parameters.Add(new SqlParameter("@Cantidad", detalle.Cantidad));
                cmd.Parameters.Add(new SqlParameter("@PrecioUnitario", detalle.PrecioUnitario));
                cmd.Parameters.Add(new SqlParameter("@CostoUnitarioHistorico", detalle.CostoUnitarioHistorico));
                cmd.Parameters.Add(new SqlParameter("@MesesGarantiaHistorico", detalle.MesesGarantiaHistorico));
                cmd.Parameters.Add(new SqlParameter("@FechaFinGarantia", detalle.FechaFinGarantia == null ? DBNull.Value : detalle.FechaFinGarantia));

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task EliminarDetalleVentaAsync(int Id)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("EliminarDetalleVenta", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdDetalleVenta", Id));

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<DetalleVenta>> ListarDetalleVentaAsync()
        {
            var list = new List<DetalleVenta>();
            using var con = _cone.GetConnection();
            await con.OpenAsync();

            try
            {
                using var cmd = new SqlCommand("ListarDetalleVentas", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using var dr = await cmd.ExecuteReaderAsync();

                while (await dr.ReadAsync())
                {
                    var dv = new DetalleVenta
                    {
                        IdDetalleVenta = Convert.ToInt32(dr["IdDetalleVenta"]),
                        IdVenta = Convert.ToInt32(dr["IdVenta"]),
                        IdProducto = Convert.ToInt32(dr["IdProducto"]),
                        Cantidad = Convert.ToInt32(dr["Cantidad"]),
                        PrecioUnitario = Convert.ToDecimal(dr["PrecioUnitario"]),
                        CostoUnitarioHistorico = Convert.ToDecimal(dr["CostoUnitarioHistorico"]),
                        Subtotal = Convert.ToDecimal(dr["Subtotal"]),
                        MesesGarantiaHistorico = Convert.ToInt32(dr["MesesGarantiaHistorico"]),
                        FechaFinGarantia = dr["FechaFinGarantia"] == DBNull.Value ? null : Convert.ToDateTime(dr["FechaFinGarantia"])
                    };
                    list.Add(dv);
                }
                return list;
            }
            catch
            {
                throw;
            }
        }

        public async Task NuevoDetalleVentaAsync(DetalleVenta detalle)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("InsertarDetalleVenta", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdVenta", detalle.IdVenta));
                cmd.Parameters.Add(new SqlParameter("@IdProducto", detalle.IdProducto));
                cmd.Parameters.Add(new SqlParameter("@Cantidad", detalle.Cantidad));
                cmd.Parameters.Add(new SqlParameter("@PrecioUnitario", detalle.PrecioUnitario));
                cmd.Parameters.Add(new SqlParameter("@CostoUnitarioHistorico", detalle.CostoUnitarioHistorico));
                cmd.Parameters.Add(new SqlParameter("@MesesGarantiaHistorico", detalle.MesesGarantiaHistorico));
                cmd.Parameters.Add(new SqlParameter("@FechaFinGarantia", detalle.FechaFinGarantia == null ? DBNull.Value : detalle.FechaFinGarantia));

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
