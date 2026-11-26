using Aplicacion.DTOs;
using Aplicacion.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentacion.Helpers;

namespace Presentacion.Controllers
{
    [Authorize(Roles = "Administrador,Vendedor")]
    public class CreditosController : Controller
    {
        private readonly CreditoServices _creditoService;
        private readonly CuotaCreditoServices _cuotaService;
        private readonly VentaServices _ventaService;
        private readonly Clientes_Services _clientesService;
        private readonly UsuarioServices _usuarioService;

        public CreditosController(
            CreditoServices creditoService,
            CuotaCreditoServices cuotaService,
            VentaServices ventaService,
            Clientes_Services clientesService,
            UsuarioServices usuarioService)
        {
            _creditoService = creditoService;
            _cuotaService = cuotaService;
            _ventaService = ventaService;
            _clientesService = clientesService;
            _usuarioService = usuarioService;
        }

        // GET: /Creditos
        public async Task<IActionResult> Index()
        {
            try
            {
                var creditos = await _creditoService.ListarCredito();
                var ventas = await _ventaService.ListarVenta();
                var clientes = await _clientesService.ListaClientes();
                var usuarios = await _usuarioService.ListaRoles();
                var cuotas = await _cuotaService.ListarCuotaCredito();

                var dictVentas = ventas.ToDictionary(v => v.IdVenta, v => v);
                var dictClientes = clientes.ToDictionary(c => c.IdCliente, c => $"{c.Nombres} {c.Apellidos}");
                var dictVendedores = usuarios.ToDictionary(u => u.IdUsuario, u => $"{u.Nombres} {u.Apellidos}");
                var fechaCorteRetraso = DateTime.Today.AddDays(-5);
                var creditosRetrasados = cuotas
                    .Where(q => q.Estado != "Pagada" && q.Estado != "Anulada" && q.FechaVencimiento.Date < fechaCorteRetraso)
                    .Select(q => q.IdCredito)
                    .ToHashSet();

                ViewBag.Ventas = dictVentas;
                ViewBag.Clientes = dictClientes;
                ViewBag.Vendedores = dictVendedores;
                ViewBag.CreditosRetrasados = creditosRetrasados;

                return View(creditos.OrderByDescending(c => c.FechaRegistro));
            }
            catch (Exception ex)
            {
                TempData["Error"] = MensajeErrorHelper.ObtenerMensajeAmigable(ex, "cargar los créditos");
                return View(Enumerable.Empty<DTOSCredito>());
            }
        }

        // GET: /Creditos/Detalle/5
        public async Task<IActionResult> Detalle(int id)
        {
            try
            {
                var creditos = await _creditoService.ListarCredito();
                var credito = creditos.FirstOrDefault(c => c.IdCredito == id);
                if (credito == null)
                {
                    TempData["Error"] = "El crédito solicitado no existe.";
                    return RedirectToAction(nameof(Index));
                }

                var venta = await _ventaService.ObtenerPorId(credito.IdVenta);
                var cliente = venta != null ? await _clientesService.ObtenerPorId(venta.IdCliente) : null;
                var vendedor = venta != null ? await _usuarioService.ObtenerPorId(venta.IdUsuarioVendedor) : null;
                var todasCuotas = await _cuotaService.ListarCuotaCredito();
                var cuotas = todasCuotas.Where(c => c.IdCredito == id).OrderBy(c => c.NumeroCuota).ToList();

                ViewBag.Venta = venta;
                ViewBag.Cliente = cliente;
                ViewBag.Vendedor = vendedor;
                ViewBag.Cuotas = cuotas;

                return View(credito);
            }
            catch (Exception ex)
            {
                TempData["Error"] = MensajeErrorHelper.ObtenerMensajeAmigable(ex, "cargar el detalle del crédito");
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: /Creditos/AplicarMora
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AplicarMora(int idCuota, decimal recargoMora)
        {
            try
            {
                var res = await _cuotaService.AplicarMora(idCuota, recargoMora);
                string msg = res.Success ? res.Message : MensajeErrorHelper.ObtenerMensajeDesdeTexto(res.Message, "actualizar el recargo por mora");
                return Json(new { success = res.Success, message = msg });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = MensajeErrorHelper.ObtenerMensajeAmigable(ex, "actualizar el recargo por mora") });
            }
        }
    }
}

