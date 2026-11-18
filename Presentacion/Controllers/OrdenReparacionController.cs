using Aplicacion.DTOs;
using Aplicacion.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentacion.Controllers
{
    [Authorize]
    public class OrdenReparacionController : Controller
    {
        private readonly OrdenReparacionServices _ordenService;
        private readonly Clientes_Services _clientesService;
        private readonly UsuarioServices _usuarioService;

        public OrdenReparacionController(
            OrdenReparacionServices ordenService,
            Clientes_Services clientesService,
            UsuarioServices usuarioService)
        {
            _ordenService = ordenService;
            _clientesService = clientesService;
            _usuarioService = usuarioService;
        }

        // GET: /OrdenReparacion
        public async Task<IActionResult> Index()
        {
            try
            {
                var ordenes = await _ordenService.ListarOrdenReparacion();
                var clientes = await _clientesService.ListaClientes();
                ViewBag.Clientes = clientes.ToDictionary(c => c.IdCliente, c => $"{c.Nombres} {c.Apellidos}");
                return View(ordenes.OrderByDescending(o => o.FechaIngreso));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar órdenes de reparación: {ex.Message}";
                return View(Enumerable.Empty<DTOSOrdenReparacion>());
            }
        }

        // GET: /OrdenReparacion/Crear
        public async Task<IActionResult> Crear()
        {
            await CargarCatalogos();
            int idTecnico = ObtenerUsuarioActual();

            var nuevo = new DTOSOrdenReparacion
            {
                NumeroOrden = $"ORD-{DateTime.Now:yyyyMMdd}-{Random.Shared.Next(100, 999)}",
                IdUsuarioTecnico = idTecnico,
                DescripcionEquipo = string.Empty,
                Marca = string.Empty,
                Modelo = string.Empty,
                NumeroSerie = string.Empty,
                EstadoEquipo = "Recibido",
                FechaIngreso = DateTime.Now
            };
            return View(nuevo);
        }

        // POST: /OrdenReparacion/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(DTOSOrdenReparacion model)
        {
            if (!ModelState.IsValid)
            {
                await CargarCatalogos();
                return View(model);
            }

            try
            {
                if (model.FechaIngreso == default) model.FechaIngreso = DateTime.Now;
                await _ordenService.NuevoOrdenReparacion(model);
                TempData["Success"] = $"Orden de reparación \"{model.NumeroOrden}\" registrada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al registrar orden: {ex.Message}";
                await CargarCatalogos();
                return View(model);
            }
        }

        // GET: /OrdenReparacion/Editar/5
        public async Task<IActionResult> Editar(int id)
        {
            var orden = await _ordenService.ObtenerPorId(id);
            if (orden == null)
            {
                TempData["Error"] = "La orden solicitada no existe.";
                return RedirectToAction(nameof(Index));
            }

            await CargarCatalogos();
            return View(orden);
        }

        // POST: /OrdenReparacion/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(DTOSOrdenReparacion model)
        {
            if (!ModelState.IsValid)
            {
                await CargarCatalogos();
                return View(model);
            }

            try
            {
                if (model.EstadoEquipo == "Reparado" && !model.FechaFinalizacion.HasValue)
                    model.FechaFinalizacion = DateTime.Now;

                if (model.EstadoEquipo == "Entregado" && !model.FechaEntrega.HasValue)
                    model.FechaEntrega = DateTime.Now;

                await _ordenService.EditarOrdenReparacion(model);
                TempData["Success"] = $"Orden \"{model.NumeroOrden}\" actualizada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al actualizar orden: {ex.Message}";
                await CargarCatalogos();
                return View(model);
            }
        }

        // POST: /OrdenReparacion/Eliminar/5
        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                await _ordenService.EliminarOrdenReparacion(id);
                return Json(new { success = true, message = "Orden eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private async Task CargarCatalogos()
        {
            var clientes = await _clientesService.ListaClientes();
            var usuarios = await _usuarioService.ListaRoles();
            ViewBag.Clientes = clientes.Where(c => c.Activo).ToList();
            ViewBag.Tecnicos = usuarios.Where(u => u.Activo).ToList();
        }

        private int ObtenerUsuarioActual()
        {
            var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(claimId, out int id) ? id : 1;
        }
    }
}

