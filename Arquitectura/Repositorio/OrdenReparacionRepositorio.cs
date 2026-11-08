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
    public class OrdenReparacionRepositorio : IOrdenReparacion
    {
        private readonly BD _cone;

        public OrdenReparacionRepositorio(BD cone)
        {
            _cone = cone;
        }

        public async Task EditarOrdenReparacionAsync(OrdenReparacion orden)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("ActualizarOrdenReparacion", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdOrdenReparacion", orden.IdOrdenReparacion));
                cmd.Parameters.Add(new SqlParameter("@NumeroOrden", orden.NumeroOrden));
                cmd.Parameters.Add(new SqlParameter("@IdCliente", orden.IdCliente));
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioTecnico", orden.IdUsuarioTecnico));
                cmd.Parameters.Add(new SqlParameter("@DescripcionEquipo", orden.DescripcionEquipo));
                cmd.Parameters.Add(new SqlParameter("@Marca", orden.Marca));
                cmd.Parameters.Add(new SqlParameter("@Modelo", orden.Modelo));
                cmd.Parameters.Add(new SqlParameter("@NumeroSerie", orden.NumeroSerie));
                cmd.Parameters.Add(new SqlParameter("@EstadoEquipo", orden.EstadoEquipo));
                cmd.Parameters.Add(new SqlParameter("@CostoReparacion", orden.CostoReparacion));
                cmd.Parameters.Add(new SqlParameter("@FechaIngreso", orden.FechaIngreso));
                cmd.Parameters.Add(new SqlParameter("@IdGarantia", (object?)orden.IdGarantia ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@DiagnosticoTecnico", (object?)orden.DiagnosticoTecnico ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@FechaFinalizacion", (object?)orden.FechaFinalizacion ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@FechaEntrega", (object?)orden.FechaEntrega ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@Observaciones", (object?)orden.Observaciones ?? DBNull.Value));

                await cmd.ExecuteReaderAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task EliminarOrdenReparacionAsync(int Id)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("EliminarOrdenReparacion", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdOrdenReparacion", Id));

                await cmd.ExecuteReaderAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<OrdenReparacion>> ListarOrdenReparacionAsync()
        {
            var list = new List<OrdenReparacion>();
            using var con = _cone.GetConnection();
            await con.OpenAsync();

            try
            {
                using var cmd = new SqlCommand("ListarOrdenesReparacion", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using var dr = await cmd.ExecuteReaderAsync();

                while (await dr.ReadAsync())
                {
                    var o = new OrdenReparacion
                    {
                        IdOrdenReparacion = Convert.ToInt32(dr["IdOrdenReparacion"]),
                        NumeroOrden = dr["NumeroOrden"].ToString()!,
                        IdGarantia = dr["IdGarantia"] == DBNull.Value ? null : Convert.ToInt32(dr["IdGarantia"]),
                        IdCliente = Convert.ToInt32(dr["IdCliente"]),
                        IdUsuarioTecnico = Convert.ToInt32(dr["IdUsuarioTecnico"]),
                        DescripcionEquipo = dr["DescripcionEquipo"].ToString()!,
                        Marca = dr["Marca"].ToString()!,
                        Modelo = dr["Modelo"].ToString()!,
                        NumeroSerie = dr["NumeroSerie"].ToString()!,
                        DiagnosticoTecnico = dr["DiagnosticoTecnico"] == DBNull.Value ? null : dr["DiagnosticoTecnico"].ToString(),
                        EstadoEquipo = dr["EstadoEquipo"].ToString()!,
                        CostoReparacion = Convert.ToDecimal(dr["CostoReparacion"]),
                        FechaIngreso = Convert.ToDateTime(dr["FechaIngreso"]),
                        FechaFinalizacion = dr["FechaFinalizacion"] == DBNull.Value ? null : Convert.ToDateTime(dr["FechaFinalizacion"]),
                        FechaEntrega = dr["FechaEntrega"] == DBNull.Value ? null : Convert.ToDateTime(dr["FechaEntrega"]),
                        Observaciones = dr["Observaciones"] == DBNull.Value ? null : dr["Observaciones"].ToString()
                    };
                    list.Add(o);
                }
                return list;
            }
            catch
            {
                throw;
            }
        }

        public async Task NuevoOrdenReparacionAsync(OrdenReparacion orden)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("InsertarOrdenReparacion", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@NumeroOrden", orden.NumeroOrden));
                cmd.Parameters.Add(new SqlParameter("@IdCliente", orden.IdCliente));
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioTecnico", orden.IdUsuarioTecnico));
                cmd.Parameters.Add(new SqlParameter("@DescripcionEquipo", orden.DescripcionEquipo));
                cmd.Parameters.Add(new SqlParameter("@Marca", orden.Marca));
                cmd.Parameters.Add(new SqlParameter("@Modelo", orden.Modelo));
                cmd.Parameters.Add(new SqlParameter("@NumeroSerie", orden.NumeroSerie));
                cmd.Parameters.Add(new SqlParameter("@IdGarantia", (object?)orden.IdGarantia ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@DiagnosticoTecnico", (object?)orden.DiagnosticoTecnico ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@EstadoEquipo", orden.EstadoEquipo));
                cmd.Parameters.Add(new SqlParameter("@CostoReparacion", orden.CostoReparacion));
                cmd.Parameters.Add(new SqlParameter("@FechaIngreso", (object?)orden.FechaIngreso == null || orden.FechaIngreso == default ? DBNull.Value : orden.FechaIngreso));
                cmd.Parameters.Add(new SqlParameter("@FechaFinalizacion", (object?)orden.FechaFinalizacion ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@FechaEntrega", (object?)orden.FechaEntrega ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@Observaciones", (object?)orden.Observaciones ?? DBNull.Value));

                await cmd.ExecuteReaderAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
