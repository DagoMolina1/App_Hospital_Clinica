using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Applicationn.Abstractions;
using App_Hospital_Clinica.Domain.Common;
using App_Hospital_Clinica.Domain.Pacientes.Entities;
using App_Hospital_Clinica.Domain.Pacientes.Ports;
using App_Hospital_Clinica.Domain.Pacientes.ValueObjects;

namespace App_Hospital_Clinica.Applicationn.UseCases.Pacientes.RegistrarPaciente {
    /// <summary>
    /// ===============================================================
    ///  REGISTRAR PACIENTE — Handler — Guía rápida
    /// ---------------------------------------------------------------
    /// Propósito:
    ///   Orquestar el caso de uso para registrar un paciente.
    ///   - Valida el DTO mínimo necesario.
    ///   - Crea ValueObjects (Cedula, Telefono, Email).
    ///   - Verifica duplicados por cédula.
    ///   - Invoca la fábrica del Dominio (Paciente.Create).
    ///   - Persiste con Repositorio + UnitOfWork.
    ///   - Retorna un Result con datos clave.
    ///
    /// Entradas:
    ///   - RegistrarPacienteCommand (DTO).
    ///
    /// Salidas:
    ///   - Result<RegistrarPacienteResult>.
    ///
    /// Precondiciones:
    ///   - Los puertos (repos) están inyectados.
    ///   - IUnitOfWork coordina la confirmación de cambios.
    ///   - ILogger registra la trazabilidad del flujo.
    ///   - IClock disponible si necesitas decisiones por fecha.
    ///
    /// Postcondiciones:
    ///   - Si éxito: paciente persistido y confirmado en la BD.
    ///   - Si falla: no hay cambios persistidos; se devuelve motivo.
    ///
    /// Checklist (flujo):
    ///   [1] Log start
    ///   [2] Validar shape mínimo del DTO
    ///   [3] Crear VO: Cedula/Telefono/Email
    ///   [4] Verificar duplicado por cédula
    ///   [5] Domain: Paciente.Create(...)
    ///   [6] repo.AddAsync(...)
    ///   [7] _uow.SaveChangesAsync()
    ///   [8] Log success + return Result con datos
    /// ===============================================================
    /// </summary>
    public sealed class RegistrarPacienteHandler {
        private readonly IPacienteRepository _pacientes;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<RegistrarPacienteHandler> _logger;
        private readonly IClock _clock;

        public RegistrarPacienteHandler(
            IPacienteRepository pacientes,
            IUnitOfWork uow,
            ILogger<RegistrarPacienteHandler> logger,
            IClock clock)
        {
            _pacientes = pacientes;
            _uow = uow;
            _logger = logger;
            _clock = clock;
        }

        public async Task<Result<RegistrarPacienteResult>> Handle(RegistrarPacienteCommand cmd, CancellationToken ct = default)
        {
            _logger.LogInformation("Iniciando registro de paciente {Cedula}", cmd.Cedula);

            // [2] Validación mínima del DTO (shape). El Dominio refuerza invariantes después.
            if (string.IsNullOrWhiteSpace(cmd.NombreCompleto) || cmd.NombreCompleto.Length > 100)
                return Result<RegistrarPacienteResult>.Fail("Nombre inválido (requerido, máx 100).");

            if (string.IsNullOrWhiteSpace(cmd.Genero) || cmd.Genero.Length > 20)
                return Result<RegistrarPacienteResult>.Fail("Género inválido (requerido, máx 20).");

            if (string.IsNullOrWhiteSpace(cmd.Direccion) || cmd.Direccion.Length > 120)
                return Result<RegistrarPacienteResult>.Fail("Dirección inválida (requerida, máx 120).");

            // (opcional) uso de reloj para reglas temporales en Application
            if (cmd.FechaNac.Date > _clock.Now.Date)
                return Result<RegistrarPacienteResult>.Fail("La fecha de nacimiento no puede ser futura.");

            // [3] Crear Value Objects (cada uno valida su propio formato)
            var ced = Cedula.Create(cmd.Cedula);
            if (!ced.IsSuccess) return Result<RegistrarPacienteResult>.Fail(ced.Error!);

            var tel = Telefono.Create(cmd.Telefono);
            if (!tel.IsSuccess) return Result<RegistrarPacienteResult>.Fail(tel.Error!);

            var eml = Email.Create(cmd.Correo);
            if (!eml.IsSuccess) return Result<RegistrarPacienteResult>.Fail(eml.Error!);

            // [4] Verificar duplicado por cédula
            if (await _pacientes.ExistsByCedulaAsync(ced.Value!.Value, ct))
                return Result<RegistrarPacienteResult>.Fail("Ya existe un paciente con la cédula indicada.");

            // [5] Dominio: crear entidad protegida por invariantes
            var pacienteResult = Paciente.Create(
                ced.Value!,
                cmd.NombreCompleto,
                cmd.FechaNac,
                cmd.Genero,
                cmd.Direccion,
                tel.Value!,
                eml.Value!
            );
            if (!pacienteResult.IsSuccess)
                return Result<RegistrarPacienteResult>.Fail(pacienteResult.Error!);

            var paciente = pacienteResult.Value!;

            // [6] Persistir
            await _pacientes.AddAsync(paciente, ct);

            // [7] Confirmar
            await _uow.SaveChangesAsync(ct);

            // En este punto, Infra suele haber materializado IdPaciente (EF Core).
            _logger.LogInformation("Paciente {Cedula} registrado correctamente.", paciente.Cedula.Value);

            // [8] Respuesta
            var result = new RegistrarPacienteResult
            {
                IdPaciente = paciente.IdPaciente == 0 ? null : paciente.IdPaciente,
                Cedula = paciente.Cedula.Value,
                NombreCompleto = paciente.NombreCompleto
            };

            return Result<RegistrarPacienteResult>.Ok(result);
        }
    }
}