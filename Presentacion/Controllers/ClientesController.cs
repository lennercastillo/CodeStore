using Aplicacion.DTOs;
using Aplicacion.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentacion.Helpers;

namespace Presentacion.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
        private readonly Clientes_Services _clientesService;

        public ClientesController(Clientes_Services clientesService)
        {
            _clientesService = clientesService;
        }

        // GET: /Clientes
        public async Task<IActionResult> Index()
        {
            try
            {
                var clientes = await _clientesService.ListaClientes();
                return View(clientes);
            }
            catch (Exception ex)
            {
                TempData["Error"] = MensajeErrorHelper.ObtenerMensajeAmigable(ex, "cargar clientes");
                return View(Enumerable.Empty<DTOsClientes>());
            }
        }

        // GET: /Clientes/Crear
        public IActionResult Crear()
        {
            var nuevo = new DTOsClientes
            {
                NumeroIdentificacion = string.Empty,
                Nombres = string.Empty,
                Apellidos = string.Empty,
                Activo = true
            };
            return View(nuevo);
        }

        // POST: /Clientes/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(DTOsClientes model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _clientesService.NuevoClientes(model);
                TempData["Success"] = $"Cliente \"{model.Nombres} {model.Apellidos}\" registrado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = MensajeErrorHelper.ObtenerMensajeAmigable(ex, "registrar el cliente");
                return View(model);
            }
        }

        // POST: /Clientes/CrearRapido (AJAX desde POS)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearRapido([FromBody] DTOsClientes? model)
        {
            if (model == null)
                return Json(new { success = false, message = "No se recibieron los datos del cliente." });

            model.Activo = true;

            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return Json(new { success = false, message = errors });
            }

            try
            {
                await _clientesService.NuevoClientes(model);
                var lista = await _clientesService.ListaClientes();
                var creado = lista
                    .OrderByDescending(c => c.IdCliente)
                    .FirstOrDefault(c => string.Equals(c.NumeroIdentificacion?.Trim(), model.NumeroIdentificacion.Trim(), StringComparison.OrdinalIgnoreCase));

                if (creado == null)
                    return Json(new { success = false, message = "Cliente guardado, pero no se pudo recuperar su identificador." });

                return Json(new
                {
                    success = true,
                    message = $"Cliente \"{creado.Nombres} {creado.Apellidos}\" registrado.",
                    idCliente = creado.IdCliente,
                    texto = $"{creado.Nombres} {creado.Apellidos} ({creado.NumeroIdentificacion})"
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = MensajeErrorHelper.ObtenerMensajeAmigable(ex, "registrar el cliente") });
            }
        }

        // GET: /Clientes/Editar/5
        public async Task<IActionResult> Editar(int id)
        {
            try
            {
                var cliente = await _clientesService.ObtenerPorId(id);
                if (cliente == null)
                {
                    TempData["Error"] = "El cliente solicitado no fue encontrado.";
                    return RedirectToAction(nameof(Index));
                }
                return View(cliente);
            }
            catch (Exception ex)
            {
                TempData["Error"] = MensajeErrorHelper.ObtenerMensajeAmigable(ex, "cargar el cliente");
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: /Clientes/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(DTOsClientes model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _clientesService.EditarClientes(model);
                TempData["Success"] = $"Cliente \"{model.Nombres} {model.Apellidos}\" actualizado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = MensajeErrorHelper.ObtenerMensajeAmigable(ex, "actualizar el cliente");
                return View(model);
            }
        }

        // POST: /Clientes/Eliminar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                await _clientesService.Eliminarclientes(id);
                return Json(new { success = true, message = "Cliente eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = MensajeErrorHelper.ObtenerMensajeAmigable(ex, "eliminar el cliente") });
            }
        }
    }
}

