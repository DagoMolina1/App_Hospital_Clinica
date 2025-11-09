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

namespace App_Hospital_Clinica.Application.UseCases.Ordenes.AgregarItemProcedimiento {
    /// <summary>
    /// ===============================================================
    ///  AGREGAR ÍTEM (PROCEDIMIENTO) — Handler — Guía rápida
    /// ---------------------------------------------------------------
    /// Flujo:
    ///  [1] Validar shape (ItemN, Veces, Costo).
    ///  [2] Cargar orden.
    ///  [3] Verificar existencia del procedimiento.
    ///  [4] (Si requiere especialista) validar IdTipoEspecialidad exista.
    ///  [5] Dominio: OrdenItem.CrearProcedimiento(...)
    ///  [6] orden.AgregarItem(item).
    ///  [7] Update + SaveChanges.
    /// ===============================================================
    /// </summary>
    public sealed class AgregarItemProcedimientoHandler {
        private readonly IOrdenRepository _ordenes;
        private readonly IProcedimientoRepository _procs;
        private readonly ITipoEspecialidadRepository _esp;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<AgregarItemProcedimientoHandler> _logger;

        public AgregarItemProcedimientoHandler(IOrdenRepository ordenes, IProcedimientoRepository procs, ITipoEspecialidadRepository esp, IUnitOfWork uow, ILogger<AgregarItemProcedimientoHandler> logger) {
            _ordenes = ordenes;
            _procs = procs;
            _esp = esp;
            _uow = uow;
            _logger = logger;
        }

        public async Task<Result<AgregarItemProcedimientoResult>> Handle(AgregarItemProcedimientoCommand cmd, CancellationToken ct = default) {
            _logger.LogInformation("Agregar PROC a orden {Numero} (ItemN {ItemN})", cmd.NumeroOrden, cmd.ItemN);

            if (string.IsNullOrWhiteSpace(cmd.NumeroOrden) || cmd.NumeroOrden.Length != 6)
                return Result<AgregarItemProcedimientoResult>.Fail("Número de orden inválido.");
            if (cmd.ItemN <= 0)
                return Result<AgregarItemProcedimientoResult>.Fail("ItemN debe ser ≥ 1.");
            if (cmd.IdProcedimiento <= 0)
                return Result<AgregarItemProcedimientoResult>.Fail("Procedimiento inválido.");
            if (cmd.Veces <= 0)
                return Result<AgregarItemProcedimientoResult>.Fail("Veces debe ser ≥ 1.");
            if (cmd.Costo <= 0)
                return Result<AgregarItemProcedimientoResult>.Fail("El costo debe ser mayor a 0.");
            if (cmd.RequiereEspecialista && (!cmd.IdTipoEspecialidad.HasValue || cmd.IdTipoEspecialidad.Value <= 0))
                return Result<AgregarItemProcedimientoResult>.Fail("Se requiere un tipo de especialidad válido.");

            var orden = await _ordenes.GetByNumeroAsync(cmd.NumeroOrden.Trim(), ct);
            if (orden is null)
                return Result<AgregarItemProcedimientoResult>.Fail("La orden no existe.");

            var proc = await _procs.GetByIdAsync(cmd.IdProcedimiento, ct);
            if (proc is null)
                return Result<AgregarItemProcedimientoResult>.Fail("El procedimiento no existe.");

            if (cmd.RequiereEspecialista && cmd.IdTipoEspecialidad.HasValue) {
                var espec = await _esp.GetByIdAsync(cmd.IdTipoEspecialidad.Value, ct);
                if (espec is null)
                    return Result<AgregarItemProcedimientoResult>.Fail("El tipo de especialidad no existe.");
            }

            var itemResult = OrdenItem.CrearProcedimiento(
                numeroOrden: orden.NumeroOrden,
                itemN: cmd.ItemN,
                idProcedimiento: cmd.IdProcedimiento,
                veces: cmd.Veces,
                frecuencia: cmd.Frecuencia ?? string.Empty,
                requiereEspecialista: cmd.RequiereEspecialista,
                idTipoEspecialidad: cmd.IdTipoEspecialidad,
                costo: cmd.Costo
            );
            if (!itemResult.IsSuccess)
                return Result<AgregarItemProcedimientoResult>.Fail(itemResult.Error!);

            var agregado = orden.AgregarItem(itemResult.Value!);
            if (!agregado.IsSuccess)
                return Result<AgregarItemProcedimientoResult>.Fail(agregado.Error!);

            await _ordenes.UpdateAsync(orden, ct);
            await _uow.SaveChangesAsync(ct);

            return Result<AgregarItemProcedimientoResult>.Ok(new AgregarItemProcedimientoResult {
                NumeroOrden = orden.NumeroOrden,
                ItemN = cmd.ItemN,
                IdProcedimiento = cmd.IdProcedimiento,
                RequiereEspecialista = cmd.RequiereEspecialista,
                IdTipoEspecialidad = cmd.IdTipoEspecialidad,
                Costo = cmd.Costo,
                TotalOrden = orden.CalcularTotal()
            });
        }
    }
}