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
    public class DetalleCompraRepositorio : IDetalleCompra
    {
        private readonly BD _cone;

        public DetalleCompraRepositorio(BD cone)
        {
            _cone = cone;
        }

        public async Task EditarDetalleCompraAsync(DetalleCompra detalle)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("ActualizarDetalleCompra", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdDetalleCompra", detalle.IdDetalleCompra));
                cmd.Parameters.Add(new SqlParameter("@IdCompra", detalle.IdCompra));
                cmd.Parameters.Add(new SqlParameter("@IdProducto", detalle.IdProducto));
                cmd.Parameters.Add(new SqlParameter("@Cantidad", detalle.Cantidad));
                cmd.Parameters.Add(new SqlParameter("@CostoUnitario", detalle.CostoUnitario));

                await cmd.ExecuteReaderAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task EliminarDetalleCompraAsync(int Id)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("EliminarDetalleCompra", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdDetalleCompra", Id));

                await cmd.ExecuteReaderAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<DetalleCompra>> ListarDetalleCompraAsync()
        {
            var list = new List<DetalleCompra>();
            using var con = _cone.GetConnection();
            await con.OpenAsync();

            try
            {
                using var cmd = new SqlCommand("ListarDetalleCompras", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using var dr = await cmd.ExecuteReaderAsync();

                while (await dr.ReadAsync())
                {
                    var dc = new DetalleCompra
                    {
                        IdDetalleCompra = Convert.ToInt32(dr["IdDetalleCompra"]),
                        IdCompra = Convert.ToInt32(dr["IdCompra"]),
                        IdProducto = Convert.ToInt32(dr["IdProducto"]),
                        Cantidad = Convert.ToInt32(dr["Cantidad"]),
                        CostoUnitario = Convert.ToDecimal(dr["CostoUnitario"]),
                        Subtotal = Convert.ToDecimal(dr["Subtotal"])
                    };
                    list.Add(dc);
                }
                return list;
            }
            catch
            {
                throw;
            }
        }

        public async Task NuevoDetalleCompraAsync(DetalleCompra detalle)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("InsertarDetalleCompra", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdCompra", detalle.IdCompra));
                cmd.Parameters.Add(new SqlParameter("@IdProducto", detalle.IdProducto));
                cmd.Parameters.Add(new SqlParameter("@Cantidad", detalle.Cantidad));
                cmd.Parameters.Add(new SqlParameter("@CostoUnitario", detalle.CostoUnitario));

                await cmd.ExecuteReaderAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
