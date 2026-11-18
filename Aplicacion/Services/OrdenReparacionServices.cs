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
    public class OrdenReparacionServices
    {
        private readonly IOrdenReparacion _repo;

        public OrdenReparacionServices(IOrdenReparacion reposi)
        {
            _repo = reposi;
        }

        public async Task<IEnumerable<DTOSOrdenReparacion>> ListarOrdenReparacion()
        {
            var Lista = await _repo.ListarOrdenReparacionAsync();

            return Lista.Select(li => new DTOSOrdenReparacion
            {
                IdOrdenReparacion = li.IdOrdenReparacion,
                NumeroOrden = li.NumeroOrden,
                IdGarantia = li.IdGarantia,
                IdCliente = li.IdCliente,
                IdUsuarioTecnico = li.IdUsuarioTecnico,
                DescripcionEquipo = li.DescripcionEquipo,
                Marca = li.Marca,
                Modelo = li.Modelo,
                NumeroSerie = li.NumeroSerie,
                DiagnosticoTecnico = li.DiagnosticoTecnico,
                EstadoEquipo = li.EstadoEquipo,
                CostoReparacion = li.CostoReparacion,
                FechaIngreso = li.FechaIngreso,
                FechaFinalizacion = li.FechaFinalizacion,
                FechaEntrega = li.FechaEntrega,
                Observaciones = li.Observaciones
            });
        }

        public async Task NuevoOrdenReparacion(DTOSOrdenReparacion dt)
        {
            var no = new OrdenReparacion
            {
                IdOrdenReparacion = dt.IdOrdenReparacion,
                NumeroOrden = dt.NumeroOrden,
                IdGarantia = dt.IdGarantia,
                IdCliente = dt.IdCliente,
                IdUsuarioTecnico = dt.IdUsuarioTecnico,
                DescripcionEquipo = dt.DescripcionEquipo,
                Marca = dt.Marca,
                Modelo = dt.Modelo,
                NumeroSerie = dt.NumeroSerie,
                DiagnosticoTecnico = dt.DiagnosticoTecnico,
                EstadoEquipo = dt.EstadoEquipo,
                CostoReparacion = dt.CostoReparacion,
                FechaIngreso = dt.FechaIngreso,
                FechaFinalizacion = dt.FechaFinalizacion,
                FechaEntrega = dt.FechaEntrega,
                Observaciones = dt.Observaciones
            };
            await _repo.NuevoOrdenReparacionAsync(no);
        }

        public async Task EditarOrdenReparacion(DTOSOrdenReparacion dt)
        {
            var eo = new OrdenReparacion
            {
                IdOrdenReparacion = dt.IdOrdenReparacion,
                NumeroOrden = dt.NumeroOrden,
                IdGarantia = dt.IdGarantia,
                IdCliente = dt.IdCliente,
                IdUsuarioTecnico = dt.IdUsuarioTecnico,
                DescripcionEquipo = dt.DescripcionEquipo,
                Marca = dt.Marca,
                Modelo = dt.Modelo,
                NumeroSerie = dt.NumeroSerie,
                DiagnosticoTecnico = dt.DiagnosticoTecnico,
                EstadoEquipo = dt.EstadoEquipo,
                CostoReparacion = dt.CostoReparacion,
                FechaIngreso = dt.FechaIngreso,
                FechaFinalizacion = dt.FechaFinalizacion,
                FechaEntrega = dt.FechaEntrega,
                Observaciones = dt.Observaciones
            };
            await _repo.EditarOrdenReparacionAsync(eo);
        }

        public async Task EliminarOrdenReparacion(int id)
        {
            await _repo.EliminarOrdenReparacionAsync(id);
        }

        public async Task<DTOSOrdenReparacion?> ObtenerPorId(int id)
        {
            var lista = await ListarOrdenReparacion();
            return lista.FirstOrDefault(o => o.IdOrdenReparacion == id);
        }
    }
}
