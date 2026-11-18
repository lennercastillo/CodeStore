using Aplicacion.DTOs;
using Aplicacion.Interface;
using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Services
{
    public class AbonoServices
    {
        private readonly IAbono _repo;

        public AbonoServices(IAbono reposi)
        {
            _repo = reposi;
        }

        public async Task<IEnumerable<DTOSAbono>> ListarAbono()
        {
            var Lista = await _repo.ListarAbonoAsync();

            return Lista.Select(li => new DTOSAbono
            {
                IdAbono = li.IdAbono,
                NumeroRecibo = li.NumeroRecibo,
                IdCuota = li.IdCuota,
                IdUsuarioRegistro = li.IdUsuarioRegistro,
                FechaAbono = li.FechaAbono,
                MontoAbono = li.MontoAbono,
                MontoAplicadoMora = li.MontoAplicadoMora,
                MontoAplicadoCapital = li.MontoAplicadoCapital,
                MetodoPago = li.MetodoPago,
                Observaciones = li.Observaciones,
                Estado = li.Estado,
                IdUsuarioAnulacion = li.IdUsuarioAnulacion,
                FechaAnulacion = li.FechaAnulacion,
                MotivoAnulacion = li.MotivoAnulacion
            });
        }

        public async Task NuevoAbono(DTOSAbono dt)
        {
            var na = new Abono
            {
                IdAbono = dt.IdAbono,
                NumeroRecibo = dt.NumeroRecibo,
                IdCuota = dt.IdCuota,
                IdUsuarioRegistro = dt.IdUsuarioRegistro,
                FechaAbono = dt.FechaAbono,
                MontoAbono = dt.MontoAbono,
                MontoAplicadoMora = dt.MontoAplicadoMora,
                MontoAplicadoCapital = dt.MontoAplicadoCapital,
                MetodoPago = dt.MetodoPago,
                Observaciones = dt.Observaciones,
                Estado = dt.Estado,
                IdUsuarioAnulacion = dt.IdUsuarioAnulacion,
                FechaAnulacion = dt.FechaAnulacion,
                MotivoAnulacion = dt.MotivoAnulacion
            };
            await _repo.NuevoAbonoAsync(na);
        }

        public async Task EditarAbono(DTOSAbono dt)
        {
            var ea = new Abono
            {
                IdAbono = dt.IdAbono,
                NumeroRecibo = dt.NumeroRecibo,
                IdCuota = dt.IdCuota,
                IdUsuarioRegistro = dt.IdUsuarioRegistro,
                FechaAbono = dt.FechaAbono,
                MontoAbono = dt.MontoAbono,
                MontoAplicadoMora = dt.MontoAplicadoMora,
                MontoAplicadoCapital = dt.MontoAplicadoCapital,
                MetodoPago = dt.MetodoPago,
                Observaciones = dt.Observaciones,
                Estado = dt.Estado,
                IdUsuarioAnulacion = dt.IdUsuarioAnulacion,
                FechaAnulacion = dt.FechaAnulacion,
                MotivoAnulacion = dt.MotivoAnulacion
            };
            await _repo.EditarAbonoAsync(ea);
        }

        public async Task EliminarAbono(int id)
        {
            await _repo.EliminarAbonoAsync(id);
        }

        public async Task<(bool Success, string Message, int IdAbono, string NumeroRecibo)> RegistrarAbono(
            int idCuota,
            decimal montoAbono,
            string metodoPago,
            string? observaciones,
            int idUsuario)
        {
            return await _repo.RegistrarAbonoTransaccionalAsync(idCuota, montoAbono, metodoPago, observaciones, idUsuario);
        }
    }
}
