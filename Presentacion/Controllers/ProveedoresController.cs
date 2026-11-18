using Aplicacion.DTOs;
using Aplicacion.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [Authorize]
    public class ProveedoresController : Controller
    {
        private readonly ProveedorServices _proveedorService;

        public ProveedoresController(ProveedorServices proveedorService)
        {
            _proveedorService = proveedorService;
        }

        // GET: /Proveedores
        public async Task<IActionResult> Index()
        {
            try
            {
                var proveedores = await _proveedorService.ListarProveedor();
                return View(proveedores);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar proveedores: {ex.Message}";
                return View(Enumerable.Empty<DTOProveedor>());
            }
        }

        // GET: /Proveedores/Crear
        public IActionResult Crear()
        {
            var nuevo = new DTOProveedor
            {
                NumeroRUC = string.Empty,
                RazonSocial = string.Empty,
                Activo = true
            };
            return View(nuevo);
        }

        // POST: /Proveedores/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(DTOProveedor model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _proveedorService.NuevoProveedor(model);
                TempData["Success"] = $"Proveedor \"{model.RazonSocial}\" registrado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al registrar proveedor: {ex.Message}";
                return View(model);
            }
        }

        // GET: /Proveedores/Editar/5
        public async Task<IActionResult> Editar(int id)
        {
            var proveedor = await _proveedorService.ObtenerPorId(id);
            if (proveedor == null)
            {
                TempData["Error"] = "El proveedor solicitado no fue encontrado.";
                return RedirectToAction(nameof(Index));
            }
            return View(proveedor);
        }

        // POST: /Proveedores/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(DTOProveedor model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _proveedorService.EditarProveedor(model);
                TempData["Success"] = $"Proveedor \"{model.RazonSocial}\" actualizado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al actualizar proveedor: {ex.Message}";
                return View(model);
            }
        }

        // POST: /Proveedores/Eliminar/5
        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                await _proveedorService.EliminarProveedor(id);
                return Json(new { success = true, message = "Proveedor eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}

