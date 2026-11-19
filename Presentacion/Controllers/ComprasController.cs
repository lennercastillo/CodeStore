using Aplicacion.DTOs;
using Aplicacion.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentacion.Helpers;
using System.Security.Claims;

namespace Presentacion.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ComprasController : Controller
    {
        private readonly CompraServices _compraService;
        private readonly DetalleCompraServices _detalleCompraService;
        private readonly ProveedorServices _proveedorService;
        private readonly ProductosServices _productosService;

        public ComprasController(
            CompraServices compraService,
            DetalleCompraServices detalleCompraService,
            ProveedorServices proveedorService,
            ProductosServices productosService)
        {
            _compraService = compraService;
            _detalleCompraService = detalleCompraService;
            _proveedorService = proveedorService;
            _productosService = productosService;
        }

        // GET: /Compras
        public async Task<IActionResult> Index()
        {
            try
            {
                var compras = await _compraService.ListarCompra();
                var proveedores = await _proveedorService.ListarProveedor();
                ViewBag.Proveedores = proveedores.ToDictionary(p => p.IdProveedor, p => p.RazonSocial);
                return View(compras.OrderByDescending(c => c.FechaCompra));
            }
            catch (Exception ex)
            {
                TempData["Error"] = MensajeErrorHelper.ObtenerMensajeAmigable(ex, "cargar las compras");
                return View(Enumerable.Empty<DTOSCompra>());
            }
        }

        // GET: /Compras/Crear
        public async Task<IActionResult> Crear()
        {
            try
            {
                var proveedores = await _proveedorService.ListarProveedor();
                var productos = await _productosService.ListarProducto();

                ViewBag.Proveedores = proveedores.Where(p => p.Activo).ToList();
                ViewBag.Productos = productos.Where(p => p.Activo).ToList();

                return View(new CompraRegistroDTO());
            }
            catch (Exception ex)
            {
                TempData["Error"] = MensajeErrorHelper.ObtenerMensajeAmigable(ex, "cargar el formulario de compra");
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: /Compras/RegistrarCompra (AJAX)
        [HttpPost]
        public async Task<IActionResult> RegistrarCompra([FromBody] CompraRegistroDTO model)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return Json(new { success = false, message = errors });
            }

            int idUsuario = ObtenerIdUsuarioActual();
            var res = await _compraService.RegistrarCompraCompleta(model, idUsuario);

            return Json(new
            {
                success = res.Success,
                message = res.Message,
                idCompra = res.IdCompra,
                numeroDocumento = res.NumeroDocumento
            });
        }

        // GET: /Compras/Detalle/5
        public async Task<IActionResult> Detalle(int id)
        {
            try
            {
                var compra = await _compraService.ObtenerPorId(id);
                if (compra == null)
                {
                    TempData["Error"] = "La compra solicitada no existe.";
                    return RedirectToAction(nameof(Index));
                }

                var proveedor = await _proveedorService.ObtenerPorId(compra.IdProveedor);
                var todosDetalles = await _detalleCompraService.ListarDetalleCompra();
                var detalles = todosDetalles.Where(d => d.IdCompra == id).ToList();
                var productos = await _productosService.ListarProducto();
                var dictProductos = productos.ToDictionary(p => p.IdProducto, p => p);

                var dto = new CompraDetalleCompletoDTO
                {
                    Compra = new DTOSCompra
                    {
                        IdCompra = compra.IdCompra,
                        NumeroDocumento = compra.NumeroDocumento,
                        IdProveedor = compra.IdProveedor,
                        IdUsuarioRegistro = compra.IdUsuarioRegistro,
                        IdUsuarioRecepcion = compra.IdUsuarioRecepcion,
                        FechaCompra = compra.FechaCompra,
                        FechaRecepcion = compra.FechaRecepcion,
                        TotalCompra = compra.TotalCompra,
                        Estado = compra.Estado,
                        Observaciones = compra.Observaciones,
                        IdUsuarioAnulacion = compra.IdUsuarioAnulacion,
                        FechaAnulacion = compra.FechaAnulacion,
                        MotivoAnulacion = compra.MotivoAnulacion
                    },
                    Proveedor = proveedor,
                    Detalles = detalles.Select(d => new CompraItemDetalleDTO
                    {
                        Detalle = d,
                        Producto = dictProductos.ContainsKey(d.IdProducto) ? dictProductos[d.IdProducto] : null
                    }).ToList()
                };

                return View(dto);
            }
            catch (Exception ex)
            {
                TempData["Error"] = MensajeErrorHelper.ObtenerMensajeAmigable(ex, "cargar el detalle de la compra");
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: /Compras/ConfirmarRecepcion/5
        [HttpPost]
        public async Task<IActionResult> ConfirmarRecepcion(int id)
        {
            int idUsuario = ObtenerIdUsuarioActual();
            var res = await _compraService.ConfirmarRecepcion(id, idUsuario);
            return Json(new { success = res.Success, message = res.Message });
        }

        // POST: /Compras/Anular
        [HttpPost]
        public async Task<IActionResult> Anular(int id, string motivo)
        {
            int idUsuario = ObtenerIdUsuarioActual();
            var res = await _compraService.AnularCompra(id, idUsuario, motivo);
            return Json(new { success = res.Success, message = res.Message });
        }

        private int ObtenerIdUsuarioActual()
        {
            var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(claimId, out int parsedId) ? parsedId : 1;
        }
    }
}

