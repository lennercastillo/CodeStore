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
    public class CompraServices
    {
        private readonly ICompra _repo;

        public CompraServices(ICompra reposi)
        {
            _repo = reposi;
        }

        public async Task<IEnumerable<DTOSCompra>> ListarCompra()
        {
            var Lista = await _repo.ListarCompraAsync();

            return Lista.Select(li => new DTOSCompra
            {
                IdCompra = li.IdCompra,
                NumeroDocumento = li.NumeroDocumento,
                IdProveedor = li.IdProveedor,
                IdUsuarioRegistro = li.IdUsuarioRegistro,
                IdUsuarioRecepcion = li.IdUsuarioRecepcion,
                FechaCompra = li.FechaCompra,
                FechaRecepcion = li.FechaRecepcion,
                TotalCompra = li.TotalCompra,
                Estado = li.Estado,
                Observaciones = li.Observaciones,
                IdUsuarioAnulacion = li.IdUsuarioAnulacion,
                FechaAnulacion = li.FechaAnulacion,
                MotivoAnulacion = li.MotivoAnulacion
            });
        }

        public async Task NuevoCompra(DTOSCompra dt)
        {
            var nc = new Compra
            {
                IdCompra = dt.IdCompra,
                NumeroDocumento = dt.NumeroDocumento,
                IdProveedor = dt.IdProveedor,
                IdUsuarioRegistro = dt.IdUsuarioRegistro,
                IdUsuarioRecepcion = dt.IdUsuarioRecepcion,
                FechaCompra = dt.FechaCompra,
                FechaRecepcion = dt.FechaRecepcion,
                TotalCompra = dt.TotalCompra,
                Estado = dt.Estado,
                Observaciones = dt.Observaciones,
                IdUsuarioAnulacion = dt.IdUsuarioAnulacion,
                FechaAnulacion = dt.FechaAnulacion,
                MotivoAnulacion = dt.MotivoAnulacion
            };
            await _repo.NuevoCompraAsync(nc);
        }

        public async Task EditarCompra(DTOSCompra dt)
        {
            var ec = new Compra
            {
                IdCompra = dt.IdCompra,
                NumeroDocumento = dt.NumeroDocumento,
                IdProveedor = dt.IdProveedor,
                IdUsuarioRegistro = dt.IdUsuarioRegistro,
                IdUsuarioRecepcion = dt.IdUsuarioRecepcion,
                FechaCompra = dt.FechaCompra,
                FechaRecepcion = dt.FechaRecepcion,
                TotalCompra = dt.TotalCompra,
                Estado = dt.Estado,
                Observaciones = dt.Observaciones,
                IdUsuarioAnulacion = dt.IdUsuarioAnulacion,
                FechaAnulacion = dt.FechaAnulacion,
                MotivoAnulacion = dt.MotivoAnulacion
            };
            await _repo.EditarCompraAsync(ec);
        }

        public async Task EliminarCompra(int id)
        {
            await _repo.EliminarCompraAsync(id);
        }

        public async Task<Compra?> ObtenerPorId(int id)
        {
            return await _repo.ObtenerPorIdAsync(id);
        }

        public async Task<(bool Success, string Message, int IdCompra, string NumeroDocumento)> RegistrarCompraCompleta(CompraRegistroDTO dto, int idUsuario)
        {
            if (idUsuario <= 0)
                return (false, "Usuario inválido.", 0, string.Empty);

            if (dto.Items == null || !dto.Items.Any())
                return (false, "La compra debe contener al menos un producto.", 0, string.Empty);

            if (dto.IdProveedor <= 0)
                return (false, "Debe seleccionar un proveedor válido.", 0, string.Empty);

            if (!string.IsNullOrWhiteSpace(dto.NumeroDocumento) && dto.NumeroDocumento.Trim().Length > 50)
                return (false, "El número de documento no puede exceder 50 caracteres.", 0, string.Empty);

            if (dto.Items.Any(i => i.IdProducto <= 0 || i.Cantidad <= 0 || i.CostoUnitario <= 0))
                return (false, "Todos los productos deben ser válidos, con cantidad y costo unitario mayores a cero.", 0, string.Empty);

            var detalles = dto.Items
                .GroupBy(i => i.IdProducto)
                .Select(g =>
                {
                    int cant = g.Sum(x => x.Cantidad);
                    decimal costo = Math.Round(g.Last().CostoUnitario, 2);
                    return new DetalleCompra
                    {
                        IdProducto = g.Key,
                        Cantidad = cant,
                        CostoUnitario = costo,
                        Subtotal = Math.Round(cant * costo, 2)
                    };
                }).ToList();

            decimal total = Math.Round(detalles.Sum(d => d.Subtotal), 2);

            var compra = new Compra
            {
                NumeroDocumento = dto.NumeroDocumento?.Trim() ?? string.Empty,
                IdProveedor = dto.IdProveedor,
                IdUsuarioRegistro = idUsuario,
                FechaCompra = DateTime.Now,
                TotalCompra = total,
                Estado = dto.RecibirInmediatamente ? "Recibida" : "Pendiente",
                Observaciones = string.IsNullOrWhiteSpace(dto.Observaciones) ? null : dto.Observaciones.Trim()
            };

            return await _repo.RegistrarCompraTransaccionalAsync(compra, detalles, dto.RecibirInmediatamente);
        }

        public async Task<(bool Success, string Message)> ConfirmarRecepcion(int idCompra, int idUsuarioRecepcion)
        {
            if (idCompra <= 0 || idUsuarioRecepcion <= 0)
                return (false, "Datos inválidos para confirmar recepción.");

            return await _repo.ConfirmarRecepcionAsync(idCompra, idUsuarioRecepcion);
        }

        public async Task<(bool Success, string Message)> AnularCompra(int idCompra, int idUsuarioAnulacion, string motivo)
        {
            if (idCompra <= 0 || idUsuarioAnulacion <= 0)
                return (false, "Datos inválidos para anular la compra.");

            if (string.IsNullOrWhiteSpace(motivo))
                return (false, "Debe especificar el motivo de anulación.");

            string motivoLimpio = motivo.Trim();
            if (motivoLimpio.Length > 255)
                motivoLimpio = motivoLimpio[..255];

            return await _repo.AnularCompraAsync(idCompra, idUsuarioAnulacion, motivoLimpio);
        }
    }
}
