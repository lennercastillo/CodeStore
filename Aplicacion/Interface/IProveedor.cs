using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Interface
{
    public interface IProveedor
    {
        Task<IEnumerable<Proveedor>> ListarProveedorAsync();
        Task NuevoProveedorAsync(Proveedor proveedor);
        Task EditarProveedorAsync(Proveedor proveedor);
        Task EliminarProveedorAsync(int Id);
    }
}
