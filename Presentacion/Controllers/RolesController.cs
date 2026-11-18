using Aplicacion.DTOs;
using Aplicacion.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        private readonly rolesServices _rolesService;

        public RolesController(rolesServices rolesService)
        {
            _rolesService = rolesService;
        }

        // GET: /Roles
        public async Task<IActionResult> Index()
        {
            try
            {
                var roles = await _rolesService.ListaRoles();
                return View(roles);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar roles: {ex.Message}";
                return View(Enumerable.Empty<RolDT>());
            }
        }

        // GET: /Roles/Crear
        public IActionResult Crear()
        {
            var nuevo = new RolDT
            {
                CodigoRol = string.Empty,
                NombreRol = string.Empty,
                Descripcion = string.Empty,
                Activo = true
            };
            return View(nuevo);
        }

        // POST: /Roles/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(RolDT model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _rolesService.NuevoRol(model);
                TempData["Success"] = $"Rol \"{model.NombreRol}\" creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al crear rol: {ex.Message}";
                return View(model);
            }
        }

        // GET: /Roles/Editar/5
        public async Task<IActionResult> Editar(int id)
        {
            var rol = await _rolesService.ObtenerPorId(id);
            if (rol == null)
            {
                TempData["Error"] = "El rol solicitado no existe.";
                return RedirectToAction(nameof(Index));
            }
            return View(rol);
        }

        // POST: /Roles/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(RolDT model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _rolesService.EditarRol(model);
                TempData["Success"] = $"Rol \"{model.NombreRol}\" actualizado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al actualizar rol: {ex.Message}";
                return View(model);
            }
        }

        // POST: /Roles/Eliminar/5
        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                await _rolesService.EliminarRoles(id);
                return Json(new { success = true, message = "Rol eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}

