using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Interface
{
    public interface IDetalleVentaSerie
    {
        Task<IEnumerable<DetalleVentaSerie>> ListarDetalleVentaSerieAsync(int? idDetalleVenta = null);
        Task NuevoDetalleVentaSerieAsync(DetalleVentaSerie serie);
        Task EditarDetalleVentaSerieAsync(DetalleVentaSerie serie);
        Task EliminarDetalleVentaSerieAsync(int Id);
    }
}
