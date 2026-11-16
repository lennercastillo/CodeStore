using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Interface
{
    public interface IKardex
    {
        Task<IEnumerable<Kardex>> ListarKardexAsync();
        Task NuevoKardexAsync(Kardex kardex);
        Task EditarKardexAsync(Kardex kardex);
        Task EliminarKardexAsync(int Id);
    }
}
