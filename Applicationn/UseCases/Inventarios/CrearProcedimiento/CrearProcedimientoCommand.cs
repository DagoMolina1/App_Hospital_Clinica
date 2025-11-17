using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Applicationn.UseCases.Inventarios.CrearProcedimiento {
    /// <summary>
    /// ===============================================================
    ///  CREAR PROCEDIMIENTO — Command — Guía rápida
    /// ---------------------------------------------------------------
    /// Propósito:
    ///   Registrar un nuevo procedimiento en el catálogo.
    ///
    /// Campos:
    ///   - Nombre : requerido, único, máx 120.
    /// ===============================================================
    /// </summary>
    public sealed class CrearProcedimientoCommand {
        public string Nombre { get; init; } = default!;
    }
}