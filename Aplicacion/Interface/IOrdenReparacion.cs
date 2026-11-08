using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Interface
{
    public interface IOrdenReparacion
    {
        Task<IEnumerable<OrdenReparacion>> ListarOrdenReparacionAsync();
        Task NuevoOrdenReparacionAsync(OrdenReparacion orden);
        Task EditarOrdenReparacionAsync(OrdenReparacion orden);
        Task EliminarOrdenReparacionAsync(int Id);
    }
}
