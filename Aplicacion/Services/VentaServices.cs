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
    public class VentaServices
    {
        private readonly IVenta _repo;

        public VentaServices(IVenta reposi)
        {
            _repo = reposi;
        }

        public async Task<IEnumerable<DTOSVenta>> ListarVenta()
        {
            var Lista = await _repo.ListarVentaAsync();

            return Lista.Select(li => new DTOSVenta
            {
                IdVenta = li.IdVenta,
                NumeroFactura = li.NumeroFactura,
                IdCliente = li.IdCliente,
                IdUsuarioVendedor = li.IdUsuarioVendedor,
                FechaVenta = li.FechaVenta,
                TipoVenta = li.TipoVenta,
                MetodoPago = li.MetodoPago,
                PorcentajeImpuesto = li.PorcentajeImpuesto,
                Subtotal = li.Subtotal,
                Impuesto = li.Impuesto,
                TotalVenta = li.TotalVenta,
                MontoRecibido = li.MontoRecibido,
                Vuelto = li.Vuelto,
                Estado = li.Estado,
                IdUsuarioAutorizaAnulacion = li.IdUsuarioAutorizaAnulacion,
                FechaAnulacion = li.FechaAnulacion,
                MotivoAnulacion = li.MotivoAnulacion
            });
        }

        public async Task NuevoVenta(DTOSVenta dt)
        {
            var nv = new Venta
            {
                IdVenta = dt.IdVenta,
                NumeroFactura = dt.NumeroFactura,
                IdCliente = dt.IdCliente,
                IdUsuarioVendedor = dt.IdUsuarioVendedor,
                FechaVenta = dt.FechaVenta,
                TipoVenta = dt.TipoVenta,
                MetodoPago = dt.MetodoPago,
                PorcentajeImpuesto = dt.PorcentajeImpuesto,
                Subtotal = dt.Subtotal,
                Impuesto = dt.Impuesto,
                TotalVenta = dt.TotalVenta,
                MontoRecibido = dt.MontoRecibido,
                Vuelto = dt.Vuelto,
                Estado = dt.Estado,
                IdUsuarioAutorizaAnulacion = dt.IdUsuarioAutorizaAnulacion,
                FechaAnulacion = dt.FechaAnulacion,
                MotivoAnulacion = dt.MotivoAnulacion
            };
            await _repo.NuevoVentaAsync(nv);
        }

        public async Task EditarVenta(DTOSVenta dt)
        {
            var ev = new Venta
            {
                IdVenta = dt.IdVenta,
                NumeroFactura = dt.NumeroFactura,
                IdCliente = dt.IdCliente,
                IdUsuarioVendedor = dt.IdUsuarioVendedor,
                FechaVenta = dt.FechaVenta,
                TipoVenta = dt.TipoVenta,
                MetodoPago = dt.MetodoPago,
                PorcentajeImpuesto = dt.PorcentajeImpuesto,
                Subtotal = dt.Subtotal,
                Impuesto = dt.Impuesto,
                TotalVenta = dt.TotalVenta,
                MontoRecibido = dt.MontoRecibido,
                Vuelto = dt.Vuelto,
                Estado = dt.Estado,
                IdUsuarioAutorizaAnulacion = dt.IdUsuarioAutorizaAnulacion,
                FechaAnulacion = dt.FechaAnulacion,
                MotivoAnulacion = dt.MotivoAnulacion
            };
            await _repo.EditarVentaAsync(ev);
        }

        public async Task EliminarVenta(int id)
        {
            await _repo.EliminarVentaAsync(id);
        }

        public async Task<Venta?> ObtenerPorId(int id)
        {
            return await _repo.ObtenerPorIdAsync(id);
        }

        public async Task<(bool Success, string Message, int IdVenta, string NumeroFactura)> RegistrarVentaCompleta(VentaRegistroDTO dto, int idUsuario)
        {
            if (dto.Items == null || !dto.Items.Any())
                return (false, "La venta debe contener al menos un producto.", 0, string.Empty);

            if (dto.IdCliente <= 0)
                return (false, "Debe seleccionar un cliente válido.", 0, string.Empty);

            if (dto.PorcentajeImpuesto < 0)
                return (false, "El porcentaje de impuesto no puede ser negativo.", 0, string.Empty);

            if (dto.MontoRecibido < 0)
                return (false, "El monto recibido no puede ser negativo.", 0, string.Empty);

            if (dto.Items.Any(i => i.Cantidad <= 0 || i.PrecioUnitario < 0))
                return (false, "Todos los productos deben tener cantidad mayor a 0 y precio no negativo.", 0, string.Empty);

            // Validar y calcular montos
            decimal subtotal = Math.Round(dto.Items.Sum(i => i.Cantidad * i.PrecioUnitario), 2);
            decimal impuesto = Math.Round(subtotal * (dto.PorcentajeImpuesto / 100m), 2);
            decimal total = Math.Round(subtotal + impuesto, 2);
            decimal vuelto = dto.TipoVenta == "Contado" ? Math.Round(Math.Max(0, dto.MontoRecibido - total), 2) : 0;

            if (dto.TipoVenta == "Contado" && dto.MontoRecibido < total)
            {
                return (false, $"El monto recibido (C$ {dto.MontoRecibido:N2}) es menor al total de la venta (C$ {total:N2}).", 0, string.Empty);
            }

            if (dto.TipoVenta == "Credito")
            {
                if (dto.PrimaInicial < 0)
                    return (false, "La prima inicial no puede ser negativa.", 0, string.Empty);
                if (dto.PrimaInicial >= total)
                    return (false, "La prima inicial no puede ser mayor o igual al total; regístrela como venta al Contado.", 0, string.Empty);
                if (dto.CantidadCuotas <= 0)
                    return (false, "La cantidad de cuotas debe ser mayor a cero.", 0, string.Empty);
                if (dto.PorcentajeInteres < 0)
                    return (false, "El porcentaje de interés no puede ser negativo.", 0, string.Empty);
            }

            var series = new List<DetalleVentaSerie>();
            var seriesRegistradas = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var item in dto.Items)
            {
                if (item.RequiereSerie)
                {
                    var seriesValidas = (item.Series ?? new List<string>())
                        .Where(ser => !string.IsNullOrWhiteSpace(ser))
                        .Select(ser => ser.Trim())
                        .ToList();

                    if (seriesValidas.Count != item.Cantidad)
                    {
                        string nombreProducto = string.IsNullOrWhiteSpace(item.Nombre) ? $"#{item.IdProducto}" : item.Nombre;
                        return (false, $"El producto \"{nombreProducto}\" requiere {item.Cantidad} número(s) de serie.", 0, string.Empty);
                    }

                    foreach (var s in seriesValidas)
                    {
                        if (!seriesRegistradas.Add(s))
                            return (false, $"El número de serie \"{s}\" está repetido en la venta.", 0, string.Empty);

                        series.Add(new DetalleVentaSerie
                        {
                            IdDetalleVenta = item.IdProducto,
                            NumeroSerie = s,
                            EstadoSerie = "Vendido",
                            FechaRegistro = DateTime.Now
                        });
                    }
                }
            }

            var venta = new Venta
            {
                NumeroFactura = dto.NumeroFactura ?? string.Empty,
                IdCliente = dto.IdCliente,
                IdUsuarioVendedor = idUsuario,
                FechaVenta = DateTime.Now,
                TipoVenta = dto.TipoVenta,
                MetodoPago = dto.MetodoPago,
                PorcentajeImpuesto = dto.PorcentajeImpuesto,
                Subtotal = subtotal,
                Impuesto = impuesto,
                TotalVenta = total,
                MontoRecibido = dto.MontoRecibido,
                Vuelto = vuelto,
                Estado = "Completada"
            };

            var detalles = dto.Items.Select(i => new DetalleVenta
            {
                IdProducto = i.IdProducto,
                Cantidad = i.Cantidad,
                PrecioUnitario = i.PrecioUnitario,
                Subtotal = Math.Round(i.Cantidad * i.PrecioUnitario, 2)
            }).ToList();

            Credito? credito = null;
            List<CuotaCredito>? cuotas = null;

            if (dto.TipoVenta == "Credito")
            {
                decimal financiado = Math.Round(Math.Max(0, total - dto.PrimaInicial), 2);
                decimal interes = Math.Round(financiado * (dto.PorcentajeInteres / 100m), 2);
                decimal totalCredito = Math.Round(financiado + interes, 2);

                credito = new Credito
                {
                    MontoVentaTotal = total,
                    PrimaInicial = dto.PrimaInicial,
                    MontoFinanciado = financiado,
                    FrecuenciaPago = dto.FrecuenciaPago,
                    CantidadCuotas = dto.CantidadCuotas,
                    PorcentajeInteres = dto.PorcentajeInteres,
                    MontoInteresesPactados = interes,
                    MontoTotalCredito = totalCredito,
                    SaldoPendiente = totalCredito,
                    Estado = "Activo",
                    FechaRegistro = DateTime.Now
                };

                cuotas = new List<CuotaCredito>();
                decimal valorCuotaBase = Math.Round(totalCredito / dto.CantidadCuotas, 2);
                DateTime fechaVence = DateTime.Today;

                for (int c = 1; c <= dto.CantidadCuotas; c++)
                {
                    switch (dto.FrecuenciaPago)
                    {
                        case "Semanal": fechaVence = fechaVence.AddDays(7); break;
                        case "Quincenal": fechaVence = fechaVence.AddDays(15); break;
                        default: fechaVence = fechaVence.AddMonths(1); break;
                    }

                    decimal montoCuota = (c == dto.CantidadCuotas)
                        ? Math.Round(totalCredito - (valorCuotaBase * (dto.CantidadCuotas - 1)), 2)
                        : valorCuotaBase;

                    cuotas.Add(new CuotaCredito
                    {
                        NumeroCuota = c,
                        FechaVencimiento = fechaVence,
                        MontoCuota = montoCuota,
                        RecargoMora = 0,
                        MontoAbonado = 0,
                        Estado = "Pendiente"
                    });
                }
            }

            return await _repo.RegistrarVentaTransaccionalAsync(venta, detalles, series, credito, cuotas);
        }

        public async Task<(bool Success, string Message)> AnularVenta(int idVenta, int idUsuarioAutoriza, string motivo)
        {
            if (string.IsNullOrWhiteSpace(motivo))
                return (false, "Debe especificar un motivo de anulación.");

            return await _repo.AnularVentaAsync(idVenta, idUsuarioAutoriza, motivo.Trim());
        }
    }
}
