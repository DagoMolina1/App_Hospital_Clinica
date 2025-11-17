using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Applicationn.Abstractions;
using App_Hospital_Clinica.Domain.Common;
using App_Hospital_Clinica.Domain.Historia_Clinica.Ports;

namespace App_Hospital_Clinica.Applicationn.UseCases.HistoriaClinica.CompletarDiagnosticoYTratamiento {
    /// <summary>
    /// ===============================================================
    ///  COMPLETAR DIAGNÓSTICO Y TRATAMIENTO — Handler — Guía rápida
    /// ---------------------------------------------------------------
    /// Flujo:
    ///  [1] Validar DTO.
    ///  [2] Buscar historia por Id.
    ///  [3] Ejecutar método de dominio historia.ActualizarDiagnostico().
    ///  [4] Persistir cambios.
    ///  [5] Retornar DTO con fecha de actualización.
    /// ===============================================================
    /// </summary>
    public sealed class CompletarDiagnosticoYTratamientoHandler {
        private readonly IHistoriaClinicaRepository _historias;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<CompletarDiagnosticoYTratamientoHandler> _logger;
        private readonly IClock _clock;

        public CompletarDiagnosticoYTratamientoHandler(IHistoriaClinicaRepository historias, IUnitOfWork uow, ILogger<CompletarDiagnosticoYTratamientoHandler> logger, IClock clock) {
            _historias = historias;
            _uow = uow;
            _logger = logger;
            _clock = clock;
        }

        public async Task<Result<CompletarDiagnosticoYTratamientoResult>> Handle(CompletarDiagnosticoYTratamientoCommand cmd, CancellationToken ct = default) {
            _logger.LogInformation("Actualizando diagnóstico/tratamiento de historia {Historia}", cmd.IdHistoria);

            if (string.IsNullOrWhiteSpace(cmd.Diagnostico))
                return Result<CompletarDiagnosticoYTratamientoResult>.Fail("El diagnóstico no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(cmd.Tratamiento))
                return Result<CompletarDiagnosticoYTratamientoResult>.Fail("El tratamiento no puede estar vacío.");

            var historia = await _historias.GetByIdAsync(cmd.IdHistoria.Trim(), ct);
            if (historia is null)
                return Result<CompletarDiagnosticoYTratamientoResult>.Fail("La historia clínica no existe.");

            var actualizar = historia.CompletarDiagnosticoYTratamiento (
                cmd.Diagnostico.Trim(),
                cmd.Tratamiento.Trim()
            );

            if (!actualizar.IsSuccess)
                return Result<CompletarDiagnosticoYTratamientoResult>.Fail(actualizar.Error!);

            await _historias.UpdateAsync(historia, ct);
            await _uow.SaveChangesAsync(ct);

            return Result<CompletarDiagnosticoYTratamientoResult>.Ok(new CompletarDiagnosticoYTratamientoResult {
                IdHistoria = historia.IdHistoria,
                Diagnostico = historia.Diagnostico,
                Tratamiento = historia.Tratamiento,
                FechaActualizacion = _clock.Now
            });
        }
    }
}