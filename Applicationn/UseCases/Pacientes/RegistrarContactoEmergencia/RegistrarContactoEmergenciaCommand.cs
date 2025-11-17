using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Applicationn.UseCases.Pacientes.RegistrarContactoEmergencia {
    /// <summary>
    /// ===============================================================
    ///  REGISTRAR/ACTUALIZAR CONTACTO DE EMERGENCIA — Command — Guía rápida
    /// ---------------------------------------------------------------
    /// Propósito:
    ///   Recibir desde UI/API los datos del contacto 1:1 de un paciente.
    ///
    /// Campos:
    ///   - CedulaPaciente  : identifica al paciente (lookup).
    ///   - Nombre          : requerido (máx 60).
    ///   - Apellidos       : requerido (máx 60).
    ///   - Relacion        : requerido (máx 50) (padre, madre, pareja, etc.).
    ///   - Telefono        : requerido (7..10 dígitos).
    ///
    /// Notas:
    ///   - Si el paciente ya tiene contacto → se actualiza (Upsert).
    ///   - Si no lo tiene → se crea.
    /// ===============================================================
    /// </summary>
    public sealed class RegistrarContactoEmergenciaCommand {
        public string CedulaPaciente { get; init; } = default!;
        public string Nombre { get; init; } = default!;
        public string Apellidos { get; init; } = default!;
        public string Relacion { get; init; } = default!;
        public string Telefono { get; init; } = default!;
    }
}