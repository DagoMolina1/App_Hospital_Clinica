using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Applicationn.Abstractions;
using App_Hospital_Clinica.Domain.Common;
using App_Hospital_Clinica.Domain.Pacientes.Entities;
using App_Hospital_Clinica.Domain.Pacientes.Policies;
using App_Hospital_Clinica.Domain.Pacientes.Ports;

namespace App_Hospital_Clinica.Applicationn.UseCases.Pacientes.RegistrarPoliza {
    /// <summary>
    /// ===============================================================
    ///  REGISTRAR PÓLIZA — Handler — Guía rápida
    /// ---------------------------------------------------------------
    /// Propósito:
    ///   Orquestar el alta/activación de una póliza para un paciente:
    ///   [1] Validar shape del DTO.
    ///   [2] Buscar paciente por cédula.
    ///   [3] Verificar unicidad de NumeroPoliza.
    ///   [4] Cargar póliza actual del paciente.
    ///   [5] Evaluar PolizaActivaPolicy (no 2 activas a la vez).
    ///   [6] Dominio: Poliza.Create(...)
    ///   [7] Persistir y confirmar con UnitOfWork.
    ///   [8] Devolver Result con datos clave.
    ///
    /// Entradas:
    ///   - RegistrarPolizaCommand
    ///
    /// Salidas:
    ///   - Result<RegistrarPolizaResult>
    ///
    /// Precondiciones:
    ///   - Repositorios inyectados (paciente/póliza).
    ///   - IUnitOfWork, ILogger, IClock disponibles.
    ///
    /// Postcondiciones:
    ///   - Si éxito: póliza guardada y activa.
    ///   - Si falla: no se persiste nada; se informa el motivo.
    ///
    /// Checklist de uso (desde Controller/UI):
    ///   [ ] Construir command desde la petición.
    ///   [ ] Llamar handler.Handle(command).
    ///   [ ] Manejar Result (200 OK / 400 con mensaje).
    /// ===============================================================
    /// </summary>
    public sealed class RegistrarPolizaHandler {
        private readonly IPacienteRepository _pacientes;
        private readonly IPolizaRepository _polizas;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<RegistrarPolizaHandler> _logger;
        private readonly IClock _clock;

        public RegistrarPolizaHandler(IPacienteRepository pacientes, IPolizaRepository polizas, IUnitOfWork uow, ILogger<RegistrarPolizaHandler> logger, IClock clock) {
            _pacientes = pacientes;
            _polizas = polizas;
            _uow = uow;
            _logger = logger;
            _clock = clock;
        }

        public async Task<Result<RegistrarPolizaResult>> Handle(RegistrarPolizaCommand cmd, CancellationToken ct = default) {
            _logger.LogInformation("Iniciando registro de póliza {NumeroPoliza} para cédula {Cedula}",
                cmd.NumeroPoliza, cmd.CedulaPaciente);

            // [1] Validación básica del DTO (shape)
            if (string.IsNullOrWhiteSpace(cmd.NumeroPoliza) || cmd.NumeroPoliza.Trim().Length > 40)
                return Result<RegistrarPolizaResult>.Fail("Número de póliza inválido (requerido, máx 40).");

            if (cmd.FechaFin.Date < _clock.Now.Date)
                return Result<RegistrarPolizaResult>.Fail("La fecha de finalización no puede ser pasada.");

            // [2] Buscar paciente por cédula
            var paciente = await _pacientes.FindByCedulaAsync(cmd.CedulaPaciente.Trim(), ct);
            if (paciente is null)
                return Result<RegistrarPolizaResult>.Fail("No existe un paciente con la cédula indicada.");

            // [3] Verificar unicidad del número de póliza
            var byNumero = await _polizas.GetByNumeroAsync(cmd.NumeroPoliza.Trim(), ct);
            if (byNumero is not null)
                return Result<RegistrarPolizaResult>.Fail("Ya existe una póliza con ese número.");

            // [4] Cargar póliza actual del paciente
            var actual = await _polizas.GetByPacienteIdAsync(paciente.IdPaciente, ct);

            // [5] Evaluar policy (no dos activas)
            var policy = PolizaActivaPolicy.PuedeActivarse(actual, cmd.FechaFin.Date);
            if (!policy.IsSuccess)
                return Result<RegistrarPolizaResult>.Fail(policy.Error!);

            // [6] Dominio: crear póliza
            var polizaResult = Poliza.Create(
                idPaciente: paciente.IdPaciente,
                idAseguradora: cmd.IdAseguradora,
                numeroPoliza: cmd.NumeroPoliza.Trim(),
                fechaFin: cmd.FechaFin.Date
            );
            if (!polizaResult.IsSuccess)
                return Result<RegistrarPolizaResult>.Fail(polizaResult.Error!);

            var poliza = polizaResult.Value!;

            // [7] Persistir + confirmar
            await _polizas.AddAsync(poliza, ct);
            await _uow.SaveChangesAsync(ct);

            _logger.LogInformation("Póliza {Numero} registrada para paciente {IdPaciente}",
                poliza.NumeroPoliza, paciente.IdPaciente);

            // [8] Salida
            var result = new RegistrarPolizaResult {
                IdPoliza = poliza.IdPoliza,
                IdPaciente = paciente.IdPaciente,
                NumeroPoliza = poliza.NumeroPoliza,
                FechaFin = poliza.FechaFin,
                Activa = poliza.Activa
            };
            return Result<RegistrarPolizaResult>.Ok(result);
        }
    }
}