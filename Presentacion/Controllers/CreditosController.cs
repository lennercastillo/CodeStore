using Aplicacion.DTOs;
using Aplicacion.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [Authorize]
    public class CreditosController : Controller
    {
        private readonly CreditoServices _creditoService;
        private readonly CuotaCreditoServices _cuotaService;
        private readonly VentaServices _ventaService;
        private readonly Clientes_Services _clientesService;

        public CreditosController(
            CreditoServices creditoService,
            CuotaCreditoServices cuotaService,
            VentaServices ventaService,
            Clientes_Services clientesService)
        {
            _creditoService = creditoService;
            _cuotaService = cuotaService;
            _ventaService = ventaService;
            _clientesService = clientesService;
        }

        // GET: /Creditos
        public async Task<IActionResult> Index()
        {
            try
            {
                var creditos = await _creditoService.ListarCredito();
                var ventas = await _ventaService.ListarVenta();
                var clientes = await _clientesService.ListaClientes();

                var dictVentas = ventas.ToDictionary(v => v.IdVenta, v => v);
                var dictClientes = clientes.ToDictionary(c => c.IdCliente, c => $"{c.Nombres} {c.Apellidos}");

                ViewBag.Ventas = dictVentas;
                ViewBag.Clientes = dictClientes;

                return View(creditos.OrderByDescending(c => c.FechaRegistro));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar créditos: {ex.Message}";
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
                var todasCuotas = await _cuotaService.ListarCuotaCredito();
                var cuotas = todasCuotas.Where(c => c.IdCredito == id).OrderBy(c => c.NumeroCuota).ToList();

                ViewBag.Venta = venta;
                ViewBag.Cliente = cliente;
                ViewBag.Cuotas = cuotas;

                return View(credito);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar detalle de crédito: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}

