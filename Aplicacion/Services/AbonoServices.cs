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
            if (idCuota <= 0)
                return (false, "Debe seleccionar una cuota válida.", 0, string.Empty);
            if (montoAbono <= 0)
                return (false, "El monto del abono debe ser mayor a cero.", 0, string.Empty);
            if (idUsuario <= 0)
                return (false, "Usuario inválido.", 0, string.Empty);

            string? metodoNorm = new[] { "Efectivo", "Tarjeta", "Transferencia" }
                .FirstOrDefault(m => string.Equals(m, metodoPago?.Trim(), StringComparison.OrdinalIgnoreCase));
            if (metodoNorm == null)
                return (false, "Método de pago no válido. Use Efectivo, Tarjeta o Transferencia.", 0, string.Empty);

            string? obsLimpia = string.IsNullOrWhiteSpace(observaciones) ? null : observaciones.Trim();
            if (obsLimpia != null && obsLimpia.Length > 255)
                obsLimpia = obsLimpia[..255];

            return await _repo.RegistrarAbonoTransaccionalAsync(idCuota, Math.Round(montoAbono, 2), metodoNorm, obsLimpia, idUsuario);
        }

        public async Task<(bool Success, string Message)> AnularAbono(int idAbono, int idUsuarioAnulacion, string motivo)
        {
            if (idAbono <= 0 || idUsuarioAnulacion <= 0)
                return (false, "Datos inválidos para anular el abono.");

            if (string.IsNullOrWhiteSpace(motivo))
                return (false, "Debe especificar el motivo de anulación.");

            string motivoLimpio = motivo.Trim();
            if (motivoLimpio.Length > 255)
                motivoLimpio = motivoLimpio[..255];

            return await _repo.AnularAbonoTransaccionalAsync(idAbono, idUsuarioAnulacion, motivoLimpio);
        }
    }
}
