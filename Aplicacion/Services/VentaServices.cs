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
            if (idUsuario <= 0)
                return (false, "Usuario vendedor inválido.", 0, string.Empty);

            if (dto.Items == null || !dto.Items.Any())
                return (false, "La venta debe contener al menos un producto.", 0, string.Empty);

            if (dto.IdCliente <= 0)
                return (false, "Debe seleccionar un cliente válido.", 0, string.Empty);

            if (dto.PorcentajeImpuesto < 0 || dto.PorcentajeImpuesto > 100)
                return (false, "El porcentaje de impuesto debe estar entre 0% y 100%.", 0, string.Empty);

            if (dto.MontoRecibido < 0)
                return (false, "El monto recibido no puede ser negativo.", 0, string.Empty);

            if (!string.IsNullOrWhiteSpace(dto.NumeroFactura) && dto.NumeroFactura.Trim().Length > 30)
                return (false, "El número de factura no puede exceder 30 caracteres.", 0, string.Empty);

            if (dto.Items.Any(i => i.IdProducto <= 0 || i.Cantidad <= 0 || i.PrecioUnitario < 0))
                return (false, "Todos los productos deben ser válidos, con cantidad mayor a 0 y precio no negativo.", 0, string.Empty);

            // Normalizar modalidad y método de pago (case-insensitive)
            string tipoVentaNorm;
            if (string.Equals(dto.TipoVenta?.Trim(), "Contado", StringComparison.OrdinalIgnoreCase))
                tipoVentaNorm = "Contado";
            else if (string.Equals(dto.TipoVenta?.Trim(), "Credito", StringComparison.OrdinalIgnoreCase))
                tipoVentaNorm = "Credito";
            else
                return (false, "Modalidad de venta no válida. Debe ser Contado o Credito.", 0, string.Empty);

            string? metodoPagoNorm = new[] { "Efectivo", "Tarjeta", "Transferencia", "Mixto", "Credito" }
                .FirstOrDefault(m => string.Equals(m, dto.MetodoPago?.Trim(), StringComparison.OrdinalIgnoreCase));
            if (metodoPagoNorm == null)
                return (false, "Método de pago no válido.", 0, string.Empty);

            if (tipoVentaNorm == "Contado" && metodoPagoNorm == "Credito")
                return (false, "Una venta de Contado no puede registrarse con método de pago Crédito.", 0, string.Empty);

            string frecuenciaNorm = "Mensual";
            if (tipoVentaNorm == "Credito")
            {
                string? freqMatch = new[] { "Semanal", "Quincenal", "Mensual" }
                    .FirstOrDefault(f => string.Equals(f, dto.FrecuenciaPago?.Trim(), StringComparison.OrdinalIgnoreCase));
                if (freqMatch == null)
                    return (false, "Frecuencia de pago no válida. Debe ser Semanal, Quincenal o Mensual.", 0, string.Empty);
                frecuenciaNorm = freqMatch;
            }

            // Consolidar líneas duplicadas del mismo producto para evitar colisiones de series
            var itemsAgrupados = dto.Items
                .GroupBy(i => i.IdProducto)
                .Select(g => new VentaItemDTO
                {
                    IdProducto = g.Key,
                    Nombre = g.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.Nombre))?.Nombre,
                    CodigoSKU = g.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.CodigoSKU))?.CodigoSKU,
                    Cantidad = g.Sum(x => x.Cantidad),
                    PrecioUnitario = g.First().PrecioUnitario,
                    RequiereSerie = g.Any(x => x.RequiereSerie),
                    Series = g.SelectMany(x => x.Series ?? Enumerable.Empty<string>()).ToList()
                })
                .ToList();

            // Validar y calcular montos
            decimal subtotal = Math.Round(itemsAgrupados.Sum(i => i.Cantidad * i.PrecioUnitario), 2);
            decimal impuesto = Math.Round(subtotal * (dto.PorcentajeImpuesto / 100m), 2);
            decimal total = Math.Round(subtotal + impuesto, 2);
            decimal vuelto = tipoVentaNorm == "Contado" ? Math.Round(Math.Max(0, dto.MontoRecibido - total), 2) : 0;

            if (tipoVentaNorm == "Contado" && dto.MontoRecibido < total)
            {
                return (false, $"El monto recibido (C$ {dto.MontoRecibido:N2}) es menor al total de la venta (C$ {total:N2}).", 0, string.Empty);
            }

            if (tipoVentaNorm == "Credito")
            {
                if (total <= 0)
                    return (false, "Una venta a crédito debe tener un monto total mayor a cero.", 0, string.Empty);
                if (dto.PrimaInicial < 0)
                    return (false, "La prima inicial no puede ser negativa.", 0, string.Empty);
                if (dto.PrimaInicial >= total)
                    return (false, "La prima inicial no puede ser mayor o igual al total; regístrela como venta al Contado.", 0, string.Empty);
                if (dto.CantidadCuotas <= 0 || dto.CantidadCuotas > 120)
                    return (false, "La cantidad de cuotas debe estar entre 1 y 120.", 0, string.Empty);
                if (dto.PorcentajeInteres < 0 || dto.PorcentajeInteres > 100)
                    return (false, "El porcentaje de interés debe estar entre 0% y 100%.", 0, string.Empty);
            }

            var series = new List<DetalleVentaSerie>();
            var seriesRegistradas = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var item in itemsAgrupados)
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
                        if (s.Length > 100)
                            return (false, $"El número de serie \"{s}\" excede el máximo de 100 caracteres.", 0, string.Empty);

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
                NumeroFactura = dto.NumeroFactura?.Trim() ?? string.Empty,
                IdCliente = dto.IdCliente,
                IdUsuarioVendedor = idUsuario,
                FechaVenta = DateTime.Now,
                TipoVenta = tipoVentaNorm,
                MetodoPago = metodoPagoNorm,
                PorcentajeImpuesto = dto.PorcentajeImpuesto,
                Subtotal = subtotal,
                Impuesto = impuesto,
                TotalVenta = total,
                MontoRecibido = tipoVentaNorm == "Credito" ? dto.PrimaInicial : dto.MontoRecibido,
                Vuelto = vuelto,
                Estado = "Completada"
            };

            var detalles = itemsAgrupados.Select(i => new DetalleVenta
            {
                IdProducto = i.IdProducto,
                Cantidad = i.Cantidad,
                PrecioUnitario = Math.Round(i.PrecioUnitario, 2),
                Subtotal = Math.Round(i.Cantidad * i.PrecioUnitario, 2)
            }).ToList();

            Credito? credito = null;
            List<CuotaCredito>? cuotas = null;

            if (tipoVentaNorm == "Credito")
            {
                decimal financiado = Math.Round(Math.Max(0, total - dto.PrimaInicial), 2);
                decimal interes = Math.Round(financiado * (dto.PorcentajeInteres / 100m), 2);
                decimal totalCredito = Math.Round(financiado + interes, 2);

                if (totalCredito < dto.CantidadCuotas * 0.01m)
                    return (false, $"El monto a financiar (C$ {totalCredito:N2}) es demasiado pequeño para dividirlo en {dto.CantidadCuotas} cuotas.", 0, string.Empty);

                credito = new Credito
                {
                    MontoVentaTotal = total,
                    PrimaInicial = Math.Round(dto.PrimaInicial, 2),
                    MontoFinanciado = financiado,
                    FrecuenciaPago = frecuenciaNorm,
                    CantidadCuotas = dto.CantidadCuotas,
                    PorcentajeInteres = dto.PorcentajeInteres,
                    MontoInteresesPactados = interes,
                    MontoTotalCredito = totalCredito,
                    SaldoPendiente = totalCredito,
                    Estado = "Activo",
                    FechaRegistro = DateTime.Now
                };

                cuotas = new List<CuotaCredito>();
                decimal valorCuotaBase = Math.Floor((totalCredito / dto.CantidadCuotas) * 100m) / 100m;
                DateTime fechaVence = DateTime.Today;

                for (int c = 1; c <= dto.CantidadCuotas; c++)
                {
                    switch (frecuenciaNorm)
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
                        MoraAbonada = 0,
                        CapitalInteresAbonado = 0,
                        MontoAbonado = 0,
                        SaldoCuota = montoCuota,
                        Estado = "Pendiente"
                    });
                }
            }

            return await _repo.RegistrarVentaTransaccionalAsync(venta, detalles, series, credito, cuotas);
        }

        public async Task<(bool Success, string Message)> AnularVenta(int idVenta, int idUsuarioAutoriza, string motivo)
        {
            if (idVenta <= 0 || idUsuarioAutoriza <= 0)
                return (false, "Datos inválidos para anular la venta.");

            if (string.IsNullOrWhiteSpace(motivo))
                return (false, "Debe especificar un motivo de anulación.");

            string motivoLimpio = motivo.Trim();
            if (motivoLimpio.Length > 255)
                motivoLimpio = motivoLimpio[..255];

            return await _repo.AnularVentaAsync(idVenta, idUsuarioAutoriza, motivoLimpio);
        }
    }
}
