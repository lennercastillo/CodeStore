using Aplicacion.DTOs;
using Aplicacion.Services;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
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
                TempData["Error"] = $"Error al cargar clientes: {ex.Message}";
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
                TempData["Error"] = $"Error al registrar cliente: {ex.Message}";
                return View(model);
            }
        }

        // GET: /Clientes/Editar/5
        public async Task<IActionResult> Editar(int id)
        {
            var cliente = await _clientesService.ObtenerPorId(id);
            if (cliente == null)
            {
                TempData["Error"] = "El cliente solicitado no fue encontrado.";
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
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
                TempData["Error"] = $"Error al actualizar cliente: {ex.Message}";
                return View(model);
            }
        }

        // POST: /Clientes/Eliminar/5
        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                await _clientesService.Eliminarclientes(id);
                return Json(new { success = true, message = "Cliente eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}

