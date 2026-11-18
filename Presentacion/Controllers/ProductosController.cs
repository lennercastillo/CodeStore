using Aplicacion.DTOs;
using Aplicacion.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentacion.Helpers;

namespace Presentacion.Controllers
{
    [Authorize]
    public class ProductosController : Controller
    {
        private readonly ProductosServices _productosService;
        private readonly ProveedorServices _proveedorService;

        public ProductosController(ProductosServices productosService, ProveedorServices proveedorService)
        {
            _productosService = productosService;
            _proveedorService = proveedorService;
        }

        // GET: /Productos
        public async Task<IActionResult> Index()
        {
            try
            {
                var productos = await _productosService.ListarProducto();
                return View(productos);
            }
            catch (Exception ex)
            {
                TempData["Error"] = MensajeErrorHelper.ObtenerMensajeAmigable(ex, "cargar los productos");
                return View(Enumerable.Empty<DTOSProductos>());
            }
        }

        // GET: /Productos/Crear
        public async Task<IActionResult> Crear()
        {
            try
            {
                ViewBag.Proveedores = await _proveedorService.ListarProveedor();
            }
            catch
            {
                ViewBag.Proveedores = Enumerable.Empty<DTOProveedor>();
            }

            var nuevo = new DTOSProductos
            {
                CodigoSKU = string.Empty,
                Nombre = string.Empty,
                Marca = string.Empty,
                Modelo = string.Empty,
                TipoEquipo = "Laptop",
                PrecioVenta = 0,
                StockMinimo = 1,
                Activo = true
            };

            return View(nuevo);
        }

        // POST: /Productos/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(DTOSProductos model)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    ViewBag.Proveedores = await _proveedorService.ListarProveedor();
                }
                catch
                {
                    ViewBag.Proveedores = Enumerable.Empty<DTOProveedor>();
                }
                return View(model);
            }

            try
            {
                await _productosService.NuevoProducto(model);
                TempData["Success"] = $"El producto \"{model.Nombre}\" ha sido registrado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = MensajeErrorHelper.ObtenerMensajeAmigable(ex, "registrar el producto");
                try
                {
                    ViewBag.Proveedores = await _proveedorService.ListarProveedor();
                }
                catch
                {
                    ViewBag.Proveedores = Enumerable.Empty<DTOProveedor>();
                }
                return View(model);
            }
        }

        // GET: /Productos/Editar/5
        public async Task<IActionResult> Editar(int id)
        {
            var producto = await _productosService.ObtenerPorId(id);
            if (producto == null)
            {
                TempData["Error"] = "El producto solicitado no fue encontrado.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                ViewBag.Proveedores = await _proveedorService.ListarProveedor();
            }
            catch
            {
                ViewBag.Proveedores = Enumerable.Empty<DTOProveedor>();
            }

            return View(producto);
        }

        // POST: /Productos/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(DTOSProductos model)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    ViewBag.Proveedores = await _proveedorService.ListarProveedor();
                }
                catch
                {
                    ViewBag.Proveedores = Enumerable.Empty<DTOProveedor>();
                }
                return View(model);
            }

            try
            {
                await _productosService.EditarProducto(model);
                TempData["Success"] = $"El producto \"{model.Nombre}\" ha sido actualizado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = MensajeErrorHelper.ObtenerMensajeAmigable(ex, "actualizar el producto");
                try
                {
                    ViewBag.Proveedores = await _proveedorService.ListarProveedor();
                }
                catch
                {
                    ViewBag.Proveedores = Enumerable.Empty<DTOProveedor>();
                }
                return View(model);
            }
        }

        // POST: /Productos/Eliminar/5
        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                await _productosService.EliminarProducto(id);
                return Json(new { success = true, message = "Producto eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = MensajeErrorHelper.ObtenerMensajeAmigable(ex, "eliminar el producto") });
            }
        }
    }
}

