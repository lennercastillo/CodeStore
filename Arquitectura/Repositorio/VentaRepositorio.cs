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
    public class VentaRepositorio : IVenta
    {
        private readonly BD _cone;

        public VentaRepositorio(BD cone)
        {
            _cone = cone;
        }

        public async Task EditarVentaAsync(Venta venta)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("ActualizarVenta", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdVenta", venta.IdVenta));
                cmd.Parameters.Add(new SqlParameter("@NumeroFactura", venta.NumeroFactura));
                cmd.Parameters.Add(new SqlParameter("@IdCliente", venta.IdCliente));
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioVendedor", venta.IdUsuarioVendedor));
                cmd.Parameters.Add(new SqlParameter("@FechaVenta", venta.FechaVenta));
                cmd.Parameters.Add(new SqlParameter("@TipoVenta", venta.TipoVenta));
                cmd.Parameters.Add(new SqlParameter("@MetodoPago", venta.MetodoPago));
                cmd.Parameters.Add(new SqlParameter("@PorcentajeImpuesto", venta.PorcentajeImpuesto));
                cmd.Parameters.Add(new SqlParameter("@Subtotal", venta.Subtotal));
                cmd.Parameters.Add(new SqlParameter("@Impuesto", venta.Impuesto));
                cmd.Parameters.Add(new SqlParameter("@TotalVenta", venta.TotalVenta));
                cmd.Parameters.Add(new SqlParameter("@MontoRecibido", venta.MontoRecibido));
                cmd.Parameters.Add(new SqlParameter("@Vuelto", venta.Vuelto));
                cmd.Parameters.Add(new SqlParameter("@Estado", venta.Estado));
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioAutorizaAnulacion", venta.IdUsuarioAutorizaAnulacion == null ? DBNull.Value : venta.IdUsuarioAutorizaAnulacion));
                cmd.Parameters.Add(new SqlParameter("@FechaAnulacion", venta.FechaAnulacion == null ? DBNull.Value : venta.FechaAnulacion));
                cmd.Parameters.Add(new SqlParameter("@MotivoAnulacion", venta.MotivoAnulacion == null ? DBNull.Value : venta.MotivoAnulacion));

                await cmd.ExecuteReaderAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task EliminarVentaAsync(int Id)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("EliminarVenta", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdVenta", Id));

                await cmd.ExecuteReaderAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Venta>> ListarVentaAsync()
        {
            var list = new List<Venta>();
            using var con = _cone.GetConnection();
            await con.OpenAsync();

            try
            {
                using var cmd = new SqlCommand("ListarVentas", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using var dr = await cmd.ExecuteReaderAsync();

                while (await dr.ReadAsync())
                {
                    var v = new Venta
                    {
                        IdVenta = Convert.ToInt32(dr["IdVenta"]),
                        NumeroFactura = dr["NumeroFactura"].ToString()!,
                        IdCliente = Convert.ToInt32(dr["IdCliente"]),
                        IdUsuarioVendedor = Convert.ToInt32(dr["IdUsuarioVendedor"]),
                        FechaVenta = Convert.ToDateTime(dr["FechaVenta"]),
                        TipoVenta = dr["TipoVenta"].ToString()!,
                        MetodoPago = dr["MetodoPago"].ToString()!,
                        PorcentajeImpuesto = Convert.ToDecimal(dr["PorcentajeImpuesto"]),
                        Subtotal = Convert.ToDecimal(dr["Subtotal"]),
                        Impuesto = Convert.ToDecimal(dr["Impuesto"]),
                        TotalVenta = Convert.ToDecimal(dr["TotalVenta"]),
                        MontoRecibido = Convert.ToDecimal(dr["MontoRecibido"]),
                        Vuelto = Convert.ToDecimal(dr["Vuelto"]),
                        Estado = dr["Estado"].ToString()!,
                        IdUsuarioAutorizaAnulacion = dr["IdUsuarioAutorizaAnulacion"] == DBNull.Value ? null : Convert.ToInt32(dr["IdUsuarioAutorizaAnulacion"]),
                        FechaAnulacion = dr["FechaAnulacion"] == DBNull.Value ? null : Convert.ToDateTime(dr["FechaAnulacion"]),
                        MotivoAnulacion = dr["MotivoAnulacion"] == DBNull.Value ? null : dr["MotivoAnulacion"].ToString()
                    };
                    list.Add(v);
                }
                return list;
            }
            catch
            {
                throw;
            }
        }

        public async Task NuevoVentaAsync(Venta venta)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("InsertarVenta", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@NumeroFactura", venta.NumeroFactura));
                cmd.Parameters.Add(new SqlParameter("@IdCliente", venta.IdCliente));
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioVendedor", venta.IdUsuarioVendedor));
                cmd.Parameters.Add(new SqlParameter("@TipoVenta", venta.TipoVenta));
                cmd.Parameters.Add(new SqlParameter("@MetodoPago", venta.MetodoPago));
                cmd.Parameters.Add(new SqlParameter("@FechaVenta", venta.FechaVenta));
                cmd.Parameters.Add(new SqlParameter("@PorcentajeImpuesto", venta.PorcentajeImpuesto));
                cmd.Parameters.Add(new SqlParameter("@Subtotal", venta.Subtotal));
                cmd.Parameters.Add(new SqlParameter("@Impuesto", venta.Impuesto));
                cmd.Parameters.Add(new SqlParameter("@TotalVenta", venta.TotalVenta));
                cmd.Parameters.Add(new SqlParameter("@MontoRecibido", venta.MontoRecibido));
                cmd.Parameters.Add(new SqlParameter("@Vuelto", venta.Vuelto));
                cmd.Parameters.Add(new SqlParameter("@Estado", venta.Estado));
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioAutorizaAnulacion", venta.IdUsuarioAutorizaAnulacion == null ? DBNull.Value : venta.IdUsuarioAutorizaAnulacion));
                cmd.Parameters.Add(new SqlParameter("@FechaAnulacion", venta.FechaAnulacion == null ? DBNull.Value : venta.FechaAnulacion));
                cmd.Parameters.Add(new SqlParameter("@MotivoAnulacion", venta.MotivoAnulacion == null ? DBNull.Value : venta.MotivoAnulacion));

                await cmd.ExecuteReaderAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
