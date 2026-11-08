using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Interface
{
    public interface ICuotaCredito
    {
        Task<IEnumerable<CuotaCredito>> ListarCuotaCreditoAsync(int? idCredito = null);
        Task NuevoCuotaCreditoAsync(CuotaCredito cuota);
        Task EditarCuotaCreditoAsync(CuotaCredito cuota);
        Task EliminarCuotaCreditoAsync(int Id);
    }
}
