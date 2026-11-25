using Aplicacion.DTOs;
using Aplicacion.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentacion.Helpers;

namespace Presentacion.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class UsuariosController : Controller
    {
        private readonly UsuarioServices _usuarioService;
        private readonly rolesServices _rolesService;

        public UsuariosController(UsuarioServices usuarioService, rolesServices rolesService)
        {
            _usuarioService = usuarioService;
            _rolesService = rolesService;
        }

        // GET: /Usuarios
        public async Task<IActionResult> Index()
        {
            try
            {
                var usuarios = await _usuarioService.ListaRoles();
                var roles = await _rolesService.ListaRoles();
                ViewBag.Roles = roles.ToDictionary(r => r.IdRol, r => r.NombreRol);
                return View(usuarios);
            }
            catch (Exception ex)
            {
                TempData["Error"] = MensajeErrorHelper.ObtenerMensajeAmigable(ex, "cargar usuarios");
                return View(Enumerable.Empty<DTOSUsuario>());
            }
        }

        // GET: /Usuarios/Crear
        public async Task<IActionResult> Crear()
        {
            await CargarRolesViewBag();
            var nuevo = new DTOSUsuario
            {
                Nombres = string.Empty,
                Apellidos = string.Empty,
                Correo = string.Empty,
                Activo = true
            };
            return View(nuevo);
        }

        // POST: /Usuarios/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(DTOSUsuario model)
        {
            if (string.IsNullOrWhiteSpace(model.Contraseña))
            {
                ModelState.AddModelError(nameof(model.Contraseña), "La contraseña es obligatoria al crear un usuario.");
            }

            if (!ModelState.IsValid)
            {
                await CargarRolesViewBag();
                return View(model);
            }

            try
            {
                await _usuarioService.NuevoUsuario(model);
                TempData["Success"] = $"Usuario \"{model.Nombres} {model.Apellidos}\" registrado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = MensajeErrorHelper.ObtenerMensajeAmigable(ex, "registrar el usuario");
                await CargarRolesViewBag();
                return View(model);
            }
        }

        // GET: /Usuarios/Editar/5
        public async Task<IActionResult> Editar(int id)
        {
            try
            {
                var usuario = await _usuarioService.ObtenerPorId(id);
                if (usuario == null)
                {
                    TempData["Error"] = "El usuario solicitado no existe.";
                    return RedirectToAction(nameof(Index));
                }

                await CargarRolesViewBag(usuario.IdRol);
                return View(usuario);
            }
            catch (Exception ex)
            {
                TempData["Error"] = MensajeErrorHelper.ObtenerMensajeAmigable(ex, "cargar el usuario");
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: /Usuarios/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(DTOSUsuario model)
        {
            if (!ModelState.IsValid)
            {
                await CargarRolesViewBag(model.IdRol);
                return View(model);
            }

            try
            {
                await _usuarioService.EditarUsuario(model);
                TempData["Success"] = $"Usuario \"{model.Nombres} {model.Apellidos}\" actualizado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = MensajeErrorHelper.ObtenerMensajeAmigable(ex, "actualizar el usuario");
                await CargarRolesViewBag(model.IdRol);
                return View(model);
            }
        }

        // POST: /Usuarios/Eliminar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var claimId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(claimId, out int idActual) && idActual == id)
                {
                    return Json(new { success = false, message = "No puedes eliminar tu propia cuenta mientras tienes la sesión abierta." });
                }

                await _usuarioService.Eliminarusuario(id);
                return Json(new { success = true, message = "Usuario eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = MensajeErrorHelper.ObtenerMensajeAmigable(ex, "eliminar el usuario") });
            }
        }

        private async Task CargarRolesViewBag(int? idRolActual = null)
        {
            try
            {
                var roles = await _rolesService.ListaRoles();
                ViewBag.Roles = roles.Where(r => r.Activo || (idRolActual.HasValue && r.IdRol == idRolActual.Value)).ToList();
            }
            catch
            {
                ViewBag.Roles = new List<RolDT>();
            }
        }
    }
}

