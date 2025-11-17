using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Applicationn.UseCases.Pacientes.RegistrarPoliza {
    /// <summary>
    /// ===============================================================
    ///  REGISTRAR PÓLIZA — Result (DTO de salida) — Guía rápida
    /// ---------------------------------------------------------------
    /// Propósito:
    ///   Entregar a la capa superior la confirmación del alta con datos clave.
    /// ===============================================================
    /// </summary>
    public sealed class RegistrarPolizaResult {
        public int IdPoliza { get; init; }
        public int IdPaciente { get; init; }
        public string NumeroPoliza { get; init; } = default!;
        public DateTime FechaFin { get; init; }
        public bool Activa { get; init; }
    }
}