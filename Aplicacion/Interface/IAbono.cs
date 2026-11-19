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
        Task<IEnumerable<Abono>> ListarAbonoAsync();
        Task NuevoAbonoAsync(Abono abono);
        Task EditarAbonoAsync(Abono abono);
        Task EliminarAbonoAsync(int Id);
        Task<(bool Success, string Message, int IdAbono, string NumeroRecibo)> RegistrarAbonoTransaccionalAsync(
            int idCuota,
            decimal montoAbono,
            string metodoPago,
            string? observaciones,
            int idUsuarioRegistro);
        Task<(bool Success, string Message)> AnularAbonoTransaccionalAsync(int idAbono, int idUsuarioAnulacion, string motivo);
    }
}
