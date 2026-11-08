using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Interface
{
    public interface ICredito
    {
        Task<IEnumerable<Credito>> ListarCreditoAsync();
        Task NuevoCreditoAsync(Credito credito);
        Task EditarCreditoAsync(Credito credito);
        Task EliminarCreditoAsync(int Id);
    }
}
