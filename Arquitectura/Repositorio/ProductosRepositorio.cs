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
    public class ProductosRepositorio : IProductos
    {
        private readonly BD _cone;

        public ProductosRepositorio(BD cone)
        {
            _cone = cone;
        }

        public async Task EditarProductoAsync(Productos producto)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("ActualizarProducto", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdProducto", producto.IdProducto));
                cmd.Parameters.Add(new SqlParameter("@CodigoSKU", producto.CodigoSKU));
                cmd.Parameters.Add(new SqlParameter("@Nombre", producto.Nombre));
                cmd.Parameters.Add(new SqlParameter("@Marca", producto.Marca));
                cmd.Parameters.Add(new SqlParameter("@Modelo", producto.Modelo));
                cmd.Parameters.Add(new SqlParameter("@TipoEquipo", producto.TipoEquipo));
                cmd.Parameters.Add(new SqlParameter("@PrecioVenta", producto.PrecioVenta));
                cmd.Parameters.Add(new SqlParameter("@UltimoCostoCompra", producto.UltimoCostoCompra));
                cmd.Parameters.Add(new SqlParameter("@CostoPromedio", producto.CostoPromedio));
                cmd.Parameters.Add(new SqlParameter("@StockActual", producto.StockActual));
                cmd.Parameters.Add(new SqlParameter("@StockMinimo", producto.StockMinimo));
                cmd.Parameters.Add(new SqlParameter("@MesesGarantia", producto.MesesGarantia));
                cmd.Parameters.Add(new SqlParameter("@RequiereSerie", producto.RequiereSerie));
                cmd.Parameters.Add(new SqlParameter("@Activo", producto.Activo));
                cmd.Parameters.Add(new SqlParameter("@Descripcion", (object?)producto.Descripcion ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@IdProveedorPreferido", (object?)producto.IdProveedorPreferido ?? DBNull.Value));

                await cmd.ExecuteReaderAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task EliminarProductoAsync(int Id)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("EliminarProducto", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdProducto", Id));

                await cmd.ExecuteReaderAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Productos>> ListarProductoAsync()
        {
            var list = new List<Productos>();
            using var con = _cone.GetConnection();
            await con.OpenAsync();

            try
            {
                using var cmd = new SqlCommand("ListarProductos", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using var dr = await cmd.ExecuteReaderAsync();

                while (await dr.ReadAsync())
                {
                    var p = new Productos
                    {
                        IdProducto = Convert.ToInt32(dr["IdProducto"]),
                        CodigoSKU = dr["CodigoSKU"].ToString()!,
                        Nombre = dr["Nombre"].ToString()!,
                        Marca = dr["Marca"].ToString()!,
                        Modelo = dr["Modelo"].ToString()!,
                        TipoEquipo = dr["TipoEquipo"].ToString()!,
                        Descripcion = dr["Descripcion"] == DBNull.Value ? null : dr["Descripcion"].ToString(),
                        PrecioVenta = Convert.ToDecimal(dr["PrecioVenta"]),
                        UltimoCostoCompra = Convert.ToDecimal(dr["UltimoCostoCompra"]),
                        CostoPromedio = Convert.ToDecimal(dr["CostoPromedio"]),
                        StockActual = Convert.ToInt32(dr["StockActual"]),
                        StockMinimo = Convert.ToInt32(dr["StockMinimo"]),
                        MesesGarantia = Convert.ToInt32(dr["MesesGarantia"]),
                        RequiereSerie = Convert.ToBoolean(dr["RequiereSerie"]),
                        IdProveedorPreferido = dr["IdProveedorPreferido"] == DBNull.Value ? null : Convert.ToInt32(dr["IdProveedorPreferido"]),
                        Activo = Convert.ToBoolean(dr["Activo"]),
                        FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"])
                    };
                    list.Add(p);
                }
                return list;
            }
            catch
            {
                throw;
            }
        }

        public async Task NuevoProductoAsync(Productos producto)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("InsertarProducto", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CodigoSKU", producto.CodigoSKU));
                cmd.Parameters.Add(new SqlParameter("@Nombre", producto.Nombre));
                cmd.Parameters.Add(new SqlParameter("@Marca", producto.Marca));
                cmd.Parameters.Add(new SqlParameter("@Modelo", producto.Modelo));
                cmd.Parameters.Add(new SqlParameter("@TipoEquipo", producto.TipoEquipo));
                cmd.Parameters.Add(new SqlParameter("@PrecioVenta", producto.PrecioVenta));
                cmd.Parameters.Add(new SqlParameter("@Descripcion", (object?)producto.Descripcion ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@UltimoCostoCompra", producto.UltimoCostoCompra));
                cmd.Parameters.Add(new SqlParameter("@CostoPromedio", producto.CostoPromedio));
                cmd.Parameters.Add(new SqlParameter("@StockActual", producto.StockActual));
                cmd.Parameters.Add(new SqlParameter("@StockMinimo", producto.StockMinimo));
                cmd.Parameters.Add(new SqlParameter("@MesesGarantia", producto.MesesGarantia));
                cmd.Parameters.Add(new SqlParameter("@RequiereSerie", producto.RequiereSerie));
                cmd.Parameters.Add(new SqlParameter("@IdProveedorPreferido", (object?)producto.IdProveedorPreferido ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@Activo", producto.Activo));

                await cmd.ExecuteReaderAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
