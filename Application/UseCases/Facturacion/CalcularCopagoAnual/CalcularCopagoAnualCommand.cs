using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Application.UseCases.Facturacion.CalcularCopagoAnual {
    /// <summary>
    /// ===============================================================
    ///  CALCULAR COPAGO ANUAL — Command — Guía rápida
    /// ---------------------------------------------------------------
    /// Propósito:
    ///   Solicitar el cálculo del total de copagos pagados por un paciente
    ///   en un año calendario específico.
    ///
    /// Entradas (DTO):
    ///   - CedulaPaciente : identifica al paciente.
    ///   - Anio           : año calendario (YYYY).
    ///
    /// Uso típico (UI/API):
    ///   POST /facturacion/copago/anual { cedula, anio }
    /// ===============================================================
    /// </summary>
    public sealed class CalcularCopagoAnualCommand {
        public string CedulaPaciente { get; init; } = default!;
        public int Anio { get; init; }
    }
}