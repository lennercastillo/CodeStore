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

                await cmd.ExecuteNonQueryAsync();
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

                await cmd.ExecuteNonQueryAsync();
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

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Venta?> ObtenerPorIdAsync(int id)
        {
            var lista = await ListarVentaAsync();
            return lista.FirstOrDefault(v => v.IdVenta == id);
        }

        public async Task<(bool Success, string Message, int IdVenta, string NumeroFactura)> RegistrarVentaTransaccionalAsync(
            Venta venta,
            List<DetalleVenta> detalles,
            List<DetalleVentaSerie> series,
            Credito? credito,
            List<CuotaCredito>? cuotas)
        {
            using var con = _cone.GetConnection();
            await con.OpenAsync();
            using var tran = con.BeginTransaction();

            try
            {
                // 1. Insertar Cabecera de Venta (en Pendiente para luego confirmar stock y Kardex)
                int idVentaGenerado = 0;
                string numeroFacturaGenerada = string.Empty;

                using (var insertVentaCmd = new SqlCommand("InsertarVenta", con, tran))
                {
                    insertVentaCmd.CommandType = CommandType.StoredProcedure;
                    insertVentaCmd.Parameters.Add(new SqlParameter("@NumeroFactura", string.IsNullOrWhiteSpace(venta.NumeroFactura) ? DBNull.Value : venta.NumeroFactura));
                    insertVentaCmd.Parameters.Add(new SqlParameter("@IdCliente", venta.IdCliente));
                    insertVentaCmd.Parameters.Add(new SqlParameter("@IdUsuarioVendedor", venta.IdUsuarioVendedor));
                    insertVentaCmd.Parameters.Add(new SqlParameter("@TipoVenta", venta.TipoVenta));
                    insertVentaCmd.Parameters.Add(new SqlParameter("@MetodoPago", venta.MetodoPago));
                    insertVentaCmd.Parameters.Add(new SqlParameter("@FechaVenta", venta.FechaVenta == default ? DBNull.Value : venta.FechaVenta));
                    insertVentaCmd.Parameters.Add(new SqlParameter("@PorcentajeImpuesto", venta.PorcentajeImpuesto));
                    insertVentaCmd.Parameters.Add(new SqlParameter("@Subtotal", venta.Subtotal));
                    insertVentaCmd.Parameters.Add(new SqlParameter("@Impuesto", venta.Impuesto));
                    insertVentaCmd.Parameters.Add(new SqlParameter("@TotalVenta", venta.TotalVenta));
                    insertVentaCmd.Parameters.Add(new SqlParameter("@MontoRecibido", venta.MontoRecibido));
                    insertVentaCmd.Parameters.Add(new SqlParameter("@Vuelto", venta.Vuelto));
                    insertVentaCmd.Parameters.Add(new SqlParameter("@Estado", "Pendiente"));

                    using var rdr = await insertVentaCmd.ExecuteReaderAsync();
                    if (await rdr.ReadAsync())
                    {
                        idVentaGenerado = Convert.ToInt32(rdr["IdVenta"]);
                        numeroFacturaGenerada = rdr["NumeroFactura"].ToString()!;
                    }
                }

                // 2. Insertar Detalle y Series
                foreach (var item in detalles)
                {
                    int idDetalleGenerado;
                    using (var insertDetalleCmd = new SqlCommand("InsertarDetalleVenta", con, tran))
                    {
                        insertDetalleCmd.CommandType = CommandType.StoredProcedure;
                        insertDetalleCmd.Parameters.Add(new SqlParameter("@IdVenta", idVentaGenerado));
                        insertDetalleCmd.Parameters.Add(new SqlParameter("@IdProducto", item.IdProducto));
                        insertDetalleCmd.Parameters.Add(new SqlParameter("@Cantidad", item.Cantidad));
                        insertDetalleCmd.Parameters.Add(new SqlParameter("@PrecioUnitario", item.PrecioUnitario));

                        idDetalleGenerado = Convert.ToInt32(await insertDetalleCmd.ExecuteScalarAsync());
                    }

                    var seriesProducto = series.Where(s => s.IdDetalleVenta == item.IdProducto).ToList();
                    foreach (var s in seriesProducto)
                    {
                        using var insertSerieCmd = new SqlCommand("InsertarDetalleVentaSerie", con, tran);
                        insertSerieCmd.CommandType = CommandType.StoredProcedure;
                        insertSerieCmd.Parameters.Add(new SqlParameter("@IdDetalleVenta", idDetalleGenerado));
                        insertSerieCmd.Parameters.Add(new SqlParameter("@NumeroSerie", s.NumeroSerie));
                        insertSerieCmd.Parameters.Add(new SqlParameter("@EstadoSerie", string.IsNullOrWhiteSpace(s.EstadoSerie) ? "Vendido" : s.EstadoSerie));
                        await insertSerieCmd.ExecuteNonQueryAsync();
                    }
                }

                // 3. Confirmar venta (valida stock, actualiza garantías, descuenta stock, registra Kardex y marca Completada)
                using (var confirmarCmd = new SqlCommand("ConfirmarVenta", con, tran))
                {
                    confirmarCmd.CommandType = CommandType.StoredProcedure;
                    confirmarCmd.Parameters.Add(new SqlParameter("@IdVenta", idVentaGenerado));
                    confirmarCmd.Parameters.Add(new SqlParameter("@IdUsuarioVendedor", venta.IdUsuarioVendedor));
                    await confirmarCmd.ExecuteNonQueryAsync();
                }

                // 4. Crédito y Cuotas si aplica
                if (credito != null)
                {
                    int idCreditoGenerado;
                    using (var insertCreditoCmd = new SqlCommand("InsertarCredito", con, tran))
                    {
                        insertCreditoCmd.CommandType = CommandType.StoredProcedure;
                        insertCreditoCmd.Parameters.Add(new SqlParameter("@IdVenta", idVentaGenerado));
                        insertCreditoCmd.Parameters.Add(new SqlParameter("@MontoVentaTotal", credito.MontoVentaTotal));
                        insertCreditoCmd.Parameters.Add(new SqlParameter("@MontoFinanciado", credito.MontoFinanciado));
                        insertCreditoCmd.Parameters.Add(new SqlParameter("@FrecuenciaPago", credito.FrecuenciaPago));
                        insertCreditoCmd.Parameters.Add(new SqlParameter("@CantidadCuotas", credito.CantidadCuotas));
                        insertCreditoCmd.Parameters.Add(new SqlParameter("@MontoTotalCredito", credito.MontoTotalCredito));
                        insertCreditoCmd.Parameters.Add(new SqlParameter("@SaldoPendiente", credito.SaldoPendiente));
                        insertCreditoCmd.Parameters.Add(new SqlParameter("@PrimaInicial", credito.PrimaInicial));
                        insertCreditoCmd.Parameters.Add(new SqlParameter("@PorcentajeInteres", credito.PorcentajeInteres));
                        insertCreditoCmd.Parameters.Add(new SqlParameter("@MontoInteresesPactados", credito.MontoInteresesPactados));
                        insertCreditoCmd.Parameters.Add(new SqlParameter("@Estado", "Activo"));

                        idCreditoGenerado = Convert.ToInt32(await insertCreditoCmd.ExecuteScalarAsync());
                    }

                    if (cuotas != null)
                    {
                        foreach (var cuota in cuotas)
                        {
                            using var insertCuotaCmd = new SqlCommand("InsertarCuotaCredito", con, tran);
                            insertCuotaCmd.CommandType = CommandType.StoredProcedure;
                            insertCuotaCmd.Parameters.Add(new SqlParameter("@IdCredito", idCreditoGenerado));
                            insertCuotaCmd.Parameters.Add(new SqlParameter("@NumeroCuota", cuota.NumeroCuota));
                            insertCuotaCmd.Parameters.Add(new SqlParameter("@FechaVencimiento", cuota.FechaVencimiento));
                            insertCuotaCmd.Parameters.Add(new SqlParameter("@MontoCuota", cuota.MontoCuota));
                            await insertCuotaCmd.ExecuteNonQueryAsync();
                        }
                    }
                }

                await tran.CommitAsync();
                return (true, "Venta completada exitosamente.", idVentaGenerado, numeroFacturaGenerada);
            }
            catch (Exception ex)
            {
                try { await tran.RollbackAsync(); } catch { }
                return (false, ex.Message, 0, string.Empty);
            }
        }

        public async Task<(bool Success, string Message)> AnularVentaAsync(int idVenta, int idUsuarioAutoriza, string motivo)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("AnularVenta", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdVenta", idVenta));
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioAutoriza", idUsuarioAutoriza));
                cmd.Parameters.Add(new SqlParameter("@MotivoAnulacion", motivo));

                string numeroFactura = string.Empty;
                using var rdr = await cmd.ExecuteReaderAsync();
                if (await rdr.ReadAsync())
                {
                    numeroFactura = rdr["NumeroFactura"].ToString()!;
                }

                return (true, $"Factura {numeroFactura} anulada. Stock, Kardex y crédito revertidos correctamente.");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
