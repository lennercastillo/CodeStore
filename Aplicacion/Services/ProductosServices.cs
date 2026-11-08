using Aplicacion.DTOs;
using Aplicacion.Interface;
using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Services
{
    public class ProductosServices
    {
        private readonly IProductos _repo;

        public ProductosServices(IProductos reposi)
        {
            _repo = reposi;
        }

        public async Task<IEnumerable<DTOSProductos>> ListarProducto()
        {
            var Lista = await _repo.ListarProductoAsync();

            return Lista.Select(li => new DTOSProductos
            {
                IdProducto = li.IdProducto,
                CodigoSKU = li.CodigoSKU,
                Nombre = li.Nombre,
                Marca = li.Marca,
                Modelo = li.Modelo,
                TipoEquipo = li.TipoEquipo,
                Descripcion = li.Descripcion,
                PrecioVenta = li.PrecioVenta,
                UltimoCostoCompra = li.UltimoCostoCompra,
                CostoPromedio = li.CostoPromedio,
                StockActual = li.StockActual,
                StockMinimo = li.StockMinimo,
                MesesGarantia = li.MesesGarantia,
                RequiereSerie = li.RequiereSerie,
                IdProveedorPreferido = li.IdProveedorPreferido,
                Activo = li.Activo,
                FechaRegistro = li.FechaRegistro
            });
        }

        public async Task NuevoProducto(DTOSProductos dt)
        {
            var np = new Productos
            {
                IdProducto = dt.IdProducto,
                CodigoSKU = dt.CodigoSKU,
                Nombre = dt.Nombre,
                Marca = dt.Marca,
                Modelo = dt.Modelo,
                TipoEquipo = dt.TipoEquipo,
                Descripcion = dt.Descripcion,
                PrecioVenta = dt.PrecioVenta,
                UltimoCostoCompra = dt.UltimoCostoCompra,
                CostoPromedio = dt.CostoPromedio,
                StockActual = dt.StockActual,
                StockMinimo = dt.StockMinimo,
                MesesGarantia = dt.MesesGarantia,
                RequiereSerie = dt.RequiereSerie,
                IdProveedorPreferido = dt.IdProveedorPreferido,
                Activo = dt.Activo,
                FechaRegistro = dt.FechaRegistro
            };
            await _repo.NuevoProductoAsync(np);
        }

        public async Task EditarProducto(DTOSProductos dt)
        {
            var ep = new Productos
            {
                IdProducto = dt.IdProducto,
                CodigoSKU = dt.CodigoSKU,
                Nombre = dt.Nombre,
                Marca = dt.Marca,
                Modelo = dt.Modelo,
                TipoEquipo = dt.TipoEquipo,
                Descripcion = dt.Descripcion,
                PrecioVenta = dt.PrecioVenta,
                UltimoCostoCompra = dt.UltimoCostoCompra,
                CostoPromedio = dt.CostoPromedio,
                StockActual = dt.StockActual,
                StockMinimo = dt.StockMinimo,
                MesesGarantia = dt.MesesGarantia,
                RequiereSerie = dt.RequiereSerie,
                IdProveedorPreferido = dt.IdProveedorPreferido,
                Activo = dt.Activo,
                FechaRegistro = dt.FechaRegistro
            };
            await _repo.EditarProductoAsync(ep);
        }

        public async Task EliminarProducto(int id)
        {
            await _repo.EliminarProductoAsync(id);
        }
    }
}
