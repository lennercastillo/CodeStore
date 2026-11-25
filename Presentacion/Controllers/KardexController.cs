using Aplicacion.DTOs;
using Aplicacion.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentacion.Helpers;
using System.Security.Claims;

namespace Presentacion.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class KardexController : Controller
    {
        private readonly KardexServices _kardexService;
        private readonly ProductosServices _productosService;

        public KardexController(KardexServices kardexService, ProductosServices productosService)
        {
            _kardexService = kardexService;
            _productosService = productosService;
        }

        // GET: /Kardex
        public async Task<IActionResult> Index(int? idProducto = null)
        {
            try
            {
                var movimientos = await _kardexService.ListarKardex();
                var productos = await _productosService.ListarProducto();

                ViewBag.Productos = productos.ToDictionary(p => p.IdProducto, p => p);
                ViewBag.IdProductoFiltro = idProducto;

                if (idProducto.HasValue && idProducto.Value > 0)
                {
                    movimientos = movimientos.Where(m => m.IdProducto == idProducto.Value);
                }

                return View(movimientos.OrderByDescending(m => m.FechaMovimiento).ThenByDescending(m => m.IdMovimiento));
            }
            catch (Exception ex)
            {
                TempData["Error"] = MensajeErrorHelper.ObtenerMensajeAmigable(ex, "cargar el Kardex");
                return View(Enumerable.Empty<DTOSKardex>());
            }
        }

        // GET: /Kardex/Ajuste
        public async Task<IActionResult> Ajuste()
        {
            try
            {
                var productos = await _productosService.ListarProducto();
                ViewBag.Productos = productos.Where(p => p.Activo).ToList();
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = MensajeErrorHelper.ObtenerMensajeAmigable(ex, "cargar los productos");
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: /Kardex/Ajuste
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ajuste(int idProducto, string tipoMovimiento, int cantidad, string justificacion)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int idUsuario))
                return RedirectToAction("Login", "Account");

            try
            {
                var res = await _kardexService.RegistrarAjuste(idProducto, tipoMovimiento, cantidad, justificacion, idUsuario);
                if (res.Success)
                {
                    TempData["Success"] = res.Message;
                    return RedirectToAction(nameof(Index));
                }

                TempData["Error"] = MensajeErrorHelper.ObtenerMensajeDesdeTexto(res.Message, "registrar el ajuste de inventario");
            }
            catch (Exception ex)
            {
                TempData["Error"] = MensajeErrorHelper.ObtenerMensajeAmigable(ex, "registrar el ajuste de inventario");
            }

            try
            {
                var productos = await _productosService.ListarProducto();
                ViewBag.Productos = productos.Where(p => p.Activo).ToList();
            }
            catch
            {
                ViewBag.Productos = new List<DTOSProductos>();
            }
            ViewBag.IdProducto = idProducto;
            ViewBag.TipoMovimiento = tipoMovimiento;
            ViewBag.Cantidad = cantidad;
            ViewBag.Justificacion = justificacion;
            return View();
        }
    }
}

