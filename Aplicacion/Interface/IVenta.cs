using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Interface
{
    public interface IVenta
    {
        Task<IEnumerable<Venta>> ListarVentaAsync();
        Task NuevoVentaAsync(Venta venta);
        Task EditarVentaAsync(Venta venta);
        Task EliminarVentaAsync(int Id);
        Task<(bool Success, string Message, int IdVenta, string NumeroFactura)> RegistrarVentaTransaccionalAsync(
            Venta venta,
            List<DetalleVenta> detalles,
            List<DetalleVentaSerie> series,
            Credito? credito,
            List<CuotaCredito>? cuotas);
        Task<Venta?> ObtenerPorIdAsync(int id);
        Task<(bool Success, string Message)> AnularVentaAsync(int idVenta, int idUsuarioAutoriza, string motivo);
    }
}
