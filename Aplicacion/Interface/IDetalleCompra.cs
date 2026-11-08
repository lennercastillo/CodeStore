using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Interface
{
    public interface IDetalleCompra
    {
        Task<IEnumerable<DetalleCompra>> ListarDetalleCompraAsync(int? idCompra = null);
        Task NuevoDetalleCompraAsync(DetalleCompra detalle);
        Task EditarDetalleCompraAsync(DetalleCompra detalle);
        Task EliminarDetalleCompraAsync(int Id);
    }
}
