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
                cmd.Parameters.Add(new SqlParameter("@DocumentoOrigen", kardex.DocumentoOrigen == null ? DBNull.Value : kardex.DocumentoOrigen));
                cmd.Parameters.Add(new SqlParameter("@IdReferenciaOrigen", kardex.IdReferenciaOrigen == null ? DBNull.Value : kardex.IdReferenciaOrigen));
                cmd.Parameters.Add(new SqlParameter("@Justificacion", kardex.Justificacion == null ? DBNull.Value : kardex.Justificacion));

                await cmd.ExecuteNonQueryAsync();
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

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Kardex>> ListarKardexAsync()
        {
            var list = new List<Kardex>();
            using var con = _cone.GetConnection();
            await con.OpenAsync();

            try
            {
                using var cmd = new SqlCommand("ListarKardex", con);
                cmd.CommandType = CommandType.StoredProcedure;

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
                cmd.Parameters.Add(new SqlParameter("@FechaMovimiento", kardex.FechaMovimiento));
                cmd.Parameters.Add(new SqlParameter("@CostoUnitario", kardex.CostoUnitario));
                cmd.Parameters.Add(new SqlParameter("@DocumentoOrigen", kardex.DocumentoOrigen == null ? DBNull.Value : kardex.DocumentoOrigen));
                cmd.Parameters.Add(new SqlParameter("@IdReferenciaOrigen", kardex.IdReferenciaOrigen == null ? DBNull.Value : kardex.IdReferenciaOrigen));
                cmd.Parameters.Add(new SqlParameter("@Justificacion", kardex.Justificacion == null ? DBNull.Value : kardex.Justificacion));

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<(bool Success, string Message)> RegistrarAjusteInventarioAsync(
            int idProducto,
            string tipoMovimiento,
            int cantidad,
            string justificacion,
            int idUsuario)
        {
            if (cantidad <= 0)
                return (false, "La cantidad debe ser mayor a cero.");

            if (tipoMovimiento != "Ajuste de Entrada" && tipoMovimiento != "Ajuste de Salida" && tipoMovimiento != "Baja de Inventario")
                return (false, "Tipo de movimiento no permitido para ajuste manual.");

            using var con = _cone.GetConnection();
            await con.OpenAsync();
            using var tran = con.BeginTransaction();

            try
            {
                int stockAnterior;
                decimal ultimoCosto;
                string nombreProd;

                using (var cmd = new SqlCommand("SELECT Nombre, StockActual, UltimoCostoCompra FROM Productos WITH (UPDLOCK) WHERE IdProducto = @IdProducto", con, tran))
                {
                    cmd.Parameters.Add(new SqlParameter("@IdProducto", idProducto));
                    using var rdr = await cmd.ExecuteReaderAsync();
                    if (!await rdr.ReadAsync())
                    {
                        await tran.RollbackAsync();
                        return (false, "El producto seleccionado no existe.");
                    }
                    nombreProd = rdr["Nombre"].ToString()!;
                    stockAnterior = Convert.ToInt32(rdr["StockActual"]);
                    ultimoCosto = Convert.ToDecimal(rdr["UltimoCostoCompra"]);
                }

                bool esEntrada = tipoMovimiento == "Ajuste de Entrada";
                if (!esEntrada && stockAnterior < cantidad)
                {
                    await tran.RollbackAsync();
                    return (false, $"Stock insuficiente en \"{nombreProd}\". Disponible: {stockAnterior}, solicitado: {cantidad}.");
                }

                int stockNuevo = esEntrada ? stockAnterior + cantidad : stockAnterior - cantidad;

                using (var karCmd = new SqlCommand(@"
                    INSERT INTO Kardex (
                        IdProducto, FechaMovimiento, TipoMovimiento, Cantidad, StockAnterior, StockNuevo,
                        CostoUnitario, DocumentoOrigen, Justificacion, IdUsuarioResponsable
                    ) VALUES (
                        @IdProducto, GETDATE(), @TipoMovimiento, @Cantidad, @StockAnterior, @StockNuevo,
                        @CostoUnitario, @DocumentoOrigen, @Justificacion, @IdUsuarioResponsable
                    )", con, tran))
                {
                    karCmd.Parameters.Add(new SqlParameter("@IdProducto", idProducto));
                    karCmd.Parameters.Add(new SqlParameter("@TipoMovimiento", tipoMovimiento));
                    karCmd.Parameters.Add(new SqlParameter("@Cantidad", cantidad));
                    karCmd.Parameters.Add(new SqlParameter("@StockAnterior", stockAnterior));
                    karCmd.Parameters.Add(new SqlParameter("@StockNuevo", stockNuevo));
                    karCmd.Parameters.Add(new SqlParameter("@CostoUnitario", ultimoCosto));
                    karCmd.Parameters.Add(new SqlParameter("@DocumentoOrigen", $"AJU-{DateTime.Now:yyyyMMddHHmm}"));
                    karCmd.Parameters.Add(new SqlParameter("@Justificacion", justificacion));
                    karCmd.Parameters.Add(new SqlParameter("@IdUsuarioResponsable", idUsuario));
                    await karCmd.ExecuteNonQueryAsync();
                }

                using (var updProd = new SqlCommand("UPDATE Productos SET StockActual = @StockNuevo WHERE IdProducto = @IdProducto", con, tran))
                {
                    updProd.Parameters.Add(new SqlParameter("@StockNuevo", stockNuevo));
                    updProd.Parameters.Add(new SqlParameter("@IdProducto", idProducto));
                    await updProd.ExecuteNonQueryAsync();
                }

                await tran.CommitAsync();
                return (true, $"Ajuste aplicado en \"{nombreProd}\". Nuevo stock: {stockNuevo} unidades.");
            }
            catch (Exception ex)
            {
                await tran.RollbackAsync();
                return (false, $"Error al registrar ajuste: {ex.Message}");
            }
        }
    }
}
