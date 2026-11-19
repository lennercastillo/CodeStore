using Aplicacion.DTOs;
using Aplicacion.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentacion.Helpers;
using System.Security.Claims;

namespace Presentacion.Controllers
{
    [Authorize(Roles = "Administrador,Vendedor")]
    public class VentasController : Controller
    {
        private readonly VentaServices _ventaService;
        private readonly ProductosServices _productosService;
        private readonly Clientes_Services _clientesService;
        private readonly DetalleVentaServices _detalleVentaService;
        private readonly DetalleVentaSerieServices _serieService;
        private readonly CreditoServices _creditoService;
        private readonly CuotaCreditoServices _cuotaService;

        public VentasController(
            VentaServices ventaService,
            ProductosServices productosService,
            Clientes_Services clientesService,
            DetalleVentaServices detalleVentaService,
            DetalleVentaSerieServices serieService,
            CreditoServices creditoService,
            CuotaCreditoServices cuotaService)
        {
            _ventaService = ventaService;
            _productosService = productosService;
            _clientesService = clientesService;
            _detalleVentaService = detalleVentaService;
            _serieService = serieService;
            _creditoService = creditoService;
            _cuotaService = cuotaService;
        }

        // GET: /Ventas
        public async Task<IActionResult> Index()
        {
            try
            {
                var ventas = await _ventaService.ListarVenta();
                var clientes = await _clientesService.ListaClientes();
                ViewBag.Clientes = clientes.ToDictionary(c => c.IdCliente, c => $"{c.Nombres} {c.Apellidos}");
                return View(ventas.OrderByDescending(v => v.FechaVenta));
            }
            catch (Exception ex)
            {
                TempData["Error"] = MensajeErrorHelper.ObtenerMensajeAmigable(ex, "listar las ventas");
                return View(Enumerable.Empty<DTOSVenta>());
            }
        }

        // GET: /Ventas/Crear (POS)
        public async Task<IActionResult> Crear()
        {
            try
            {
                var productos = await _productosService.ListarProducto();
                var clientes = await _clientesService.ListaClientes();

                ViewBag.Productos = productos.Where(p => p.Activo).ToList();
                ViewBag.Clientes = clientes.Where(c => c.Activo).ToList();

                var nuevo = new VentaRegistroDTO
                {
                    PorcentajeImpuesto = 15.00m,
                    TipoVenta = "Contado",
                    MetodoPago = "Efectivo"
                };

                return View(nuevo);
            }
            catch (Exception ex)
            {
                TempData["Error"] = MensajeErrorHelper.ObtenerMensajeAmigable(ex, "iniciar el punto de venta");
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: /Ventas/RegistrarVenta (AJAX)
        [HttpPost]
        public async Task<IActionResult> RegistrarVenta([FromBody] VentaRegistroDTO model)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return Json(new { success = false, message = errors });
            }

            int idUsuario = 1; // Fallback
            var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(claimId, out int parsedId))
            {
                idUsuario = parsedId;
            }

            var resultado = await _ventaService.RegistrarVentaCompleta(model, idUsuario);

            return Json(new
            {
                success = resultado.Success,
                message = resultado.Message,
                idVenta = resultado.IdVenta,
                numeroFactura = resultado.NumeroFactura
            });
        }

        // GET: /Ventas/Detalle/5
        public async Task<IActionResult> Detalle(int id)
        {
            try
            {
                var venta = await _ventaService.ObtenerPorId(id);
                if (venta == null)
                {
                    TempData["Error"] = "La venta solicitada no existe.";
                    return RedirectToAction(nameof(Index));
                }

                var cliente = await _clientesService.ObtenerPorId(venta.IdCliente);
                var todosDetalles = await _detalleVentaService.ListarDetalleVenta();
                var detallesVenta = todosDetalles.Where(d => d.IdVenta == id).ToList();

                var productos = await _productosService.ListarProducto();
                var dictProductos = productos.ToDictionary(p => p.IdProducto, p => p);
                var todasSeries = await _serieService.ListarDetalleVentaSerie();

                var dtoCompleto = new VentaDetalleCompletoDTO
                {
                    Venta = new DTOSVenta
                    {
                        IdVenta = venta.IdVenta,
                        NumeroFactura = venta.NumeroFactura,
                        IdCliente = venta.IdCliente,
                        IdUsuarioVendedor = venta.IdUsuarioVendedor,
                        FechaVenta = venta.FechaVenta,
                        TipoVenta = venta.TipoVenta,
                        MetodoPago = venta.MetodoPago,
                        PorcentajeImpuesto = venta.PorcentajeImpuesto,
                        Subtotal = venta.Subtotal,
                        Impuesto = venta.Impuesto,
                        TotalVenta = venta.TotalVenta,
                        MontoRecibido = venta.MontoRecibido,
                        Vuelto = venta.Vuelto,
                        Estado = venta.Estado,
                        FechaAnulacion = venta.FechaAnulacion,
                        MotivoAnulacion = venta.MotivoAnulacion
                    },
                    Cliente = cliente,
                    Detalles = detallesVenta.Select(d => new ItemDetalleCompletoDTO
                    {
                        Detalle = d,
                        Producto = dictProductos.ContainsKey(d.IdProducto) ? dictProductos[d.IdProducto] : null,
                        Series = todasSeries.Where(s => s.IdDetalleVenta == d.IdDetalleVenta).Select(s => s.NumeroSerie).ToList()
                    }).ToList()
                };

                if (venta.TipoVenta == "Credito")
                {
                    var creditos = await _creditoService.ListarCredito();
                    var credito = creditos.FirstOrDefault(c => c.IdVenta == id);
                    if (credito != null)
                    {
                        dtoCompleto.Credito = credito;
                        var cuotas = await _cuotaService.ListarCuotaCredito();
                        dtoCompleto.Cuotas = cuotas.Where(c => c.IdCredito == credito.IdCredito).OrderBy(c => c.NumeroCuota).ToList();
                    }
                }

                return View(dtoCompleto);
            }
            catch (Exception ex)
            {
                TempData["Error"] = MensajeErrorHelper.ObtenerMensajeAmigable(ex, "cargar la factura");
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: /Ventas/Anular
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Anular(int id, string motivo)
        {
            int idUsuario = 1;
            var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(claimId, out int parsedId))
            {
                idUsuario = parsedId;
            }

            var res = await _ventaService.AnularVenta(id, idUsuario, motivo);
            return Json(new { success = res.Success, message = res.Message });
        }
    }
}

