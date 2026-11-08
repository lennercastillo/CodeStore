using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Interface
{
    public interface IReclamoGarantia
    {
        Task<IEnumerable<ReclamoGarantia>> ListarReclamoGarantiaAsync();
        Task NuevoReclamoGarantiaAsync(ReclamoGarantia reclamo);
        Task EditarReclamoGarantiaAsync(ReclamoGarantia reclamo);
        Task EliminarReclamoGarantiaAsync(int Id);
    }
}
