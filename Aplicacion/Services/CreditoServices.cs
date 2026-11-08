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
    public class CreditoServices
    {
        private readonly ICredito _repo;

        public CreditoServices(ICredito reposi)
        {
            _repo = reposi;
        }

        public async Task<IEnumerable<DTOSCredito>> ListarCredito()
        {
            var Lista = await _repo.ListarCreditoAsync();

            return Lista.Select(li => new DTOSCredito
            {
                IdCredito = li.IdCredito,
                IdVenta = li.IdVenta,
                MontoVentaTotal = li.MontoVentaTotal,
                PrimaInicial = li.PrimaInicial,
                MontoFinanciado = li.MontoFinanciado,
                FrecuenciaPago = li.FrecuenciaPago,
                CantidadCuotas = li.CantidadCuotas,
                PorcentajeInteres = li.PorcentajeInteres,
                MontoInteresesPactados = li.MontoInteresesPactados,
                MontoTotalCredito = li.MontoTotalCredito,
                SaldoPendiente = li.SaldoPendiente,
                Estado = li.Estado,
                FechaRegistro = li.FechaRegistro
            });
        }

        public async Task NuevoCredito(DTOSCredito dt)
        {
            var nc = new Credito
            {
                IdCredito = dt.IdCredito,
                IdVenta = dt.IdVenta,
                MontoVentaTotal = dt.MontoVentaTotal,
                PrimaInicial = dt.PrimaInicial,
                MontoFinanciado = dt.MontoFinanciado,
                FrecuenciaPago = dt.FrecuenciaPago,
                CantidadCuotas = dt.CantidadCuotas,
                PorcentajeInteres = dt.PorcentajeInteres,
                MontoInteresesPactados = dt.MontoInteresesPactados,
                MontoTotalCredito = dt.MontoTotalCredito,
                SaldoPendiente = dt.SaldoPendiente,
                Estado = dt.Estado,
                FechaRegistro = dt.FechaRegistro
            };
            await _repo.NuevoCreditoAsync(nc);
        }

        public async Task EditarCredito(DTOSCredito dt)
        {
            var ec = new Credito
            {
                IdCredito = dt.IdCredito,
                IdVenta = dt.IdVenta,
                MontoVentaTotal = dt.MontoVentaTotal,
                PrimaInicial = dt.PrimaInicial,
                MontoFinanciado = dt.MontoFinanciado,
                FrecuenciaPago = dt.FrecuenciaPago,
                CantidadCuotas = dt.CantidadCuotas,
                PorcentajeInteres = dt.PorcentajeInteres,
                MontoInteresesPactados = dt.MontoInteresesPactados,
                MontoTotalCredito = dt.MontoTotalCredito,
                SaldoPendiente = dt.SaldoPendiente,
                Estado = dt.Estado,
                FechaRegistro = dt.FechaRegistro
            };
            await _repo.EditarCreditoAsync(ec);
        }

        public async Task EliminarCredito(int id)
        {
            await _repo.EliminarCreditoAsync(id);
        }
    }
}
