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
    }
}
