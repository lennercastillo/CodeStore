using Aplicacion.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentacion.Models;
using System.Diagnostics;

namespace Presentacion.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly VentaServices _ventaService;
        private readonly Clientes_Services _clientesService;
        private readonly ProductosServices _productosService;
        private readonly OrdenReparacionServices _ordenService;
        private readonly CreditoServices _creditoService;

        public HomeController(
            ILogger<HomeController> logger,
            VentaServices ventaService,
            Clientes_Services clientesService,
            ProductosServices productosService,
            OrdenReparacionServices ordenService,
            CreditoServices creditoService)
        {
            _logger = logger;
            _ventaService = ventaService;
            _clientesService = clientesService;
            _productosService = productosService;
            _ordenService = ordenService;
            _creditoService = creditoService;
        }

        public async Task<IActionResult> Index()
        {
            var vm = new DashboardViewModel();
            try
            {
                var ventas = (await _ventaService.ListarVenta()).ToList();
                var clientes = (await _clientesService.ListaClientes()).ToList();
                var productos = (await _productosService.ListarProducto()).ToList();
                var ordenes = (await _ordenService.ListarOrdenReparacion()).ToList();
                var creditos = (await _creditoService.ListarCredito()).ToList();

                var inicioMes = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                var ventasMes = ventas.Where(v => v.Estado == "Completada" && v.FechaVenta >= inicioMes).ToList();

                vm.VentasDelMes = ventasMes.Sum(v => v.TotalVenta);
                vm.CantidadVentasMes = ventasMes.Count;
                vm.TotalClientesActivos = clientes.Count(c => c.Activo);
                vm.EquiposEnTaller = ordenes.Count(o => o.EstadoEquipo != "Entregado" && o.EstadoEquipo != "Sin Solucion");
                vm.ProductosStockBajo = productos.Count(p => p.Activo && p.StockActual <= p.StockMinimo);
                vm.CarteraCreditosPendiente = creditos.Where(c => c.Estado != "Anulado").Sum(c => c.SaldoPendiente);

                vm.ClientesNombres = clientes.ToDictionary(c => c.IdCliente, c => $"{c.Nombres} {c.Apellidos}");
                vm.UltimasVentas = ventas.OrderByDescending(v => v.FechaVenta).Take(6).ToList();
                vm.ProductosCriticos = productos.Where(p => p.Activo && p.StockActual <= p.StockMinimo).OrderBy(p => p.StockActual).Take(6).ToList();
                vm.OrdenesRecientes = ordenes.OrderByDescending(o => o.FechaIngreso).Take(5).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar métricas del Dashboard");
                TempData["Error"] = "No se pudieron cargar algunas métricas de la base de datos. Verifique la conexión.";
            }

            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
