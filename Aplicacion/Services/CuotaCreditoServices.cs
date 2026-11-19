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
    public class CuotaCreditoServices
    {
        private readonly ICuotaCredito _repo;

        public CuotaCreditoServices(ICuotaCredito reposi)
        {
            _repo = reposi;
        }

        public async Task<IEnumerable<DTOSCuotaCredito>> ListarCuotaCredito()
        {
            var Lista = await _repo.ListarCuotaCreditoAsync();

            return Lista.Select(li => new DTOSCuotaCredito
            {
                IdCuota = li.IdCuota,
                IdCredito = li.IdCredito,
                NumeroCuota = li.NumeroCuota,
                FechaVencimiento = li.FechaVencimiento,
                MontoCuota = li.MontoCuota,
                RecargoMora = li.RecargoMora,
                MoraAbonada = li.MoraAbonada,
                CapitalInteresAbonado = li.CapitalInteresAbonado,
                MontoAbonado = li.MontoAbonado,
                SaldoCuota = li.SaldoCuota,
                Estado = li.Estado
            });
        }

        public async Task NuevoCuotaCredito(DTOSCuotaCredito dt)
        {
            var nc = new CuotaCredito
            {
                IdCuota = dt.IdCuota,
                IdCredito = dt.IdCredito,
                NumeroCuota = dt.NumeroCuota,
                FechaVencimiento = dt.FechaVencimiento,
                MontoCuota = dt.MontoCuota,
                RecargoMora = dt.RecargoMora,
                MoraAbonada = dt.MoraAbonada,
                CapitalInteresAbonado = dt.CapitalInteresAbonado,
                MontoAbonado = dt.MontoAbonado,
                SaldoCuota = dt.SaldoCuota,
                Estado = dt.Estado
            };
            await _repo.NuevoCuotaCreditoAsync(nc);
        }

        public async Task EditarCuotaCredito(DTOSCuotaCredito dt)
        {
            var ec = new CuotaCredito
            {
                IdCuota = dt.IdCuota,
                IdCredito = dt.IdCredito,
                NumeroCuota = dt.NumeroCuota,
                FechaVencimiento = dt.FechaVencimiento,
                MontoCuota = dt.MontoCuota,
                RecargoMora = dt.RecargoMora,
                MoraAbonada = dt.MoraAbonada,
                CapitalInteresAbonado = dt.CapitalInteresAbonado,
                MontoAbonado = dt.MontoAbonado,
                SaldoCuota = dt.SaldoCuota,
                Estado = dt.Estado
            };
            await _repo.EditarCuotaCreditoAsync(ec);
        }

        public async Task EliminarCuotaCredito(int id)
        {
            await _repo.EliminarCuotaCreditoAsync(id);
        }

        public async Task<(bool Success, string Message)> AplicarMora(int idCuota, decimal recargoMora)
        {
            return await _repo.AplicarMoraAsync(idCuota, recargoMora);
        }
    }
}
