using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Application.Abstractions;
using App_Hospital_Clinica.Domain.Common;
using App_Hospital_Clinica.Domain.Historia_Clinica.Ports;
using App_Hospital_Clinica.Domain.Historia_Clinica.Aggregates;

namespace App_Hospital_Clinica.Application.UseCases.HistoriaClinica.RegistrarAtencion {
    /// <summary>
    /// ===============================================================
    ///  REGISTRAR ATENCIÓN — Handler — Guía rápida
    /// ---------------------------------------------------------------
    /// Flujo:
    ///  [1] Validar datos del DTO.
    ///  [2] Cargar historia clínica por ID.
    ///  [3] Crear nueva nota de evolución dentro de la historia.
    ///  [4] Persistir los cambios.
    ///  [5] Devolver confirmación con ID de nota creada.
    /// ===============================================================
    /// </summary>
    public sealed class RegistrarAtencionHandler {
        private readonly IHistoriaClinicaRepository _historias;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<RegistrarAtencionHandler> _logger;

        public RegistrarAtencionHandler(IHistoriaClinicaRepository historias, IUnitOfWork uow, ILogger<RegistrarAtencionHandler> logger) {
            _historias = historias;
            _uow = uow;
            _logger = logger;
        }

        public async Task<Result<RegistrarAtencionResult>> Handle(RegistrarAtencionCommand cmd, CancellationToken ct = default) {
            _logger.LogInformation("Registrando atención para historia {Historia}", cmd.IdHistoria);

            if (string.IsNullOrWhiteSpace(cmd.MedicoResponsable))
                return Result<RegistrarAtencionResult>.Fail("El médico responsable es requerido.");
            if (string.IsNullOrWhiteSpace(cmd.MotivoConsulta))
                return Result<RegistrarAtencionResult>.Fail("El motivo de consulta es obligatorio.");

            var historia = await _historias.GetByIdAsync(cmd.IdHistoria.Trim(), ct);
            if (historia is null)
                return Result<RegistrarAtencionResult>.Fail("La historia clínica no existe.");

            var evolucion = historia.AgregarEvolucion(
                nota: cmd.Observaciones ?? cmd.MotivoConsulta,
                medico: cmd.MedicoResponsable.Trim()
            );
            if (!evolucion.IsSuccess)
                return Result<RegistrarAtencionResult>.Fail(evolucion.Error!);

            await _historias.UpdateAsync(historia, ct);
            await _uow.SaveChangesAsync(ct);

            return Result<RegistrarAtencionResult>.Ok(new RegistrarAtencionResult
            {
                IdHistoria = historia.IdHistoria,
                FechaAtencion = cmd.FechaAtencion,
                MedicoResponsable = cmd.MedicoResponsable
            });
        }
    }
}