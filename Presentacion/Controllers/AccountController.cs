using Aplicacion.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentacion.Helpers;
using Presentacion.Models;
using System.Security.Claims;

namespace Presentacion.Controllers
{
    public class AccountController : Controller
    {
        private readonly UsuarioServices _usuarioService;
        private readonly rolesServices _rolesService;

        public AccountController(UsuarioServices usuarioService, rolesServices rolesService)
        {
            _usuarioService = usuarioService;
            _rolesService = rolesService;
        }

        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            ViewData["ReturnUrl"] = model.ReturnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var usuario = await _usuarioService.ValidarLogin(model.Correo, model.Contraseña);

                if (usuario == null)
                {
                    ModelState.AddModelError(string.Empty, "Credenciales incorrectas o usuario inactivo.");
                    return View(model);
                }

                // Obtener nombre del rol
                string nombreRol = "Usuario";
                try
                {
                    var roles = await _rolesService.ListaRoles();
                    var rol = roles.FirstOrDefault(r => r.IdRol == usuario.IdRol);
                    if (rol != null)
                    {
                        nombreRol = rol.NombreRol;
                    }
                }
                catch
                {
                    // Si falla consulta de rol, se mantiene rol por defecto
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                    new Claim(ClaimTypes.Name, $"{usuario.Nombres} {usuario.Apellidos}"),
                    new Claim(ClaimTypes.Email, usuario.Correo),
                    new Claim(ClaimTypes.Role, nombreRol),
                    new Claim("IdRol", usuario.IdRol.ToString()),
                    new Claim("Nombres", usuario.Nombres),
                    new Claim("Apellidos", usuario.Apellidos)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.Recordarme,
                    ExpiresUtc = model.Recordarme ? DateTimeOffset.UtcNow.AddDays(7) : DateTimeOffset.UtcNow.AddHours(8)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                TempData["Success"] = $"¡Bienvenido de nuevo, {usuario.Nombres}!";

                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, MensajeErrorHelper.ObtenerMensajeAmigable(ex, "iniciar sesión"));
                return View(model);
            }
        }

        // POST / GET: /Account/Logout
        [HttpPost]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        // GET: /Account/AccessDenied
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

