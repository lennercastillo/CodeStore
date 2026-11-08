using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Interface
{
     public interface IUsuario
    {
        Task<IEnumerable<Usuario>> ListarUsuarioAsync();
        Task NuevoUsuarioAsync(Usuario usuario);
        Task EditarUsuarioAsync(Usuario usuario);
        Task EliminarUsuarioAsync(int Id);
    }
}
