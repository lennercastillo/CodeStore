using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Interface
{
    public interface IClientes
    {
        Task<IEnumerable<Clientes>> ListarClienteAsync();
        Task NuevoClienteAsync(Clientes cliente);
        Task EditarClienteAsync(Clientes cliente);
        Task EliminarClienteAsync(int Id);
    }
}
