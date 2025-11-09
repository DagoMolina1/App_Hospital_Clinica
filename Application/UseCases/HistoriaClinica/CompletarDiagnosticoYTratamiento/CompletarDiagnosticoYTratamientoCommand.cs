using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Application.UseCases.HistoriaClinica.CompletarDiagnosticoYTratamiento {
    /// <summary>
    /// ===============================================================
    ///  COMPLETAR DIAGNÓSTICO Y TRATAMIENTO — Command
    /// ---------------------------------------------------------------
    /// Propósito:
    ///   Permitir que un médico o administrativo actualice los campos
    ///   del diagnóstico y tratamiento de una historia existente.
    ///
    /// Campos:
    ///   - IdHistoria    : ID de la historia clínica.
    ///   - Diagnostico   : Texto nuevo (obligatorio).
    ///   - Tratamiento   : Texto nuevo (obligatorio).
    /// ===============================================================
    /// </summary>
    public sealed class CompletarDiagnosticoYTratamientoCommand {
        public string IdHistoria { get; init; } = default!;
        public string Diagnostico { get; init; } = default!;
        public string Tratamiento { get; init; } = default!;
    }
}