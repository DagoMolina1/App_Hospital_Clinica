using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Application.UseCases.Inventarios.CrearMedicamento {
    /// <summary>
    /// ===============================================================
    ///  CREAR MEDICAMENTO — Command — Guía rápida
    /// ---------------------------------------------------------------
    /// Propósito:
    ///   Transportar el nombre del medicamento a registrar en el catálogo.
    ///
    /// Campos:
    ///   - Nombre : requerido, único, máx 120.
    /// ===============================================================
    /// </summary>
    public sealed class CrearMedicamentoCommand {
        public string Nombre { get; init; } = default!;
    }
}