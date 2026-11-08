using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Interface
{
    public  interface IRol
    {
        Task<IEnumerable<Roles>> ListarRolAsync();
        Task NuevoRolAsync(Roles rol);
        Task EditarRolAsync(Roles rol);
        Task EliminarRolAsync(int Id);
    }
}
