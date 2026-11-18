using Aplicacion.DTOs;
using Aplicacion.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentacion.Controllers
{
    [Authorize]
    public class ReclamoGarantiaController : Controller
    {
        private readonly ReclamoGarantiaServices _reclamoService;
        private readonly DetalleVentaServices _detalleVentaService;
        private readonly VentaServices _ventaService;
        private readonly ProductosServices _productosService;
        private readonly UsuarioServices _usuarioService;

        public ReclamoGarantiaController(
            ReclamoGarantiaServices reclamoService,
            DetalleVentaServices detalleVentaService,
            VentaServices ventaService,
            ProductosServices productosService,
            UsuarioServices usuarioService)
        {
            _reclamoService = reclamoService;
            _detalleVentaService = detalleVentaService;
            _ventaService = ventaService;
            _productosService = productosService;
            _usuarioService = usuarioService;
        }

        // GET: /ReclamoGarantia
        public async Task<IActionResult> Index()
        {
            try
            {
                var reclamos = await _reclamoService.ListarReclamoGarantia();
                return View(reclamos.OrderByDescending(r => r.FechaReclamo));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar reclamos de garantía: {ex.Message}";
                return View(Enumerable.Empty<DTOSReclamoGarantia>());
            }
        }

        // GET: /ReclamoGarantia/Crear
        public async Task<IActionResult> Crear()
        {
            await CargarCatalogos();
            var nuevo = new DTOSReclamoGarantia
            {
                NumeroReclamo = $"GAR-{DateTime.Now:yyyyMMdd}-{Random.Shared.Next(100, 999)}",
                IdUsuarioTecnico = ObtenerUsuarioActual(),
                MotivoFalla = string.Empty,
                EstadoRevision = "Recibido",
                FechaReclamo = DateTime.Now
            };
            return View(nuevo);
        }

        // POST: /ReclamoGarantia/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(DTOSReclamoGarantia model)
        {
            if (!ModelState.IsValid)
            {
                await CargarCatalogos();
                return View(model);
            }

            try
            {
                // Validación crítica de vigencia de garantía
                var detalles = await _detalleVentaService.ListarDetalleVenta();
                var detalle = detalles.FirstOrDefault(d => d.IdDetalleVenta == model.IdDetalleVenta);
                if (detalle == null)
                {
                    ModelState.AddModelError(nameof(model.IdDetalleVenta), "El detalle de venta seleccionado no existe.");
                    await CargarCatalogos();
                    return View(model);
                }

                if (!detalle.FechaFinGarantia.HasValue || detalle.FechaFinGarantia.Value.Date < DateTime.Today)
                {
                    var fechaStr = detalle.FechaFinGarantia?.ToString("dd/MM/yyyy") ?? "Sin garantía";
                    TempData["Error"] = $"Garantía vencida o inexistente (Vencimiento: {fechaStr}). Este equipo debe ingresar como Orden de Reparación regular.";
                    await CargarCatalogos();
                    return View(model);
                }

                if (model.FechaReclamo == default) model.FechaReclamo = DateTime.Now;
                await _reclamoService.NuevoReclamoGarantia(model);
                TempData["Success"] = $"Reclamo de garantía \"{model.NumeroReclamo}\" registrado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al registrar reclamo: {ex.Message}";
                await CargarCatalogos();
                return View(model);
            }
        }

        // GET: /ReclamoGarantia/Editar/5
        public async Task<IActionResult> Editar(int id)
        {
            var reclamo = await _reclamoService.ObtenerPorId(id);
            if (reclamo == null)
            {
                TempData["Error"] = "El reclamo solicitado no existe.";
                return RedirectToAction(nameof(Index));
            }
            await CargarCatalogos();
            return View(reclamo);
        }

        // POST: /ReclamoGarantia/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(DTOSReclamoGarantia model)
        {
            if (!ModelState.IsValid)
            {
                await CargarCatalogos();
                return View(model);
            }

            try
            {
                if ((model.EstadoRevision == "Resuelto" || model.EstadoRevision == "Rechazado") && !model.FechaResolucion.HasValue)
                    model.FechaResolucion = DateTime.Now;

                await _reclamoService.EditarReclamoGarantia(model);
                TempData["Success"] = $"Reclamo \"{model.NumeroReclamo}\" actualizado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al actualizar reclamo: {ex.Message}";
                await CargarCatalogos();
                return View(model);
            }
        }

        // POST: /ReclamoGarantia/Eliminar/5
        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                await _reclamoService.EliminarReclamoGarantia(id);
                return Json(new { success = true, message = "Reclamo eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private async Task CargarCatalogos()
        {
            var detalles = await _detalleVentaService.ListarDetalleVenta();
            var ventas = await _ventaService.ListarVenta();
            var productos = await _productosService.ListarProducto();
            var usuarios = await _usuarioService.ListaRoles();

            var dictVentas = ventas.ToDictionary(v => v.IdVenta, v => v);
            var dictProductos = productos.ToDictionary(p => p.IdProducto, p => p);

            var itemsGarantia = detalles.Select(d =>
            {
                var ven = dictVentas.ContainsKey(d.IdVenta) ? dictVentas[d.IdVenta] : null;
                var prod = dictProductos.ContainsKey(d.IdProducto) ? dictProductos[d.IdProducto] : null;
                var vence = d.FechaFinGarantia.HasValue ? d.FechaFinGarantia.Value.ToString("dd/MM/yyyy") : "Sin garantía";
                return new
                {
                    d.IdDetalleVenta,
                    Texto = $"Factura {ven?.NumeroFactura ?? "-"} | {prod?.Nombre ?? "Producto"} | Cant: {d.Cantidad} | Garantía hasta: {vence}"
                };
            }).OrderByDescending(x => x.IdDetalleVenta).ToList();

            ViewBag.DetallesVenta = itemsGarantia;
            ViewBag.Tecnicos = usuarios.Where(u => u.Activo).ToList();
        }

        private int ObtenerUsuarioActual()
        {
            var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(claimId, out int id) ? id : 1;
        }
    }
}

