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
    public class DetalleVentaSerieServices
    {
        private readonly IDetalleVentaSerie _repo;

        public DetalleVentaSerieServices(IDetalleVentaSerie reposi)
        {
            _repo = reposi;
        }

        public async Task<IEnumerable<DTOSDetalleVentaSerie>> ListarDetalleVentaSerie(int? idDetalleVenta = null)
        {
            var Lista = await _repo.ListarDetalleVentaSerieAsync(idDetalleVenta);

            return Lista.Select(li => new DTOSDetalleVentaSerie
            {
                IdSerieVendida = li.IdSerieVendida,
                IdDetalleVenta = li.IdDetalleVenta,
                NumeroSerie = li.NumeroSerie,
                EstadoSerie = li.EstadoSerie,
                FechaRegistro = li.FechaRegistro
            });
        }

        public async Task NuevoDetalleVentaSerie(DTOSDetalleVentaSerie dt)
        {
            var ns = new DetalleVentaSerie
            {
                IdSerieVendida = dt.IdSerieVendida,
                IdDetalleVenta = dt.IdDetalleVenta,
                NumeroSerie = dt.NumeroSerie,
                EstadoSerie = dt.EstadoSerie,
                FechaRegistro = dt.FechaRegistro
            };
            await _repo.NuevoDetalleVentaSerieAsync(ns);
        }

        public async Task EditarDetalleVentaSerie(DTOSDetalleVentaSerie dt)
        {
            var es = new DetalleVentaSerie
            {
                IdSerieVendida = dt.IdSerieVendida,
                IdDetalleVenta = dt.IdDetalleVenta,
                NumeroSerie = dt.NumeroSerie,
                EstadoSerie = dt.EstadoSerie,
                FechaRegistro = dt.FechaRegistro
            };
            await _repo.EditarDetalleVentaSerieAsync(es);
        }

        public async Task EliminarDetalleVentaSerie(int id)
        {
            await _repo.EliminarDetalleVentaSerieAsync(id);
        }
    }
}
