using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Interface
{
    public interface ICompra
    {
        Task<IEnumerable<Compra>> ListarCompraAsync();
        Task NuevoCompraAsync(Compra compra);
        Task EditarCompraAsync(Compra compra);
        Task EliminarCompraAsync(int Id);
        Task<Compra?> ObtenerPorIdAsync(int id);
        Task<(bool Success, string Message, int IdCompra, string NumeroDocumento)> RegistrarCompraTransaccionalAsync(
            Compra compra,
            List<DetalleCompra> detalles,
            bool recibirInmediatamente);
        Task<(bool Success, string Message)> ConfirmarRecepcionAsync(int idCompra, int idUsuarioRecepcion);
        Task<(bool Success, string Message)> AnularCompraAsync(int idCompra, int idUsuarioAnulacion, string motivo);
    }
}
