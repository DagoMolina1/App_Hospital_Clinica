using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Applicationn.UseCases.Pacientes.RegistrarPoliza {
    /// <summary>
    /// ===============================================================
    ///  REGISTRAR PÓLIZA — Command (DTO de entrada) — Guía rápida
    /// ---------------------------------------------------------------
    /// Propósito:
    ///   Transportar los datos necesarios para registrar/activar una póliza
    ///   de un paciente identificado por su cédula.
    ///
    /// Campos:
    ///   - CedulaPaciente (lookup del paciente)
    ///   - IdAseguradora (opcional; null si es particular)
    ///   - NumeroPoliza  (único)
    ///   - FechaFin      (vigencia futura)
    ///
    /// Nota:
    ///   - La validación de unicidad y vigencia se hace en Handler/Policies.
    /// ===============================================================
    /// </summary>
    public sealed class RegistrarPolizaCommand {
        public string CedulaPaciente { get; init; } = default!;
        public int? IdAseguradora { get; init; }
        public string NumeroPoliza { get; init; } = default!;
        public DateTime FechaFin { get; init; }
    }
}