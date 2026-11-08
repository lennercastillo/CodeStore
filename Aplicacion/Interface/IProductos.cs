using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Interface
{
    public interface IProductos
    {
        Task<IEnumerable<Productos>> ListarProductoAsync();
        Task NuevoProductoAsync(Productos producto);
        Task EditarProductoAsync(Productos producto);
        Task EliminarProductoAsync(int Id);
    }
}
