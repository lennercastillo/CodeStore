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
                // 1. Validar Stock y bloquear filas con UPDLOCK
                foreach (var item in detalles)
                {
                    using var checkCmd = new SqlCommand(@"
                        SELECT Nombre, StockActual, Activo, UltimoCostoCompra, MesesGarantia, RequiereSerie
                        FROM Productos WITH (UPDLOCK)
                        WHERE IdProducto = @IdProducto", con, tran);
                    checkCmd.Parameters.Add(new SqlParameter("@IdProducto", item.IdProducto));

                    using var rdr = await checkCmd.ExecuteReaderAsync();
                    if (!await rdr.ReadAsync())
                    {
                        await tran.RollbackAsync();
                        return (false, $"El producto con ID #{item.IdProducto} no existe.", 0, string.Empty);
                    }

                    var nombreProd = rdr["Nombre"].ToString();
                    var stockActual = Convert.ToInt32(rdr["StockActual"]);
                    var activo = Convert.ToBoolean(rdr["Activo"]);
                    var ultimoCosto = Convert.ToDecimal(rdr["UltimoCostoCompra"]);
                    var mesesGarantia = Convert.ToInt32(rdr["MesesGarantia"]);

                    if (!activo)
                    {
                        await tran.RollbackAsync();
                        return (false, $"El producto \"{nombreProd}\" se encuentra inactivo.", 0, string.Empty);
                    }

                    if (stockActual < item.Cantidad)
                    {
                        await tran.RollbackAsync();
                        return (false, $"Stock insuficiente para \"{nombreProd}\". Disponible: {stockActual}, solicitado: {item.Cantidad}.", 0, string.Empty);
                    }

                    item.CostoUnitarioHistorico = ultimoCosto;
                    item.MesesGarantiaHistorico = mesesGarantia;
                    if (mesesGarantia > 0)
                    {
                        item.FechaFinGarantia = DateTime.Today.AddMonths(mesesGarantia);
                    }

                    rdr.Close();
                }

                // 2. Generar Número de Factura consecutivo si viene vacío
                if (string.IsNullOrWhiteSpace(venta.NumeroFactura))
                {
                    using var countCmd = new SqlCommand("SELECT COUNT(1) + 1 FROM Ventas", con, tran);
                    var count = Convert.ToInt32(await countCmd.ExecuteScalarAsync());
                    venta.NumeroFactura = $"FAC-{DateTime.Now:yyyyMMdd}-{count:D4}";
                }

                // 3. Insertar Cabecera de Venta
                int idVentaGenerado;
                using (var insertVentaCmd = new SqlCommand(@"
                    INSERT INTO Ventas (
                        NumeroFactura, IdCliente, IdUsuarioVendedor, FechaVenta, TipoVenta, MetodoPago,
                        PorcentajeImpuesto, Subtotal, Impuesto, TotalVenta, MontoRecibido, Vuelto, Estado
                    ) VALUES (
                        @NumeroFactura, @IdCliente, @IdUsuarioVendedor, @FechaVenta, @TipoVenta, @MetodoPago,
                        @PorcentajeImpuesto, @Subtotal, @Impuesto, @TotalVenta, @MontoRecibido, @Vuelto, @Estado
                    );
                    SELECT CAST(SCOPE_IDENTITY() AS INT);", con, tran))
                {
                    insertVentaCmd.Parameters.Add(new SqlParameter("@NumeroFactura", venta.NumeroFactura));
                    insertVentaCmd.Parameters.Add(new SqlParameter("@IdCliente", venta.IdCliente));
                    insertVentaCmd.Parameters.Add(new SqlParameter("@IdUsuarioVendedor", venta.IdUsuarioVendedor));
                    insertVentaCmd.Parameters.Add(new SqlParameter("@FechaVenta", venta.FechaVenta == default ? DateTime.Now : venta.FechaVenta));
                    insertVentaCmd.Parameters.Add(new SqlParameter("@TipoVenta", venta.TipoVenta));
                    insertVentaCmd.Parameters.Add(new SqlParameter("@MetodoPago", venta.MetodoPago));
                    insertVentaCmd.Parameters.Add(new SqlParameter("@PorcentajeImpuesto", venta.PorcentajeImpuesto));
                    insertVentaCmd.Parameters.Add(new SqlParameter("@Subtotal", venta.Subtotal));
                    insertVentaCmd.Parameters.Add(new SqlParameter("@Impuesto", venta.Impuesto));
                    insertVentaCmd.Parameters.Add(new SqlParameter("@TotalVenta", venta.TotalVenta));
                    insertVentaCmd.Parameters.Add(new SqlParameter("@MontoRecibido", venta.MontoRecibido));
                    insertVentaCmd.Parameters.Add(new SqlParameter("@Vuelto", venta.Vuelto));
                    insertVentaCmd.Parameters.Add(new SqlParameter("@Estado", string.IsNullOrWhiteSpace(venta.Estado) ? "Completada" : venta.Estado));

                    idVentaGenerado = (int)(await insertVentaCmd.ExecuteScalarAsync())!;
                }

                // 4. Insertar Detalle, Kardex y descontar Stock
                foreach (var item in detalles)
                {
                    item.IdVenta = idVentaGenerado;
                    int idDetalleGenerado;

                    using (var insertDetalleCmd = new SqlCommand(@"
                        INSERT INTO DetalleVentas (
                            IdVenta, IdProducto, Cantidad, PrecioUnitario, CostoUnitarioHistorico,
                            MesesGarantiaHistorico, FechaFinGarantia
                        ) VALUES (
                            @IdVenta, @IdProducto, @Cantidad, @PrecioUnitario, @CostoUnitarioHistorico,
                            @MesesGarantiaHistorico, @FechaFinGarantia
                        );
                        SELECT CAST(SCOPE_IDENTITY() AS INT);", con, tran))
                    {
                        insertDetalleCmd.Parameters.Add(new SqlParameter("@IdVenta", item.IdVenta));
                        insertDetalleCmd.Parameters.Add(new SqlParameter("@IdProducto", item.IdProducto));
                        insertDetalleCmd.Parameters.Add(new SqlParameter("@Cantidad", item.Cantidad));
                        insertDetalleCmd.Parameters.Add(new SqlParameter("@PrecioUnitario", item.PrecioUnitario));
                        insertDetalleCmd.Parameters.Add(new SqlParameter("@CostoUnitarioHistorico", item.CostoUnitarioHistorico));
                        insertDetalleCmd.Parameters.Add(new SqlParameter("@MesesGarantiaHistorico", item.MesesGarantiaHistorico));
                        insertDetalleCmd.Parameters.Add(new SqlParameter("@FechaFinGarantia", (object?)item.FechaFinGarantia ?? DBNull.Value));

                        idDetalleGenerado = (int)(await insertDetalleCmd.ExecuteScalarAsync())!;
                    }

                    // Series si aplican
                    var seriesProducto = series.Where(s => s.IdDetalleVenta == item.IdProducto || s.IdDetalleVenta == 0).ToList();
                    foreach (var s in seriesProducto)
                    {
                        using var insertSerieCmd = new SqlCommand(@"
                            INSERT INTO DetalleVentaSeries (IdDetalleVenta, NumeroSerie, EstadoSerie, FechaRegistro)
                            VALUES (@IdDetalleVenta, @NumeroSerie, @EstadoSerie, GETDATE())", con, tran);
                        insertSerieCmd.Parameters.Add(new SqlParameter("@IdDetalleVenta", idDetalleGenerado));
                        insertSerieCmd.Parameters.Add(new SqlParameter("@NumeroSerie", s.NumeroSerie));
                        insertSerieCmd.Parameters.Add(new SqlParameter("@EstadoSerie", string.IsNullOrWhiteSpace(s.EstadoSerie) ? "Vendido" : s.EstadoSerie));
                        await insertSerieCmd.ExecuteNonQueryAsync();
                    }

                    // Obtener Stock anterior para Kardex
                    int stockAnterior = 0;
                    using (var stockCmd = new SqlCommand("SELECT StockActual FROM Productos WHERE IdProducto = @IdProducto", con, tran))
                    {
                        stockCmd.Parameters.Add(new SqlParameter("@IdProducto", item.IdProducto));
                        stockAnterior = Convert.ToInt32(await stockCmd.ExecuteScalarAsync());
                    }

                    int stockNuevo = stockAnterior - item.Cantidad;

                    // Descontar Stock
                    using (var updateStockCmd = new SqlCommand(@"
                        UPDATE Productos
                        SET StockActual = StockActual - @Cantidad
                        WHERE IdProducto = @IdProducto", con, tran))
                    {
                        updateStockCmd.Parameters.Add(new SqlParameter("@Cantidad", item.Cantidad));
                        updateStockCmd.Parameters.Add(new SqlParameter("@IdProducto", item.IdProducto));
                        await updateStockCmd.ExecuteNonQueryAsync();
                    }

                    // Registrar en Kardex
                    using (var insertKardexCmd = new SqlCommand(@"
                        INSERT INTO Kardex (
                            IdProducto, FechaMovimiento, TipoMovimiento, Cantidad, StockAnterior, StockNuevo,
                            CostoUnitario, DocumentoOrigen, IdReferenciaOrigen, Justificacion, IdUsuarioResponsable
                        ) VALUES (
                            @IdProducto, GETDATE(), 'Salida por Venta', @Cantidad, @StockAnterior, @StockNuevo,
                            @CostoUnitario, @DocumentoOrigen, @IdReferenciaOrigen, 'Venta POS confirmada', @IdUsuarioResponsable
                        )", con, tran))
                    {
                        insertKardexCmd.Parameters.Add(new SqlParameter("@IdProducto", item.IdProducto));
                        insertKardexCmd.Parameters.Add(new SqlParameter("@Cantidad", item.Cantidad));
                        insertKardexCmd.Parameters.Add(new SqlParameter("@StockAnterior", stockAnterior));
                        insertKardexCmd.Parameters.Add(new SqlParameter("@StockNuevo", stockNuevo));
                        insertKardexCmd.Parameters.Add(new SqlParameter("@CostoUnitario", item.CostoUnitarioHistorico));
                        insertKardexCmd.Parameters.Add(new SqlParameter("@DocumentoOrigen", venta.NumeroFactura));
                        insertKardexCmd.Parameters.Add(new SqlParameter("@IdReferenciaOrigen", idVentaGenerado));
                        insertKardexCmd.Parameters.Add(new SqlParameter("@IdUsuarioResponsable", venta.IdUsuarioVendedor));
                        await insertKardexCmd.ExecuteNonQueryAsync();
                    }
                }

                // 5. Crédito si aplica
                if (credito != null)
                {
                    credito.IdVenta = idVentaGenerado;
                    int idCreditoGenerado;

                    using (var insertCreditoCmd = new SqlCommand(@"
                        INSERT INTO Creditos (
                            IdVenta, MontoVentaTotal, PrimaInicial, MontoFinanciado, FrecuenciaPago,
                            CantidadCuotas, PorcentajeInteres, MontoInteresesPactados, MontoTotalCredito,
                            SaldoPendiente, Estado, FechaRegistro
                        ) VALUES (
                            @IdVenta, @MontoVentaTotal, @PrimaInicial, @MontoFinanciado, @FrecuenciaPago,
                            @CantidadCuotas, @PorcentajeInteres, @MontoInteresesPactados, @MontoTotalCredito,
                            @SaldoPendiente, 'Activo', GETDATE()
                        );
                        SELECT CAST(SCOPE_IDENTITY() AS INT);", con, tran))
                    {
                        insertCreditoCmd.Parameters.Add(new SqlParameter("@IdVenta", credito.IdVenta));
                        insertCreditoCmd.Parameters.Add(new SqlParameter("@MontoVentaTotal", credito.MontoVentaTotal));
                        insertCreditoCmd.Parameters.Add(new SqlParameter("@PrimaInicial", credito.PrimaInicial));
                        insertCreditoCmd.Parameters.Add(new SqlParameter("@MontoFinanciado", credito.MontoFinanciado));
                        insertCreditoCmd.Parameters.Add(new SqlParameter("@FrecuenciaPago", credito.FrecuenciaPago));
                        insertCreditoCmd.Parameters.Add(new SqlParameter("@CantidadCuotas", credito.CantidadCuotas));
                        insertCreditoCmd.Parameters.Add(new SqlParameter("@PorcentajeInteres", credito.PorcentajeInteres));
                        insertCreditoCmd.Parameters.Add(new SqlParameter("@MontoInteresesPactados", credito.MontoInteresesPactados));
                        insertCreditoCmd.Parameters.Add(new SqlParameter("@MontoTotalCredito", credito.MontoTotalCredito));
                        insertCreditoCmd.Parameters.Add(new SqlParameter("@SaldoPendiente", credito.SaldoPendiente));

                        idCreditoGenerado = (int)(await insertCreditoCmd.ExecuteScalarAsync())!;
                    }

                    if (cuotas != null)
                    {
                        foreach (var cuota in cuotas)
                        {
                            using var insertCuotaCmd = new SqlCommand(@"
                                INSERT INTO CuotasCredito (
                                    IdCredito, NumeroCuota, FechaVencimiento, MontoCuota,
                                    RecargoMora, MoraAbonada, CapitalInteresAbonado, Estado
                                ) VALUES (
                                    @IdCredito, @NumeroCuota, @FechaVencimiento, @MontoCuota,
                                    0, 0, 0, 'Pendiente'
                                )", con, tran);
                            insertCuotaCmd.Parameters.Add(new SqlParameter("@IdCredito", idCreditoGenerado));
                            insertCuotaCmd.Parameters.Add(new SqlParameter("@NumeroCuota", cuota.NumeroCuota));
                            insertCuotaCmd.Parameters.Add(new SqlParameter("@FechaVencimiento", cuota.FechaVencimiento));
                            insertCuotaCmd.Parameters.Add(new SqlParameter("@MontoCuota", cuota.MontoCuota));
                            await insertCuotaCmd.ExecuteNonQueryAsync();
                        }
                    }
                }

                await tran.CommitAsync();
                return (true, "Venta completada exitosamente.", idVentaGenerado, venta.NumeroFactura);
            }
            catch (Exception ex)
            {
                await tran.RollbackAsync();
                return (false, $"Error al procesar la venta: {ex.Message}", 0, string.Empty);
            }
        }

        public async Task<(bool Success, string Message)> AnularVentaAsync(int idVenta, int idUsuarioAutoriza, string motivo)
        {
            using var con = _cone.GetConnection();
            await con.OpenAsync();
            using var tran = con.BeginTransaction();

            try
            {
                string? estadoVenta = null;
                string? numeroFactura = null;

                using (var checkCmd = new SqlCommand("SELECT Estado, NumeroFactura FROM Ventas WITH (UPDLOCK) WHERE IdVenta = @IdVenta", con, tran))
                {
                    checkCmd.Parameters.Add(new SqlParameter("@IdVenta", idVenta));
                    using var rdr = await checkCmd.ExecuteReaderAsync();
                    if (!await rdr.ReadAsync())
                    {
                        await tran.RollbackAsync();
                        return (false, "La venta especificada no existe.");
                    }
                    estadoVenta = rdr["Estado"].ToString();
                    numeroFactura = rdr["NumeroFactura"].ToString();
                }

                if (estadoVenta == "Anulada")
                {
                    await tran.RollbackAsync();
                    return (false, "La venta ya se encuentra anulada.");
                }

                if (estadoVenta == "Completada")
                {
                    var items = new List<(int IdProducto, int Cantidad, decimal Costo)>();
                    using (var detCmd = new SqlCommand("SELECT IdProducto, Cantidad, CostoUnitarioHistorico FROM DetalleVentas WHERE IdVenta = @IdVenta", con, tran))
                    {
                        detCmd.Parameters.Add(new SqlParameter("@IdVenta", idVenta));
                        using var rdr = await detCmd.ExecuteReaderAsync();
                        while (await rdr.ReadAsync())
                        {
                            items.Add((Convert.ToInt32(rdr["IdProducto"]), Convert.ToInt32(rdr["Cantidad"]), Convert.ToDecimal(rdr["CostoUnitarioHistorico"])));
                        }
                    }

                    foreach (var item in items)
                    {
                        int stockAnterior = 0;
                        using (var prodCmd = new SqlCommand("SELECT StockActual FROM Productos WITH (UPDLOCK) WHERE IdProducto = @IdProducto", con, tran))
                        {
                            prodCmd.Parameters.Add(new SqlParameter("@IdProducto", item.IdProducto));
                            stockAnterior = Convert.ToInt32(await prodCmd.ExecuteScalarAsync());
                        }

                        int stockNuevo = stockAnterior + item.Cantidad;

                        using (var karCmd = new SqlCommand(@"
                            INSERT INTO Kardex (
                                IdProducto, FechaMovimiento, TipoMovimiento, Cantidad, StockAnterior, StockNuevo,
                                CostoUnitario, DocumentoOrigen, IdReferenciaOrigen, Justificacion, IdUsuarioResponsable
                            ) VALUES (
                                @IdProducto, GETDATE(), 'Reversion por Anulacion Venta', @Cantidad, @StockAnterior, @StockNuevo,
                                @CostoUnitario, @DocumentoOrigen, @IdReferenciaOrigen, @Justificacion, @IdUsuarioResponsable
                            )", con, tran))
                        {
                            karCmd.Parameters.Add(new SqlParameter("@IdProducto", item.IdProducto));
                            karCmd.Parameters.Add(new SqlParameter("@Cantidad", item.Cantidad));
                            karCmd.Parameters.Add(new SqlParameter("@StockAnterior", stockAnterior));
                            karCmd.Parameters.Add(new SqlParameter("@StockNuevo", stockNuevo));
                            karCmd.Parameters.Add(new SqlParameter("@CostoUnitario", item.Costo));
                            karCmd.Parameters.Add(new SqlParameter("@DocumentoOrigen", numeroFactura));
                            karCmd.Parameters.Add(new SqlParameter("@IdReferenciaOrigen", idVenta));
                            karCmd.Parameters.Add(new SqlParameter("@Justificacion", motivo));
                            karCmd.Parameters.Add(new SqlParameter("@IdUsuarioResponsable", idUsuarioAutoriza));
                            await karCmd.ExecuteNonQueryAsync();
                        }

                        using (var updProd = new SqlCommand("UPDATE Productos SET StockActual = StockActual + @Cantidad WHERE IdProducto = @IdProducto", con, tran))
                        {
                            updProd.Parameters.Add(new SqlParameter("@Cantidad", item.Cantidad));
                            updProd.Parameters.Add(new SqlParameter("@IdProducto", item.IdProducto));
                            await updProd.ExecuteNonQueryAsync();
                        }
                    }
                }

                using (var updCuotas = new SqlCommand("UPDATE CuotasCredito SET Estado = 'Anulada' WHERE IdCredito IN (SELECT IdCredito FROM Creditos WHERE IdVenta = @IdVenta)", con, tran))
                {
                    updCuotas.Parameters.Add(new SqlParameter("@IdVenta", idVenta));
                    await updCuotas.ExecuteNonQueryAsync();
                }

                using (var updCred = new SqlCommand("UPDATE Creditos SET Estado = 'Anulado', SaldoPendiente = 0 WHERE IdVenta = @IdVenta", con, tran))
                {
                    updCred.Parameters.Add(new SqlParameter("@IdVenta", idVenta));
                    await updCred.ExecuteNonQueryAsync();
                }

                using (var updVenta = new SqlCommand(@"
                    UPDATE Ventas
                    SET Estado = 'Anulada', IdUsuarioAutorizaAnulacion = @IdUsuario, FechaAnulacion = GETDATE(), MotivoAnulacion = @Motivo
                    WHERE IdVenta = @IdVenta", con, tran))
                {
                    updVenta.Parameters.Add(new SqlParameter("@IdUsuario", idUsuarioAutoriza));
                    updVenta.Parameters.Add(new SqlParameter("@Motivo", motivo));
                    updVenta.Parameters.Add(new SqlParameter("@IdVenta", idVenta));
                    await updVenta.ExecuteNonQueryAsync();
                }

                await tran.CommitAsync();
                return (true, $"Factura {numeroFactura} anulada. Stock, Kardex y crédito revertidos correctamente.");
            }
            catch (Exception ex)
            {
                await tran.RollbackAsync();
                return (false, $"Error al anular venta: {ex.Message}");
            }
        }
    }
}
