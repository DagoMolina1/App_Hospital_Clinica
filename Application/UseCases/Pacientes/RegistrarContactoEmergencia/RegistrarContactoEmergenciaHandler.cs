using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Application.Abstractions;
using App_Hospital_Clinica.Domain.Common;
using App_Hospital_Clinica.Domain.Pacientes.Entities;
using App_Hospital_Clinica.Domain.Pacientes.Ports;
using App_Hospital_Clinica.Domain.Pacientes.ValueObjects;

namespace App_Hospital_Clinica.Application.UseCases.Pacientes.RegistrarContactoEmergencia {
    /// <summary>
    /// ===============================================================
    ///  REGISTRAR/ACTUALIZAR CONTACTO DE EMERGENCIA — Handler — Guía rápida
    /// ---------------------------------------------------------------
    /// Propósito:
    ///   Orquestar el Upsert (crear/actualizar) del contacto 1:1 del paciente:
    ///   [1] Validar shape del DTO.
    ///   [2] Buscar paciente por cédula (obtener IdPaciente).
    ///   [3] Construir VO de Teléfono.
    ///   [4] Si existe contacto → actualizar; si no → crear.
    ///   [5] Upsert en repositorio y confirmar con UnitOfWork.
    ///   [6] Devolver Result con datos confirmados.
    ///
    /// Entradas:
    ///   - RegistrarContactoEmergenciaCommand
    ///
    /// Salidas:
    ///   - Result<RegistrarContactoEmergenciaResult>
    ///
    /// Precondiciones:
    ///   - Repositorios inyectados: IPacienteRepository, IContactoEmergenciaRepository.
    ///   - IUnitOfWork e ILogger disponibles.
    ///
    /// Postcondiciones:
    ///   - Contacto persistido y consistente con invariantes de Dominio.
    ///
    /// Checklist (desde Controller/UI):
    ///   [ ] Construir command desde request.
    ///   [ ] var r = await handler.Handle(cmd);
    ///   [ ] Si r.IsSuccess → 200 OK; si no → 400 con r.Error.
    /// ===============================================================
    /// </summary>
    public sealed class RegistrarContactoEmergenciaHandler {
        private readonly IPacienteRepository _pacientes;
        private readonly IContactoEmergenciaRepository _contactos;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<RegistrarContactoEmergenciaHandler> _logger;

        public RegistrarContactoEmergenciaHandler(IPacienteRepository pacientes, IContactoEmergenciaRepository contactos, IUnitOfWork uow, ILogger<RegistrarContactoEmergenciaHandler> logger) {
            _pacientes = pacientes;
            _contactos = contactos;
            _uow = uow;
            _logger = logger;
        }

        public async Task<Result<RegistrarContactoEmergenciaResult>> Handle(RegistrarContactoEmergenciaCommand cmd, CancellationToken ct = default) {
            _logger.LogInformation("Upsert contacto emergencia para cédula {Cedula}", cmd.CedulaPaciente);

            // [1] Validación rápida del DTO (shape). El Dominio refuerza invariantes.
            if (string.IsNullOrWhiteSpace(cmd.Nombre) || cmd.Nombre.Trim().Length > 60)
                return Result<RegistrarContactoEmergenciaResult>.Fail("Nombre inválido (requerido, máx 60).");

            if (string.IsNullOrWhiteSpace(cmd.Apellidos) || cmd.Apellidos.Trim().Length > 60)
                return Result<RegistrarContactoEmergenciaResult>.Fail("Apellidos inválidos (requerido, máx 60).");

            if (string.IsNullOrWhiteSpace(cmd.Relacion) || cmd.Relacion.Trim().Length > 50)
                return Result<RegistrarContactoEmergenciaResult>.Fail("Relación inválida (requerida, máx 50).");

            // [2] Buscar paciente por cédula
            var paciente = await _pacientes.FindByCedulaAsync(cmd.CedulaPaciente.Trim(), ct);
            if (paciente is null)
                return Result<RegistrarContactoEmergenciaResult>.Fail("No existe un paciente con la cédula indicada.");

            // [3] VO Teléfono
            var tel = Telefono.Create(cmd.Telefono);
            if (!tel.IsSuccess)
                return Result<RegistrarContactoEmergenciaResult>.Fail(tel.Error!);

            // [4] Crear/Actualizar entidad de Dominio
            var existente = await _contactos.GetByPacienteIdAsync(paciente.IdPaciente, ct);
            ContactoEmergencia entity;

            if (existente is null) {
                var nuevoResult = ContactoEmergencia.Create(
                    idPaciente: paciente.IdPaciente,
                    nombre: cmd.Nombre.Trim(),
                    apellidos: cmd.Apellidos.Trim(),
                    relacion: cmd.Relacion.Trim(),
                    telefono: tel.Value!
                );
                if (!nuevoResult.IsSuccess)
                    return Result<RegistrarContactoEmergenciaResult>.Fail(nuevoResult.Error!);

                entity = nuevoResult.Value!;
            } else {
                var upd = existente.Actualizar(cmd.Nombre.Trim(), cmd.Apellidos.Trim(), cmd.Relacion.Trim(), tel.Value!);
                if (!upd.IsSuccess)
                    return Result<RegistrarContactoEmergenciaResult>.Fail(upd.Error!);

                entity = existente;
            }

            // [5] Upsert + confirmar
            await _contactos.UpsertAsync(entity, ct);
            await _uow.SaveChangesAsync(ct);

            _logger.LogInformation("Contacto de emergencia actualizado/creado para IdPaciente {Id}", paciente.IdPaciente);

            // [6] Salida
            var result = new RegistrarContactoEmergenciaResult {
                IdPaciente = paciente.IdPaciente,
                Nombre = entity.Nombre,
                Apellidos = entity.Apellidos,
                Relacion = entity.Relacion,
                Telefono = entity.Telefono.Value
            };

            return Result<RegistrarContactoEmergenciaResult>.Ok(result);
        }
    }
}