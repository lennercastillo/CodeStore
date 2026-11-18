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
    public class AbonoRepositorio : IAbono
    {
        private readonly BD _cone;

        public AbonoRepositorio(BD cone)
        {
            _cone = cone;
        }

        public async Task EditarAbonoAsync(Abono abono)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("ActualizarAbono", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdAbono", abono.IdAbono));
                cmd.Parameters.Add(new SqlParameter("@NumeroRecibo", abono.NumeroRecibo));
                cmd.Parameters.Add(new SqlParameter("@IdCuota", abono.IdCuota));
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioRegistro", abono.IdUsuarioRegistro));
                cmd.Parameters.Add(new SqlParameter("@FechaAbono", abono.FechaAbono));
                cmd.Parameters.Add(new SqlParameter("@MontoAbono", abono.MontoAbono));
                cmd.Parameters.Add(new SqlParameter("@MontoAplicadoMora", abono.MontoAplicadoMora));
                cmd.Parameters.Add(new SqlParameter("@MontoAplicadoCapital", abono.MontoAplicadoCapital));
                cmd.Parameters.Add(new SqlParameter("@MetodoPago", abono.MetodoPago));
                cmd.Parameters.Add(new SqlParameter("@Estado", abono.Estado));
                cmd.Parameters.Add(new SqlParameter("@Observaciones", abono.Observaciones == null ? DBNull.Value : abono.Observaciones));
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioAnulacion", abono.IdUsuarioAnulacion == null ? DBNull.Value : abono.IdUsuarioAnulacion));
                cmd.Parameters.Add(new SqlParameter("@FechaAnulacion", abono.FechaAnulacion == null ? DBNull.Value : abono.FechaAnulacion));
                cmd.Parameters.Add(new SqlParameter("@MotivoAnulacion", abono.MotivoAnulacion == null ? DBNull.Value : abono.MotivoAnulacion));

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task EliminarAbonoAsync(int Id)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("EliminarAbono", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdAbono", Id));

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Abono>> ListarAbonoAsync()
        {
            var list = new List<Abono>();
            using var con = _cone.GetConnection();
            await con.OpenAsync();

            try
            {
                using var cmd = new SqlCommand("ListarAbonos", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using var dr = await cmd.ExecuteReaderAsync();

                while (await dr.ReadAsync())
                {
                    var a = new Abono
                    {
                        IdAbono = Convert.ToInt32(dr["IdAbono"]),
                        NumeroRecibo = dr["NumeroRecibo"].ToString()!,
                        IdCuota = Convert.ToInt32(dr["IdCuota"]),
                        IdUsuarioRegistro = Convert.ToInt32(dr["IdUsuarioRegistro"]),
                        FechaAbono = Convert.ToDateTime(dr["FechaAbono"]),
                        MontoAbono = Convert.ToDecimal(dr["MontoAbono"]),
                        MontoAplicadoMora = Convert.ToDecimal(dr["MontoAplicadoMora"]),
                        MontoAplicadoCapital = Convert.ToDecimal(dr["MontoAplicadoCapital"]),
                        MetodoPago = dr["MetodoPago"].ToString()!,
                        Observaciones = dr["Observaciones"] == DBNull.Value ? null : dr["Observaciones"].ToString(),
                        Estado = dr["Estado"].ToString()!,
                        IdUsuarioAnulacion = dr["IdUsuarioAnulacion"] == DBNull.Value ? null : Convert.ToInt32(dr["IdUsuarioAnulacion"]),
                        FechaAnulacion = dr["FechaAnulacion"] == DBNull.Value ? null : Convert.ToDateTime(dr["FechaAnulacion"]),
                        MotivoAnulacion = dr["MotivoAnulacion"] == DBNull.Value ? null : dr["MotivoAnulacion"].ToString()
                    };
                    list.Add(a);
                }
                return list;
            }
            catch
            {
                throw;
            }
        }

        public async Task NuevoAbonoAsync(Abono abono)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("InsertarAbono", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@NumeroRecibo", abono.NumeroRecibo));
                cmd.Parameters.Add(new SqlParameter("@IdCuota", abono.IdCuota));
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioRegistro", abono.IdUsuarioRegistro));
                cmd.Parameters.Add(new SqlParameter("@MontoAbono", abono.MontoAbono));
                cmd.Parameters.Add(new SqlParameter("@MetodoPago", abono.MetodoPago));
                cmd.Parameters.Add(new SqlParameter("@FechaAbono", abono.FechaAbono));
                cmd.Parameters.Add(new SqlParameter("@MontoAplicadoMora", abono.MontoAplicadoMora));
                cmd.Parameters.Add(new SqlParameter("@MontoAplicadoCapital", abono.MontoAplicadoCapital));
                cmd.Parameters.Add(new SqlParameter("@Observaciones", abono.Observaciones == null ? DBNull.Value : abono.Observaciones));
                cmd.Parameters.Add(new SqlParameter("@Estado", abono.Estado));
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioAnulacion", abono.IdUsuarioAnulacion == null ? DBNull.Value : abono.IdUsuarioAnulacion));
                cmd.Parameters.Add(new SqlParameter("@FechaAnulacion", abono.FechaAnulacion == null ? DBNull.Value : abono.FechaAnulacion));
                cmd.Parameters.Add(new SqlParameter("@MotivoAnulacion", abono.MotivoAnulacion == null ? DBNull.Value : abono.MotivoAnulacion));

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<(bool Success, string Message, int IdAbono, string NumeroRecibo)> RegistrarAbonoTransaccionalAsync(
            int idCuota,
            decimal montoAbono,
            string metodoPago,
            string? observaciones,
            int idUsuarioRegistro)
        {
            if (montoAbono <= 0)
                return (false, "El monto del abono debe ser mayor a cero.", 0, string.Empty);

            using var con = _cone.GetConnection();
            await con.OpenAsync();
            using var tran = con.BeginTransaction();

            try
            {
                int idCredito;
                decimal montoCuota, recargoMora, moraAbonada, capitalAbonado, montoAbonadoActual, saldoCuota;
                string estadoCuota;

                using (var cmd = new SqlCommand(@"
                    SELECT IdCredito, MontoCuota, RecargoMora, MoraAbonada, CapitalInteresAbonado, MontoAbonado, SaldoCuota, Estado
                    FROM CuotasCredito WITH (UPDLOCK)
                    WHERE IdCuota = @IdCuota", con, tran))
                {
                    cmd.Parameters.Add(new SqlParameter("@IdCuota", idCuota));
                    using var rdr = await cmd.ExecuteReaderAsync();
                    if (!await rdr.ReadAsync())
                    {
                        await tran.RollbackAsync();
                        return (false, "La cuota seleccionada no existe.", 0, string.Empty);
                    }

                    idCredito = Convert.ToInt32(rdr["IdCredito"]);
                    montoCuota = Convert.ToDecimal(rdr["MontoCuota"]);
                    recargoMora = Convert.ToDecimal(rdr["RecargoMora"]);
                    moraAbonada = Convert.ToDecimal(rdr["MoraAbonada"]);
                    capitalAbonado = Convert.ToDecimal(rdr["CapitalInteresAbonado"]);
                    montoAbonadoActual = Convert.ToDecimal(rdr["MontoAbonado"]);
                    saldoCuota = Convert.ToDecimal(rdr["SaldoCuota"]);
                    estadoCuota = rdr["Estado"].ToString()!;
                }

                if (estadoCuota == "Pagada" || estadoCuota == "Anulada")
                {
                    await tran.RollbackAsync();
                    return (false, $"La cuota ya se encuentra en estado {estadoCuota}.", 0, string.Empty);
                }

                if (montoAbono > saldoCuota)
                {
                    await tran.RollbackAsync();
                    return (false, $"El abono (C$ {montoAbono:N2}) supera el saldo pendiente de la cuota (C$ {saldoCuota:N2}).", 0, string.Empty);
                }

                decimal moraPendiente = Math.Max(0, recargoMora - moraAbonada);
                decimal aplicadoMora = Math.Min(montoAbono, moraPendiente);
                decimal aplicadoCapital = montoAbono - aplicadoMora;

                string numeroRecibo;
                using (var cntCmd = new SqlCommand("SELECT COUNT(1) + 1 FROM Abonos", con, tran))
                {
                    var count = Convert.ToInt32(await cntCmd.ExecuteScalarAsync());
                    numeroRecibo = $"REC-{DateTime.Now:yyyyMMdd}-{count:D4}";
                }

                int idAbonoGenerado;
                using (var insCmd = new SqlCommand(@"
                    INSERT INTO Abonos (
                        NumeroRecibo, IdCuota, IdUsuarioRegistro, FechaAbono, MontoAbono,
                        MontoAplicadoMora, MontoAplicadoCapital, MetodoPago, Observaciones, Estado
                    ) VALUES (
                        @NumeroRecibo, @IdCuota, @IdUsuarioRegistro, GETDATE(), @MontoAbono,
                        @MontoAplicadoMora, @MontoAplicadoCapital, @MetodoPago, @Observaciones, 'Activo'
                    );
                    SELECT CAST(SCOPE_IDENTITY() AS INT);", con, tran))
                {
                    insCmd.Parameters.Add(new SqlParameter("@NumeroRecibo", numeroRecibo));
                    insCmd.Parameters.Add(new SqlParameter("@IdCuota", idCuota));
                    insCmd.Parameters.Add(new SqlParameter("@IdUsuarioRegistro", idUsuarioRegistro));
                    insCmd.Parameters.Add(new SqlParameter("@MontoAbono", montoAbono));
                    insCmd.Parameters.Add(new SqlParameter("@MontoAplicadoMora", aplicadoMora));
                    insCmd.Parameters.Add(new SqlParameter("@MontoAplicadoCapital", aplicadoCapital));
                    insCmd.Parameters.Add(new SqlParameter("@MetodoPago", metodoPago));
                    insCmd.Parameters.Add(new SqlParameter("@Observaciones", (object?)observaciones ?? DBNull.Value));
                    idAbonoGenerado = (int)(await insCmd.ExecuteScalarAsync())!;
                }

                decimal nuevoTotalAbonado = montoAbonadoActual + montoAbono;
                string nuevoEstadoCuota = nuevoTotalAbonado >= (montoCuota + recargoMora) ? "Pagada" : "Parcial";

                using (var updCuota = new SqlCommand(@"
                    UPDATE CuotasCredito
                    SET MoraAbonada = MoraAbonada + @AplicadoMora,
                        CapitalInteresAbonado = CapitalInteresAbonado + @AplicadoCapital,
                        Estado = @Estado
                    WHERE IdCuota = @IdCuota", con, tran))
                {
                    updCuota.Parameters.Add(new SqlParameter("@AplicadoMora", aplicadoMora));
                    updCuota.Parameters.Add(new SqlParameter("@AplicadoCapital", aplicadoCapital));
                    updCuota.Parameters.Add(new SqlParameter("@Estado", nuevoEstadoCuota));
                    updCuota.Parameters.Add(new SqlParameter("@IdCuota", idCuota));
                    await updCuota.ExecuteNonQueryAsync();
                }

                using (var updCredito = new SqlCommand(@"
                    UPDATE Creditos
                    SET SaldoPendiente = CASE WHEN (SaldoPendiente - @AplicadoCapital) <= 0 THEN 0 ELSE SaldoPendiente - @AplicadoCapital END,
                        Estado = CASE WHEN (SaldoPendiente - @AplicadoCapital) <= 0 THEN 'Pagado' ELSE Estado END
                    WHERE IdCredito = @IdCredito", con, tran))
                {
                    updCredito.Parameters.Add(new SqlParameter("@AplicadoCapital", aplicadoCapital));
                    updCredito.Parameters.Add(new SqlParameter("@IdCredito", idCredito));
                    await updCredito.ExecuteNonQueryAsync();
                }

                await tran.CommitAsync();
                return (true, $"Abono registrado con éxito. Recibo {numeroRecibo}.", idAbonoGenerado, numeroRecibo);
            }
            catch (Exception ex)
            {
                await tran.RollbackAsync();
                return (false, $"Error al registrar abono: {ex.Message}", 0, string.Empty);
            }
        }
    }
}
