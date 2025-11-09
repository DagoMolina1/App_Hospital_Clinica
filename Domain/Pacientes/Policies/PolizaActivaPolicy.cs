using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Domain.Common;
using App_Hospital_Clinica.Domain.Pacientes.Entities;

namespace App_Hospital_Clinica.Domain.Pacientes.Policies {
    /// <summary>
    /// ===============================================================
    ///  POLIZA ACTIVA POLICY  —  Guía rápida
    /// ---------------------------------------------------------------
    /// Propósito:
    ///   Asegurar que un paciente no tenga más de una póliza ACTIVA al mismo tiempo.
    ///
    /// Entradas:
    ///   - polizaActual: Póliza vigente (si existe) del paciente, traída desde el repo.
    ///   - fechaFinNueva: Fecha de fin de la nueva póliza que se intenta activar/crear.
    ///
    /// Salidas:
    ///   - Result.Ok() para permitir continuar.
    ///   - Result.Fail(motivo) si viola la regla.
    ///
    /// Precondiciones (Application):
    ///   1) Ya validaste el DTO (fechas con sentido, strings no vacíos, etc.).
    ///   2) Cargaste la póliza actual con _polizaRepo.GetByPacienteIdAsync(idPaciente).
    ///
    /// Postcondiciones:
    ///   - Si falla, el caso de uso NO debe crear/activar la nueva póliza.
    ///   - Si pasa, puedes crear/activar y persistir.
    ///
    /// Checklist (desde Application):
    ///   [ ] var actual = await _polizaRepo.GetByPacienteIdAsync(idPaciente);
    ///   [ ] var check  = PolizaActivaPolicy.PuedeActivarse(actual, cmd.FechaFin);
    ///   [ ] if (!check.IsSuccess) return check;
    ///   [ ] var poliza = Poliza.Create(...); // luego guardar
    /// ===============================================================
    /// </summary>
    public static class PolizaActivaPolicy {
        public static Result PuedeActivarse(Poliza? polizaActual, DateTime fechaFinNueva) {
            if (fechaFinNueva < DateTime.UtcNow.Date)
                return Result.Fail("La nueva póliza no puede tener fecha de fin en el pasado.");

            // Si no hay póliza actual → se puede activar la nueva.
            if (polizaActual is null)
                return Result.Ok();

            // Si hay póliza actual vigente → no permitir otra activa.
            if (polizaActual.EstaVigente())
                return Result.Fail("El paciente ya tiene una póliza activa. Desactívala o ajusta la vigencia antes de crear una nueva.");

            return Result.Ok();
        }
    }
}