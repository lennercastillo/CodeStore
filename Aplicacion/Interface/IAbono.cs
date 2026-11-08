using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Interface
{
    public interface IAbono
    {
        Task<IEnumerable<Abono>> ListarAbonoAsync(int? idCuota = null);
        Task NuevoAbonoAsync(Abono abono);
        Task EditarAbonoAsync(Abono abono);
        Task EliminarAbonoAsync(int Id);
    }
}
