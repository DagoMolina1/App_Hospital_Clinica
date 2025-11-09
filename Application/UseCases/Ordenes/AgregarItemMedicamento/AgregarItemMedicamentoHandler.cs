using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Application.Abstractions;
using App_Hospital_Clinica.Domain.Common;
using App_Hospital_Clinica.Domain.Inventarios.Ports;
using App_Hospital_Clinica.Domain.Ordenes.Entities;
using App_Hospital_Clinica.Domain.Ordenes.Ports;

namespace App_Hospital_Clinica.Application.UseCases.Ordenes.AgregarItemMedicamento {
    /// <summary>
    /// ===============================================================
    ///  AGREGAR ÍTEM (MEDICAMENTO) — Handler — Guía rápida
    /// ---------------------------------------------------------------
    /// Flujo:
    ///  [1] Validar shape del DTO (ItemN≥1, Costo>0).
    ///  [2] Cargar Orden; si no existe → error.
    ///  [3] Verificar que el medicamento exista.
    ///  [4] Dominio: OrdenItem.CrearMedicamento(...)
    ///  [5] orden.AgregarItem(item) (previene ItemN duplicado).
    ///  [6] _ordenRepo.UpdateAsync + _uow.SaveChangesAsync.
    ///  [7] Devolver Result con total actualizado.
    /// ===============================================================
    /// </summary>
    public sealed class AgregarItemMedicamentoHandler {
        private readonly IOrdenRepository _ordenes;
        private readonly IMedicamentoRepository _meds;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<AgregarItemMedicamentoHandler> _logger;

        public AgregarItemMedicamentoHandler(IOrdenRepository ordenes, IMedicamentoRepository meds, IUnitOfWork uow, ILogger<AgregarItemMedicamentoHandler> logger) {
            _ordenes = ordenes;
            _meds = meds;
            _uow = uow;
            _logger = logger;
        }

        public async Task<Result<AgregarItemMedicamentoResult>> Handle(AgregarItemMedicamentoCommand cmd, CancellationToken ct = default) {
            _logger.LogInformation("Agregar MED a orden {Numero} (ItemN {ItemN})", cmd.NumeroOrden, cmd.ItemN);

            if (string.IsNullOrWhiteSpace(cmd.NumeroOrden) || cmd.NumeroOrden.Length != 6)
                return Result<AgregarItemMedicamentoResult>.Fail("Número de orden inválido.");
            if (cmd.ItemN <= 0)
                return Result<AgregarItemMedicamentoResult>.Fail("ItemN debe ser ≥ 1.");
            if (cmd.IdMedicamento <= 0)
                return Result<AgregarItemMedicamentoResult>.Fail("Medicamento inválido.");
            if (cmd.Costo <= 0)
                return Result<AgregarItemMedicamentoResult>.Fail("El costo debe ser mayor a 0.");

            var orden = await _ordenes.GetByNumeroAsync(cmd.NumeroOrden.Trim(), ct);
            if (orden is null)
                return Result<AgregarItemMedicamentoResult>.Fail("La orden no existe.");

            var med = await _meds.GetByIdAsync(cmd.IdMedicamento, ct);
            if (med is null)
                return Result<AgregarItemMedicamentoResult>.Fail("El medicamento no existe.");

            var itemResult = OrdenItem.CrearMedicamento(
                numeroOrden: orden.NumeroOrden,
                itemN: cmd.ItemN,
                idMedicamento: cmd.IdMedicamento,
                dosis: cmd.Dosis ?? string.Empty,
                duracion: cmd.Duracion ?? string.Empty,
                costo: cmd.Costo
            );
            if (!itemResult.IsSuccess)
                return Result<AgregarItemMedicamentoResult>.Fail(itemResult.Error!);

            var agregado = orden.AgregarItem(itemResult.Value!);
            if (!agregado.IsSuccess)
                return Result<AgregarItemMedicamentoResult>.Fail(agregado.Error!);

            await _ordenes.UpdateAsync(orden, ct);
            await _uow.SaveChangesAsync(ct);

            return Result<AgregarItemMedicamentoResult>.Ok(new AgregarItemMedicamentoResult
            {
                NumeroOrden = orden.NumeroOrden,
                ItemN = cmd.ItemN,
                IdMedicamento = cmd.IdMedicamento,
                Costo = cmd.Costo,
                TotalOrden = orden.CalcularTotal()
            });
        }
    }
}