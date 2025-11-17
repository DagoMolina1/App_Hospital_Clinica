using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Applicationn.Abstractions;
using App_Hospital_Clinica.Domain.Common;
using App_Hospital_Clinica.Domain.Inventarios.Ports;
using App_Hospital_Clinica.Domain.Ordenes.Entities;
using App_Hospital_Clinica.Domain.Ordenes.Ports;

namespace App_Hospital_Clinica.Applicationn.UseCases.Ordenes.AgregarItemAyuda {
    /// <summary>
    /// ===============================================================
    ///  AGREGAR ÍTEM (AYUDA DIAGNÓSTICA) — Handler — Guía rápida
    /// ---------------------------------------------------------------
    /// Flujo:
    ///  [1] Validar shape (ItemN, Cantidad, Costo).
    ///  [2] Cargar orden.
    ///  [3] Verificar existencia de la ayuda diagnóstica.
    ///  [4] Dominio: OrdenItem.CrearAyuda(...)
    ///  [5] orden.AgregarItem(item).
    ///  [6] Update + SaveChanges.
    /// ===============================================================
    /// </summary>
    public sealed class AgregarItemAyudaHandler {
        private readonly IOrdenRepository _ordenes;
        private readonly IAyudaDiagnosticaRepository _ayudas;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<AgregarItemAyudaHandler> _logger;

        public AgregarItemAyudaHandler(IOrdenRepository ordenes, IAyudaDiagnosticaRepository ayudas, IUnitOfWork uow, ILogger<AgregarItemAyudaHandler> logger) {
            _ordenes = ordenes;
            _ayudas = ayudas;
            _uow = uow;
            _logger = logger;
        }

        public async Task<Result<AgregarItemAyudaResult>> Handle(AgregarItemAyudaCommand cmd, CancellationToken ct = default) {
            _logger.LogInformation("Agregar AYUDA a orden {Numero} (ItemN {ItemN})", cmd.NumeroOrden, cmd.ItemN);

            if (string.IsNullOrWhiteSpace(cmd.NumeroOrden) || cmd.NumeroOrden.Length != 6)
                return Result<AgregarItemAyudaResult>.Fail("Número de orden inválido.");
            if (cmd.ItemN <= 0)
                return Result<AgregarItemAyudaResult>.Fail("ItemN debe ser ≥ 1.");
            if (cmd.IdAyuda <= 0)
                return Result<AgregarItemAyudaResult>.Fail("Ayuda diagnóstica inválida.");
            if (cmd.Cantidad <= 0)
                return Result<AgregarItemAyudaResult>.Fail("La cantidad debe ser ≥ 1.");
            if (cmd.Costo <= 0)
                return Result<AgregarItemAyudaResult>.Fail("El costo debe ser mayor a 0.");

            var orden = await _ordenes.GetByNumeroAsync(cmd.NumeroOrden.Trim(), ct);
            if (orden is null)
                return Result<AgregarItemAyudaResult>.Fail("La orden no existe.");

            var ayuda = await _ayudas.GetByIdAsync(cmd.IdAyuda, ct);
            if (ayuda is null)
                return Result<AgregarItemAyudaResult>.Fail("La ayuda diagnóstica no existe.");

            var itemResult = OrdenItem.CrearAyuda(
                numeroOrden: orden.NumeroOrden,
                itemN: cmd.ItemN,
                idAyuda: cmd.IdAyuda,
                cantidad: cmd.Cantidad,
                costo: cmd.Costo
            );
            if (!itemResult.IsSuccess)
                return Result<AgregarItemAyudaResult>.Fail(itemResult.Error!);

            var agregado = orden.AgregarItem(itemResult.Value!);
            if (!agregado.IsSuccess)
                return Result<AgregarItemAyudaResult>.Fail(agregado.Error!);

            await _ordenes.UpdateAsync(orden, ct);
            await _uow.SaveChangesAsync(ct);

            return Result<AgregarItemAyudaResult>.Ok(new AgregarItemAyudaResult {
                NumeroOrden = orden.NumeroOrden,
                ItemN = cmd.ItemN,
                IdAyuda = cmd.IdAyuda,
                Cantidad = cmd.Cantidad,
                Costo = cmd.Costo,
                TotalOrden = orden.CalcularTotal()
            });
        }
    }
}