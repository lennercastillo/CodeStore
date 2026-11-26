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
        private readonly UsuarioServices _usuarioService;

        public VentasController(
            VentaServices ventaService,
            ProductosServices productosService,
            Clientes_Services clientesService,
            DetalleVentaServices detalleVentaService,
            DetalleVentaSerieServices serieService,
            CreditoServices creditoService,
            CuotaCreditoServices cuotaService,
            UsuarioServices usuarioService)
        {
            _ventaService = ventaService;
            _productosService = productosService;
            _clientesService = clientesService;
            _detalleVentaService = detalleVentaService;
            _serieService = serieService;
            _creditoService = creditoService;
            _cuotaService = cuotaService;
            _usuarioService = usuarioService;
        }

        // GET: /Ventas
        public async Task<IActionResult> Index()
        {
            try
            {
                var ventas = await _ventaService.ListarVenta();
                var clientes = await _clientesService.ListaClientes();
                var usuarios = await _usuarioService.ListaRoles();
                ViewBag.Clientes = clientes.ToDictionary(c => c.IdCliente, c => $"{c.Nombres} {c.Apellidos}");
                ViewBag.Vendedores = usuarios.ToDictionary(u => u.IdUsuario, u => $"{u.Nombres} {u.Apellidos}");
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarVenta([FromBody] VentaRegistroDTO? model)
        {
            if (model == null)
                return Json(new { success = false, message = "No se recibieron los datos de la venta." });

            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return Json(new { success = false, message = errors });
            }

            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int idUsuario))
            {
                return Unauthorized(new { success = false, message = "Sesión expirada. Vuelve a iniciar sesión." });
            }

            try
            {
                var resultado = await _ventaService.RegistrarVentaCompleta(model, idUsuario);
                string msg = resultado.Success
                    ? resultado.Message
                    : MensajeErrorHelper.ObtenerMensajeDesdeTexto(resultado.Message, "registrar la venta");

                return Json(new
                {
                    success = resultado.Success,
                    message = msg,
                    idVenta = resultado.IdVenta,
                    numeroFactura = resultado.NumeroFactura
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = MensajeErrorHelper.ObtenerMensajeAmigable(ex, "registrar la venta") });
            }
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
                var vendedor = await _usuarioService.ObtenerPorId(venta.IdUsuarioVendedor);
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
                        IdUsuarioAutorizaAnulacion = venta.IdUsuarioAutorizaAnulacion,
                        FechaAnulacion = venta.FechaAnulacion,
                        MotivoAnulacion = venta.MotivoAnulacion
                    },
                    Cliente = cliente,
                    Vendedor = vendedor,
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
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Anular(int id, string motivo)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int idUsuario))
            {
                return Unauthorized(new { success = false, message = "Sesión expirada. Vuelve a iniciar sesión." });
            }

            try
            {
                var res = await _ventaService.AnularVenta(id, idUsuario, motivo);
                string msg = res.Success ? res.Message : MensajeErrorHelper.ObtenerMensajeDesdeTexto(res.Message, "anular la venta");
                return Json(new { success = res.Success, message = msg });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = MensajeErrorHelper.ObtenerMensajeAmigable(ex, "anular la venta") });
            }
        }
    }
}

