using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Interface
{
    public interface IDetalleVenta
    {
        Task<IEnumerable<DetalleVenta>> ListarDetalleVentaAsync(int? idVenta = null);
        Task NuevoDetalleVentaAsync(DetalleVenta detalle);
        Task EditarDetalleVentaAsync(DetalleVenta detalle);
        Task EliminarDetalleVentaAsync(int Id);
    }
}
