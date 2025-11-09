using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Application.Abstractions;
using App_Hospital_Clinica.Domain.Common;
using App_Hospital_Clinica.Domain.Facturacion.Entities;
using App_Hospital_Clinica.Domain.Facturacion.Ports;
using App_Hospital_Clinica.Domain.Facturacion.Policies;
using App_Hospital_Clinica.Domain.Ordenes.Ports;

namespace App_Hospital_Clinica.Application.UseCases.Facturacion.FacturarOrden {
    /// <summary>
    /// ===============================================================
    ///  FACTURAR ORDEN — Handler — Guía rápida
    /// ---------------------------------------------------------------
    /// Flujo:
    ///  [1] Validar shape del DTO (número de orden, montos >= 0).
    ///  [2] Cargar la Orden con sus ítems (desde IOrdenRepository).
    ///  [3] Validar con FacturaCreacionPolicy (orden facturable).
    ///  [4] Crear cabecera Factura (Dominio).
    ///  [5] Guardar para obtener IdFactura (UoW).
    ///  [6] Generar FacturaDetalle por cada ítem de la orden.
    ///  [7] Agregar detalles a la factura y recalcular total.
    ///  [8] Actualizar/guardar y devolver Result.
    /// ---------------------------------------------------------------
    /// Entradas:  FacturarOrdenCommand
    /// Salidas :  Result<FacturarOrdenResult>
    /// Precond.:  Repos inyectados, UoW, Logger (y opcionalmente IClock).
    /// Postcond.: Factura y detalles persistidos de forma consistente.
    /// ===============================================================
    /// </summary>
    public sealed class FacturarOrdenHandler {
        private readonly IOrdenRepository _ordenes;
        private readonly IFacturaRepository _facturas;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<FacturarOrdenHandler> _logger;

        public FacturarOrdenHandler(IOrdenRepository ordenes, IFacturaRepository facturas, IUnitOfWork uow, ILogger<FacturarOrdenHandler> logger) {
            _ordenes = ordenes;
            _facturas = facturas;
            _uow = uow;
            _logger = logger;
        }

        public async Task<Result<FacturarOrdenResult>> Handle(FacturarOrdenCommand cmd, CancellationToken ct = default) {
            _logger.LogInformation("Facturando orden {Numero} para cédula {Cedula}", cmd.NumeroOrden, cmd.CedulaPaciente);

            // [1] Validación mínima del DTO
            if (string.IsNullOrWhiteSpace(cmd.NumeroOrden) || cmd.NumeroOrden.Trim().Length != 6)
                return Result<FacturarOrdenResult>.Fail("Número de orden inválido (6 caracteres).");
            if (string.IsNullOrWhiteSpace(cmd.CedulaPaciente))
                return Result<FacturarOrdenResult>.Fail("La cédula del paciente es requerida.");
            if (cmd.CopagoCobrado < 0)
                return Result<FacturarOrdenResult>.Fail("El copago no puede ser negativo.");
            if (cmd.CargoAseguradora < 0)
                return Result<FacturarOrdenResult>.Fail("El cargo a la aseguradora no puede ser negativo.");

            // [2] Cargar Orden con sus ítems
            var orden = await _ordenes.GetByNumeroAsync(cmd.NumeroOrden.Trim(), ct);
            if (orden is null)
                return Result<FacturarOrdenResult>.Fail("La orden no existe.");

            // [3] Policy de facturación (mínimos para emitir)
            var policy = FacturaCreacionPolicy.PuedeFacturarse(orden);
            if (!policy.IsSuccess)
                return Result<FacturarOrdenResult>.Fail(policy.Error!);

            // [4] Cabecera de Factura (Dominio)
            var cabResult = Factura.Create(
                numeroOrden: orden.NumeroOrden,
                cedulaPaciente: cmd.CedulaPaciente.Trim(),
                copago: cmd.CopagoCobrado,
                cargoAseguradora: cmd.CargoAseguradora
            );
            if (!cabResult.IsSuccess)
                return Result<FacturarOrdenResult>.Fail(cabResult.Error!);

            var factura = cabResult.Value!;

            // [5] Guardar cabecera primero para materializar IdFactura
            await _facturas.AddAsync(factura, ct);
            await _uow.SaveChangesAsync(ct); // aquí Infra asigna IdFactura

            // [6] Generar detalles desde los ítems de la orden
            foreach (var it in orden.Items) {
                var descripcion = it.ItemType switch {
                    "MED" => $"MEDICAMENTO #{it.IdMedicamento} - Dosis: {it.Dosis} Duración: {it.Duracion}",
                    "PROC" => $"PROCEDIMIENTO #{it.IdProcedimiento} - Veces: {it.Veces} Frec: {it.Frecuencia}" +
                               (it.RequiereEspecialista == true ? $" (Esp: {it.IdTipoEspecialidad})" : ""),
                    "AYUDA" => $"AYUDA #{it.IdAyuda} - Cantidad: {it.Cantidad}",
                    _ => "ITEM DESCONOCIDO"
                };

                var detResult = FacturaDetalle.Create(
                    idFactura: factura.IdFactura,          // ya disponible
                    numeroOrden: orden.NumeroOrden,
                    itemN: it.ItemN,
                    descripcion: descripcion,
                    costo: it.Costo
                );
                if (!detResult.IsSuccess)
                    return Result<FacturarOrdenResult>.Fail(detResult.Error!);

                // [7] Agregar a la factura y que recalcule el total
                var agregar = factura.AgregarDetalle(detResult.Value!);
                if (!agregar.IsSuccess)
                    return Result<FacturarOrdenResult>.Fail(agregar.Error!);
            }

            // Guardar detalles y total recalculado
            await _facturas.UpdateAsync(factura, ct);
            await _uow.SaveChangesAsync(ct);

            _logger.LogInformation("Factura {Id} generada para orden {Numero}", factura.IdFactura, factura.NumeroOrden);

            // [8] Salida
            return Result<FacturarOrdenResult>.Ok(new FacturarOrdenResult {
                IdFactura = factura.IdFactura,
                NumeroOrden = factura.NumeroOrden,
                CedulaPaciente = cmd.CedulaPaciente.Trim(),
                CopagoCobrado = factura.CopagoCobrado,
                CargoAseguradora = factura.CargoAseguradora,
                TotalFactura = factura.TotalFactura,
                CantidadDetalles = factura.Detalles.Count
            });
        }
    }
}