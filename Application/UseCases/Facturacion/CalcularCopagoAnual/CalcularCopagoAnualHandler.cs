using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Application.Abstractions;
using App_Hospital_Clinica.Domain.Common;
using App_Hospital_Clinica.Domain.Facturacion.Ports;
using App_Hospital_Clinica.Domain.Ordenes.Ports;

namespace App_Hospital_Clinica.Application.UseCases.Facturacion.CalcularCopagoAnual {
    /// <summary>
    /// ===============================================================
    ///  CALCULAR COPAGO ANUAL — Handler — Guía rápida
    /// ---------------------------------------------------------------
    /// Propósito:
    ///   Calcular el total de copagos del paciente en un año, consultando
    ///   las órdenes del paciente y la(s) factura(s) asociadas por orden.
    ///
    /// Flujo:
    ///  [1] Validar shape (cédula no vacía, año razonable).
    ///  [2] Listar órdenes por cédula (IOrdenRepository).
    ///  [3] Por cada orden, intentar cargar factura (IFacturaRepository).
    ///  [4] Filtrar por año (FechaFactura.Year == cmd.Anio).
    ///  [5] Acumular CopagoCobrado y desglose mensual.
    ///  [6] Armar Result con totales/estadísticas y devolver.
    ///
    /// Entradas : CalcularCopagoAnualCommand
    /// Salidas  : Result<CalcularCopagoAnualResult>
    ///
    /// Notas:
    ///  - Este enfoque evita añadir puertos nuevos: reusa ListByCedulaPacienteAsync
    ///    y GetByNumeroOrdenAsync.
    ///  - Si más adelante necesitas grandes volúmenes, puedes agregar un
    ///    IFacturaReadRepository con queries por rango de fechas.
    /// ===============================================================
    /// </summary>
    public sealed class CalcularCopagoAnualHandler {
        private readonly IOrdenRepository _ordenes;
        private readonly IFacturaRepository _facturas;
        private readonly ILogger<CalcularCopagoAnualHandler> _logger;
        private readonly IClock _clock;

        public CalcularCopagoAnualHandler(IOrdenRepository ordenes, IFacturaRepository facturas, ILogger<CalcularCopagoAnualHandler> logger, IClock clock) {
            _ordenes = ordenes;
            _facturas = facturas;
            _logger = logger;
            _clock = clock;
        }

        public async Task<Result<CalcularCopagoAnualResult>> Handle(CalcularCopagoAnualCommand cmd, CancellationToken ct = default) {
            _logger.LogInformation("Calculando copago anual {Anio} para cédula {Cedula}", cmd.Anio, cmd.CedulaPaciente);

            // [1] Validaciones mínimas
            if (string.IsNullOrWhiteSpace(cmd.CedulaPaciente))
                return Result<CalcularCopagoAnualResult>.Fail("La cédula del paciente es requerida.");
            if (cmd.Anio < 2000 || cmd.Anio > _clock.Now.Year + 1)
                return Result<CalcularCopagoAnualResult>.Fail("El año indicado no es válido.");

            // [2] Órdenes del paciente
            var ordenes = await _ordenes.ListByCedulaPacienteAsync(cmd.CedulaPaciente.Trim(), ct);
            if (ordenes is null || ordenes.Count == 0)
                return Result<CalcularCopagoAnualResult>.Ok(new CalcularCopagoAnualResult()); // sin órdenes, todo en cero

            // [3-5] Recorrer facturas por orden y acumular
            var mensual = new decimal[12];
            decimal total = 0m;
            int count = 0;

            foreach (var o in ordenes) {
                var factura = await _facturas.GetByNumeroOrdenAsync(o.NumeroOrden, ct);
                if (factura is null) continue;

                if (factura.FechaFactura.Year != cmd.Anio) continue;

                total += factura.CopagoCobrado;
                count += 1;

                var mesIdx = factura.FechaFactura.Month - 1; // 0..11
                mensual[mesIdx] += factura.CopagoCobrado;
            }

            var result = new CalcularCopagoAnualResult {
                TotalCopago = total,
                CantFacturas = count,
                PromedioCopago = count == 0 ? 0m : decimal.Round(total / count, 2),
                Mensual = mensual
            };

            return Result<CalcularCopagoAnualResult>.Ok(result);
        }
    }
}