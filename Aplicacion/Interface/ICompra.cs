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
    }
}
