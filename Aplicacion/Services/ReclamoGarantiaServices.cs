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
    public class ReclamoGarantiaServices
    {
        private readonly IReclamoGarantia _repo;

        public ReclamoGarantiaServices(IReclamoGarantia reposi)
        {
            _repo = reposi;
        }

        public async Task<IEnumerable<DTOSReclamoGarantia>> ListarReclamoGarantia()
        {
            var Lista = await _repo.ListarReclamoGarantiaAsync();

            return Lista.Select(li => new DTOSReclamoGarantia
            {
                IdGarantia = li.IdGarantia,
                NumeroReclamo = li.NumeroReclamo,
                IdDetalleVenta = li.IdDetalleVenta,
                IdSerieVendida = li.IdSerieVendida,
                IdUsuarioTecnico = li.IdUsuarioTecnico,
                FechaReclamo = li.FechaReclamo,
                MotivoFalla = li.MotivoFalla,
                EstadoRevision = li.EstadoRevision,
                AccionTomada = li.AccionTomada,
                DescripcionResolucion = li.DescripcionResolucion,
                FechaResolucion = li.FechaResolucion
            });
        }

        public async Task NuevoReclamoGarantia(DTOSReclamoGarantia dt)
        {
            var nr = new ReclamoGarantia
            {
                IdGarantia = dt.IdGarantia,
                NumeroReclamo = dt.NumeroReclamo,
                IdDetalleVenta = dt.IdDetalleVenta,
                IdSerieVendida = dt.IdSerieVendida,
                IdUsuarioTecnico = dt.IdUsuarioTecnico,
                FechaReclamo = dt.FechaReclamo,
                MotivoFalla = dt.MotivoFalla,
                EstadoRevision = dt.EstadoRevision,
                AccionTomada = string.IsNullOrWhiteSpace(dt.AccionTomada) ? null : dt.AccionTomada.Trim(),
                DescripcionResolucion = string.IsNullOrWhiteSpace(dt.DescripcionResolucion) ? null : dt.DescripcionResolucion.Trim(),
                FechaResolucion = dt.FechaResolucion
            };
            await _repo.NuevoReclamoGarantiaAsync(nr);
        }

        public async Task EditarReclamoGarantia(DTOSReclamoGarantia dt)
        {
            var er = new ReclamoGarantia
            {
                IdGarantia = dt.IdGarantia,
                NumeroReclamo = dt.NumeroReclamo,
                IdDetalleVenta = dt.IdDetalleVenta,
                IdSerieVendida = dt.IdSerieVendida,
                IdUsuarioTecnico = dt.IdUsuarioTecnico,
                FechaReclamo = dt.FechaReclamo,
                MotivoFalla = dt.MotivoFalla,
                EstadoRevision = dt.EstadoRevision,
                AccionTomada = string.IsNullOrWhiteSpace(dt.AccionTomada) ? null : dt.AccionTomada.Trim(),
                DescripcionResolucion = string.IsNullOrWhiteSpace(dt.DescripcionResolucion) ? null : dt.DescripcionResolucion.Trim(),
                FechaResolucion = dt.FechaResolucion
            };
            await _repo.EditarReclamoGarantiaAsync(er);
        }

        public async Task EliminarReclamoGarantia(int id)
        {
            await _repo.EliminarReclamoGarantiaAsync(id);
        }

        public async Task<DTOSReclamoGarantia?> ObtenerPorId(int id)
        {
            var lista = await ListarReclamoGarantia();
            return lista.FirstOrDefault(r => r.IdGarantia == id);
        }
    }
}
