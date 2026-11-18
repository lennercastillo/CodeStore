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
    public class CompraRepositorio : ICompra
    {
        private readonly BD _cone;

        public CompraRepositorio(BD cone)
        {
            _cone = cone;
        }

        public async Task EditarCompraAsync(Compra compra)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("ActualizarCompra", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdCompra", compra.IdCompra));
                cmd.Parameters.Add(new SqlParameter("@NumeroDocumento", compra.NumeroDocumento));
                cmd.Parameters.Add(new SqlParameter("@IdProveedor", compra.IdProveedor));
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioRegistro", compra.IdUsuarioRegistro));
                cmd.Parameters.Add(new SqlParameter("@FechaCompra", compra.FechaCompra));
                cmd.Parameters.Add(new SqlParameter("@TotalCompra", compra.TotalCompra));
                cmd.Parameters.Add(new SqlParameter("@Estado", compra.Estado));
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioRecepcion", compra.IdUsuarioRecepcion == null ? DBNull.Value : compra.IdUsuarioRecepcion));
                cmd.Parameters.Add(new SqlParameter("@FechaRecepcion", compra.FechaRecepcion == null ? DBNull.Value : compra.FechaRecepcion));
                cmd.Parameters.Add(new SqlParameter("@Observaciones", compra.Observaciones == null ? DBNull.Value : compra.Observaciones));
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioAnulacion", compra.IdUsuarioAnulacion == null ? DBNull.Value : compra.IdUsuarioAnulacion));
                cmd.Parameters.Add(new SqlParameter("@FechaAnulacion", compra.FechaAnulacion == null ? DBNull.Value : compra.FechaAnulacion));
                cmd.Parameters.Add(new SqlParameter("@MotivoAnulacion", compra.MotivoAnulacion == null ? DBNull.Value : compra.MotivoAnulacion));

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task EliminarCompraAsync(int Id)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("EliminarCompra", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdCompra", Id));

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Compra>> ListarCompraAsync()
        {
            var list = new List<Compra>();
            using var con = _cone.GetConnection();
            await con.OpenAsync();

            try
            {
                using var cmd = new SqlCommand("ListarCompras", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using var dr = await cmd.ExecuteReaderAsync();

                while (await dr.ReadAsync())
                {
                    var c = new Compra
                    {
                        IdCompra = Convert.ToInt32(dr["IdCompra"]),
                        NumeroDocumento = dr["NumeroDocumento"].ToString()!,
                        IdProveedor = Convert.ToInt32(dr["IdProveedor"]),
                        IdUsuarioRegistro = Convert.ToInt32(dr["IdUsuarioRegistro"]),
                        IdUsuarioRecepcion = dr["IdUsuarioRecepcion"] == DBNull.Value ? null : Convert.ToInt32(dr["IdUsuarioRecepcion"]),
                        FechaCompra = Convert.ToDateTime(dr["FechaCompra"]),
                        FechaRecepcion = dr["FechaRecepcion"] == DBNull.Value ? null : Convert.ToDateTime(dr["FechaRecepcion"]),
                        TotalCompra = Convert.ToDecimal(dr["TotalCompra"]),
                        Estado = dr["Estado"].ToString()!,
                        Observaciones = dr["Observaciones"] == DBNull.Value ? null : dr["Observaciones"].ToString(),
                        IdUsuarioAnulacion = dr["IdUsuarioAnulacion"] == DBNull.Value ? null : Convert.ToInt32(dr["IdUsuarioAnulacion"]),
                        FechaAnulacion = dr["FechaAnulacion"] == DBNull.Value ? null : Convert.ToDateTime(dr["FechaAnulacion"]),
                        MotivoAnulacion = dr["MotivoAnulacion"] == DBNull.Value ? null : dr["MotivoAnulacion"].ToString()
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

        public async Task NuevoCompraAsync(Compra compra)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("InsertarCompra", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@NumeroDocumento", compra.NumeroDocumento));
                cmd.Parameters.Add(new SqlParameter("@IdProveedor", compra.IdProveedor));
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioRegistro", compra.IdUsuarioRegistro));
                cmd.Parameters.Add(new SqlParameter("@FechaCompra", compra.FechaCompra));
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioRecepcion", compra.IdUsuarioRecepcion == null ? DBNull.Value : compra.IdUsuarioRecepcion));
                cmd.Parameters.Add(new SqlParameter("@FechaRecepcion", compra.FechaRecepcion == null ? DBNull.Value : compra.FechaRecepcion));
                cmd.Parameters.Add(new SqlParameter("@TotalCompra", compra.TotalCompra));
                cmd.Parameters.Add(new SqlParameter("@Estado", compra.Estado));
                cmd.Parameters.Add(new SqlParameter("@Observaciones", compra.Observaciones == null ? DBNull.Value : compra.Observaciones));
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioAnulacion", compra.IdUsuarioAnulacion == null ? DBNull.Value : compra.IdUsuarioAnulacion));
                cmd.Parameters.Add(new SqlParameter("@FechaAnulacion", compra.FechaAnulacion == null ? DBNull.Value : compra.FechaAnulacion));
                cmd.Parameters.Add(new SqlParameter("@MotivoAnulacion", compra.MotivoAnulacion == null ? DBNull.Value : compra.MotivoAnulacion));

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Compra?> ObtenerPorIdAsync(int id)
        {
            var lista = await ListarCompraAsync();
            return lista.FirstOrDefault(c => c.IdCompra == id);
        }

        public async Task<(bool Success, string Message, int IdCompra, string NumeroDocumento)> RegistrarCompraTransaccionalAsync(
            Compra compra,
            List<DetalleCompra> detalles,
            bool recibirInmediatamente)
        {
            using var con = _cone.GetConnection();
            await con.OpenAsync();
            using var tran = con.BeginTransaction();

            try
            {
                if (string.IsNullOrWhiteSpace(compra.NumeroDocumento))
                {
                    using var countCmd = new SqlCommand("SELECT COUNT(1) + 1 FROM Compras", con, tran);
                    var count = Convert.ToInt32(await countCmd.ExecuteScalarAsync());
                    compra.NumeroDocumento = $"COM-{DateTime.Now:yyyyMMdd}-{count:D4}";
                }

                compra.Estado = recibirInmediatamente ? "Recibida" : "Pendiente";
                if (recibirInmediatamente)
                {
                    compra.IdUsuarioRecepcion = compra.IdUsuarioRegistro;
                    compra.FechaRecepcion = DateTime.Now;
                }

                int idCompraGenerado;
                using (var insertCmd = new SqlCommand(@"
                    INSERT INTO Compras (
                        NumeroDocumento, IdProveedor, IdUsuarioRegistro, IdUsuarioRecepcion,
                        FechaCompra, FechaRecepcion, TotalCompra, Estado, Observaciones
                    ) VALUES (
                        @NumeroDocumento, @IdProveedor, @IdUsuarioRegistro, @IdUsuarioRecepcion,
                        @FechaCompra, @FechaRecepcion, @TotalCompra, @Estado, @Observaciones
                    );
                    SELECT CAST(SCOPE_IDENTITY() AS INT);", con, tran))
                {
                    insertCmd.Parameters.Add(new SqlParameter("@NumeroDocumento", compra.NumeroDocumento));
                    insertCmd.Parameters.Add(new SqlParameter("@IdProveedor", compra.IdProveedor));
                    insertCmd.Parameters.Add(new SqlParameter("@IdUsuarioRegistro", compra.IdUsuarioRegistro));
                    insertCmd.Parameters.Add(new SqlParameter("@IdUsuarioRecepcion", (object?)compra.IdUsuarioRecepcion ?? DBNull.Value));
                    insertCmd.Parameters.Add(new SqlParameter("@FechaCompra", compra.FechaCompra == default ? DateTime.Now : compra.FechaCompra));
                    insertCmd.Parameters.Add(new SqlParameter("@FechaRecepcion", (object?)compra.FechaRecepcion ?? DBNull.Value));
                    insertCmd.Parameters.Add(new SqlParameter("@TotalCompra", compra.TotalCompra));
                    insertCmd.Parameters.Add(new SqlParameter("@Estado", compra.Estado));
                    insertCmd.Parameters.Add(new SqlParameter("@Observaciones", (object?)compra.Observaciones ?? DBNull.Value));

                    idCompraGenerado = (int)(await insertCmd.ExecuteScalarAsync())!;
                }

                foreach (var item in detalles)
                {
                    using (var detCmd = new SqlCommand(@"
                        INSERT INTO DetalleCompras (IdCompra, IdProducto, Cantidad, CostoUnitario)
                        VALUES (@IdCompra, @IdProducto, @Cantidad, @CostoUnitario);", con, tran))
                    {
                        detCmd.Parameters.Add(new SqlParameter("@IdCompra", idCompraGenerado));
                        detCmd.Parameters.Add(new SqlParameter("@IdProducto", item.IdProducto));
                        detCmd.Parameters.Add(new SqlParameter("@Cantidad", item.Cantidad));
                        detCmd.Parameters.Add(new SqlParameter("@CostoUnitario", item.CostoUnitario));
                        await detCmd.ExecuteNonQueryAsync();
                    }

                    if (recibirInmediatamente)
                    {
                        await AplicarEntradaInventarioCompraAsync(
                            con, tran, item.IdProducto, item.Cantidad, item.CostoUnitario,
                            compra.NumeroDocumento, idCompraGenerado, compra.IdUsuarioRegistro);
                    }
                }

                await tran.CommitAsync();
                return (true, recibirInmediatamente ? "Compra registrada y recibida en inventario." : "Orden de compra registrada como Pendiente.", idCompraGenerado, compra.NumeroDocumento);
            }
            catch (Exception ex)
            {
                await tran.RollbackAsync();
                return (false, $"Error al registrar compra: {ex.Message}", 0, string.Empty);
            }
        }

        public async Task<(bool Success, string Message)> ConfirmarRecepcionAsync(int idCompra, int idUsuarioRecepcion)
        {
            using var con = _cone.GetConnection();
            await con.OpenAsync();
            using var tran = con.BeginTransaction();

            try
            {
                string? estadoActual = null;
                string? numeroDoc = null;
                using (var checkCmd = new SqlCommand("SELECT Estado, NumeroDocumento FROM Compras WITH (UPDLOCK) WHERE IdCompra = @IdCompra", con, tran))
                {
                    checkCmd.Parameters.Add(new SqlParameter("@IdCompra", idCompra));
                    using var rdr = await checkCmd.ExecuteReaderAsync();
                    if (!await rdr.ReadAsync())
                    {
                        await tran.RollbackAsync();
                        return (false, "La compra especificada no existe.");
                    }
                    estadoActual = rdr["Estado"].ToString();
                    numeroDoc = rdr["NumeroDocumento"].ToString();
                }

                if (estadoActual != "Pendiente")
                {
                    await tran.RollbackAsync();
                    return (false, "Solo se pueden recepcionar compras en estado Pendiente.");
                }

                var items = new List<(int IdProducto, int Cantidad, decimal CostoUnitario)>();
                using (var detCmd = new SqlCommand("SELECT IdProducto, Cantidad, CostoUnitario FROM DetalleCompras WHERE IdCompra = @IdCompra", con, tran))
                {
                    detCmd.Parameters.Add(new SqlParameter("@IdCompra", idCompra));
                    using var rdr = await detCmd.ExecuteReaderAsync();
                    while (await rdr.ReadAsync())
                    {
                        items.Add((Convert.ToInt32(rdr["IdProducto"]), Convert.ToInt32(rdr["Cantidad"]), Convert.ToDecimal(rdr["CostoUnitario"])));
                    }
                }

                foreach (var item in items)
                {
                    await AplicarEntradaInventarioCompraAsync(
                        con, tran, item.IdProducto, item.Cantidad, item.CostoUnitario,
                        numeroDoc!, idCompra, idUsuarioRecepcion);
                }

                using (var updCmd = new SqlCommand(@"
                    UPDATE Compras
                    SET Estado = 'Recibida', IdUsuarioRecepcion = @IdUsuarioRecepcion, FechaRecepcion = GETDATE()
                    WHERE IdCompra = @IdCompra", con, tran))
                {
                    updCmd.Parameters.Add(new SqlParameter("@IdUsuarioRecepcion", idUsuarioRecepcion));
                    updCmd.Parameters.Add(new SqlParameter("@IdCompra", idCompra));
                    await updCmd.ExecuteNonQueryAsync();
                }

                await tran.CommitAsync();
                return (true, "Recepción confirmada e inventario actualizado correctamente.");
            }
            catch (Exception ex)
            {
                await tran.RollbackAsync();
                return (false, $"Error al confirmar recepción: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> AnularCompraAsync(int idCompra, int idUsuarioAnulacion, string motivo)
        {
            using var con = _cone.GetConnection();
            await con.OpenAsync();
            using var tran = con.BeginTransaction();

            try
            {
                string? estadoActual = null;
                string? numeroDoc = null;
                using (var checkCmd = new SqlCommand("SELECT Estado, NumeroDocumento FROM Compras WITH (UPDLOCK) WHERE IdCompra = @IdCompra", con, tran))
                {
                    checkCmd.Parameters.Add(new SqlParameter("@IdCompra", idCompra));
                    using var rdr = await checkCmd.ExecuteReaderAsync();
                    if (!await rdr.ReadAsync())
                    {
                        await tran.RollbackAsync();
                        return (false, "La compra especificada no existe.");
                    }
                    estadoActual = rdr["Estado"].ToString();
                    numeroDoc = rdr["NumeroDocumento"].ToString();
                }

                if (estadoActual == "Anulada")
                {
                    await tran.RollbackAsync();
                    return (false, "La compra ya se encuentra anulada.");
                }

                if (estadoActual == "Recibida")
                {
                    var items = new List<(int IdProducto, int Cantidad, decimal CostoUnitario)>();
                    using (var detCmd = new SqlCommand("SELECT IdProducto, Cantidad, CostoUnitario FROM DetalleCompras WHERE IdCompra = @IdCompra", con, tran))
                    {
                        detCmd.Parameters.Add(new SqlParameter("@IdCompra", idCompra));
                        using var rdr = await detCmd.ExecuteReaderAsync();
                        while (await rdr.ReadAsync())
                        {
                            items.Add((Convert.ToInt32(rdr["IdProducto"]), Convert.ToInt32(rdr["Cantidad"]), Convert.ToDecimal(rdr["CostoUnitario"])));
                        }
                    }

                    foreach (var item in items)
                    {
                        int stockActual;
                        string nombreProd;
                        using (var prodCmd = new SqlCommand("SELECT Nombre, StockActual FROM Productos WITH (UPDLOCK) WHERE IdProducto = @IdProducto", con, tran))
                        {
                            prodCmd.Parameters.Add(new SqlParameter("@IdProducto", item.IdProducto));
                            using var rdr = await prodCmd.ExecuteReaderAsync();
                            if (!await rdr.ReadAsync())
                            {
                                await tran.RollbackAsync();
                                return (false, $"Producto #{item.IdProducto} no encontrado.");
                            }
                            nombreProd = rdr["Nombre"].ToString()!;
                            stockActual = Convert.ToInt32(rdr["StockActual"]);
                        }

                        if (stockActual < item.Cantidad)
                        {
                            await tran.RollbackAsync();
                            return (false, $"No hay stock suficiente para revertir \"{nombreProd}\". Stock actual: {stockActual}, requerido: {item.Cantidad}.");
                        }

                        int stockNuevo = stockActual - item.Cantidad;

                        using (var karCmd = new SqlCommand(@"
                            INSERT INTO Kardex (
                                IdProducto, FechaMovimiento, TipoMovimiento, Cantidad, StockAnterior, StockNuevo,
                                CostoUnitario, DocumentoOrigen, IdReferenciaOrigen, Justificacion, IdUsuarioResponsable
                            ) VALUES (
                                @IdProducto, GETDATE(), 'Reversion por Anulacion Compra', @Cantidad, @StockAnterior, @StockNuevo,
                                @CostoUnitario, @DocumentoOrigen, @IdReferenciaOrigen, @Justificacion, @IdUsuarioResponsable
                            )", con, tran))
                        {
                            karCmd.Parameters.Add(new SqlParameter("@IdProducto", item.IdProducto));
                            karCmd.Parameters.Add(new SqlParameter("@Cantidad", item.Cantidad));
                            karCmd.Parameters.Add(new SqlParameter("@StockAnterior", stockActual));
                            karCmd.Parameters.Add(new SqlParameter("@StockNuevo", stockNuevo));
                            karCmd.Parameters.Add(new SqlParameter("@CostoUnitario", item.CostoUnitario));
                            karCmd.Parameters.Add(new SqlParameter("@DocumentoOrigen", numeroDoc));
                            karCmd.Parameters.Add(new SqlParameter("@IdReferenciaOrigen", idCompra));
                            karCmd.Parameters.Add(new SqlParameter("@Justificacion", motivo));
                            karCmd.Parameters.Add(new SqlParameter("@IdUsuarioResponsable", idUsuarioAnulacion));
                            await karCmd.ExecuteNonQueryAsync();
                        }

                        using (var updProd = new SqlCommand("UPDATE Productos SET StockActual = StockActual - @Cantidad WHERE IdProducto = @IdProducto", con, tran))
                        {
                            updProd.Parameters.Add(new SqlParameter("@Cantidad", item.Cantidad));
                            updProd.Parameters.Add(new SqlParameter("@IdProducto", item.IdProducto));
                            await updProd.ExecuteNonQueryAsync();
                        }
                    }
                }

                using (var updCompra = new SqlCommand(@"
                    UPDATE Compras
                    SET Estado = 'Anulada', IdUsuarioAnulacion = @IdUsuario, FechaAnulacion = GETDATE(), MotivoAnulacion = @Motivo
                    WHERE IdCompra = @IdCompra", con, tran))
                {
                    updCompra.Parameters.Add(new SqlParameter("@IdUsuario", idUsuarioAnulacion));
                    updCompra.Parameters.Add(new SqlParameter("@Motivo", motivo));
                    updCompra.Parameters.Add(new SqlParameter("@IdCompra", idCompra));
                    await updCompra.ExecuteNonQueryAsync();
                }

                await tran.CommitAsync();
                return (true, "Compra anulada correctamente.");
            }
            catch (Exception ex)
            {
                await tran.RollbackAsync();
                return (false, $"Error al anular compra: {ex.Message}");
            }
        }

        private static async Task AplicarEntradaInventarioCompraAsync(
            SqlConnection con,
            SqlTransaction tran,
            int idProducto,
            int cantidad,
            decimal costoUnitario,
            string numeroDoc,
            int idCompra,
            int idUsuario)
        {
            int stockAnterior;
            decimal costoPromedioAnterior;
            using (var prodCmd = new SqlCommand("SELECT StockActual, CostoPromedio FROM Productos WITH (UPDLOCK) WHERE IdProducto = @IdProducto", con, tran))
            {
                prodCmd.Parameters.Add(new SqlParameter("@IdProducto", idProducto));
                using var rdr = await prodCmd.ExecuteReaderAsync();
                if (!await rdr.ReadAsync())
                    throw new Exception($"El producto #{idProducto} no existe.");
                stockAnterior = Convert.ToInt32(rdr["StockActual"]);
                costoPromedioAnterior = Convert.ToDecimal(rdr["CostoPromedio"]);
            }

            int stockNuevo = stockAnterior + cantidad;
            decimal nuevoCostoPromedio = stockNuevo > 0
                ? Math.Round(((stockAnterior * costoPromedioAnterior) + (cantidad * costoUnitario)) / stockNuevo, 2)
                : costoUnitario;

            using (var karCmd = new SqlCommand(@"
                INSERT INTO Kardex (
                    IdProducto, FechaMovimiento, TipoMovimiento, Cantidad, StockAnterior, StockNuevo,
                    CostoUnitario, DocumentoOrigen, IdReferenciaOrigen, Justificacion, IdUsuarioResponsable
                ) VALUES (
                    @IdProducto, GETDATE(), 'Entrada por Compra', @Cantidad, @StockAnterior, @StockNuevo,
                    @CostoUnitario, @DocumentoOrigen, @IdReferenciaOrigen, 'Recepción de compra confirmada', @IdUsuarioResponsable
                )", con, tran))
            {
                karCmd.Parameters.Add(new SqlParameter("@IdProducto", idProducto));
                karCmd.Parameters.Add(new SqlParameter("@Cantidad", cantidad));
                karCmd.Parameters.Add(new SqlParameter("@StockAnterior", stockAnterior));
                karCmd.Parameters.Add(new SqlParameter("@StockNuevo", stockNuevo));
                karCmd.Parameters.Add(new SqlParameter("@CostoUnitario", costoUnitario));
                karCmd.Parameters.Add(new SqlParameter("@DocumentoOrigen", numeroDoc));
                karCmd.Parameters.Add(new SqlParameter("@IdReferenciaOrigen", idCompra));
                karCmd.Parameters.Add(new SqlParameter("@IdUsuarioResponsable", idUsuario));
                await karCmd.ExecuteNonQueryAsync();
            }

            using (var updProd = new SqlCommand(@"
                UPDATE Productos
                SET StockActual = @StockNuevo,
                    UltimoCostoCompra = @CostoUnitario,
                    CostoPromedio = @CostoPromedio
                WHERE IdProducto = @IdProducto", con, tran))
            {
                updProd.Parameters.Add(new SqlParameter("@StockNuevo", stockNuevo));
                updProd.Parameters.Add(new SqlParameter("@CostoUnitario", costoUnitario));
                updProd.Parameters.Add(new SqlParameter("@CostoPromedio", nuevoCostoPromedio));
                updProd.Parameters.Add(new SqlParameter("@IdProducto", idProducto));
                await updProd.ExecuteNonQueryAsync();
            }
        }
    }
}
