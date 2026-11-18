using Aplicacion.DTOs;
using Aplicacion.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentacion.Helpers;
using System.Security.Claims;

namespace Presentacion.Controllers
{
    [Authorize]
    public class AbonosController : Controller
    {
        private readonly AbonoServices _abonoService;
        private readonly CuotaCreditoServices _cuotaService;
        private readonly CreditoServices _creditoService;
        private readonly VentaServices _ventaService;
        private readonly Clientes_Services _clientesService;

        public AbonosController(
            AbonoServices abonoService,
            CuotaCreditoServices cuotaService,
            CreditoServices creditoService,
            VentaServices ventaService,
            Clientes_Services clientesService)
        {
            _abonoService = abonoService;
            _cuotaService = cuotaService;
            _creditoService = creditoService;
            _ventaService = ventaService;
            _clientesService = clientesService;
        }

        // GET: /Abonos
        public async Task<IActionResult> Index()
        {
            try
            {
                var abonos = await _abonoService.ListarAbono();
                return View(abonos.OrderByDescending(a => a.FechaAbono));
            }
            catch (Exception ex)
            {
                TempData["Error"] = MensajeErrorHelper.ObtenerMensajeAmigable(ex, "cargar los abonos");
                return View(Enumerable.Empty<DTOSAbono>());
            }
        }

        // GET: /Abonos/Crear?idCuota=5
        public async Task<IActionResult> Crear(int? idCuota = null)
        {
            try
            {
                await CargarCuotasPendientesViewBag();
                ViewBag.IdCuotaSeleccionada = idCuota;
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = MensajeErrorHelper.ObtenerMensajeAmigable(ex, "cargar las cuotas pendientes");
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: /Abonos/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(int idCuota, decimal montoAbono, string metodoPago, string? observaciones)
        {
            int idUsuario = 1;
            var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(claimId, out int parsed))
                idUsuario = parsed;

            var res = await _abonoService.RegistrarAbono(idCuota, montoAbono, metodoPago, observaciones, idUsuario);
            if (res.Success)
            {
                TempData["Success"] = res.Message;
                return RedirectToAction(nameof(Recibo), new { id = res.IdAbono });
            }

            TempData["Error"] = res.Message;
            await CargarCuotasPendientesViewBag();
            ViewBag.IdCuotaSeleccionada = idCuota;
            return View();
        }

        // GET: /Abonos/Recibo/5
        public async Task<IActionResult> Recibo(int id)
        {
            try
            {
                var abonos = await _abonoService.ListarAbono();
                var abono = abonos.FirstOrDefault(a => a.IdAbono == id);
                if (abono == null)
                {
                    TempData["Error"] = "El recibo solicitado no existe.";
                    return RedirectToAction(nameof(Index));
                }

                var cuotas = await _cuotaService.ListarCuotaCredito();
                var cuota = cuotas.FirstOrDefault(c => c.IdCuota == abono.IdCuota);
                var creditos = await _creditoService.ListarCredito();
                var credito = cuota != null ? creditos.FirstOrDefault(cr => cr.IdCredito == cuota.IdCredito) : null;
                var venta = credito != null ? await _ventaService.ObtenerPorId(credito.IdVenta) : null;
                var cliente = venta != null ? await _clientesService.ObtenerPorId(venta.IdCliente) : null;

                ViewBag.Cuota = cuota;
                ViewBag.Credito = credito;
                ViewBag.Venta = venta;
                ViewBag.Cliente = cliente;

                return View(abono);
            }
            catch (Exception ex)
            {
                TempData["Error"] = MensajeErrorHelper.ObtenerMensajeAmigable(ex, "cargar el recibo");
                return RedirectToAction(nameof(Index));
            }
        }

        private async Task CargarCuotasPendientesViewBag()
        {
            var cuotas = await _cuotaService.ListarCuotaCredito();
            var creditos = await _creditoService.ListarCredito();
            var ventas = await _ventaService.ListarVenta();
            var clientes = await _clientesService.ListaClientes();

            var dictCreditos = creditos.ToDictionary(c => c.IdCredito, c => c);
            var dictVentas = ventas.ToDictionary(v => v.IdVenta, v => v);
            var dictClientes = clientes.ToDictionary(c => c.IdCliente, c => $"{c.Nombres} {c.Apellidos}");

            var pendientes = cuotas
                .Where(c => c.Estado != "Pagada" && c.Estado != "Anulada" && c.SaldoCuota > 0)
                .OrderBy(c => c.FechaVencimiento)
                .Select(c =>
                {
                    var cred = dictCreditos.ContainsKey(c.IdCredito) ? dictCreditos[c.IdCredito] : null;
                    var ven = (cred != null && dictVentas.ContainsKey(cred.IdVenta)) ? dictVentas[cred.IdVenta] : null;
                    var cliNombre = (ven != null && dictClientes.ContainsKey(ven.IdCliente)) ? dictClientes[ven.IdCliente] : "Cliente N/D";
                    return new
                    {
                        c.IdCuota,
                        c.SaldoCuota,
                        Texto = $"{cliNombre} | Factura {ven?.NumeroFactura ?? "-"} | Cuota #{c.NumeroCuota} | Vence: {c.FechaVencimiento:dd/MM/yyyy} | Saldo: C$ {c.SaldoCuota:N2}"
                    };
                }).ToList();

            ViewBag.CuotasPendientes = pendientes;
        }
    }
}

