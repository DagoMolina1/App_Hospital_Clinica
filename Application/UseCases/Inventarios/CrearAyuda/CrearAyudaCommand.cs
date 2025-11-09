using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Application.UseCases.Inventarios.CrearAyuda {
    /// <summary>
    /// ===============================================================
    ///  CREAR AYUDA DIAGNÓSTICA — Command — Guía rápida
    /// ---------------------------------------------------------------
    /// Propósito:
    ///   Registrar una ayuda diagnóstica en el catálogo.
    ///
    /// Campos:
    ///   - Nombre : requerido, único, máx 120.
    /// ===============================================================
    /// </summary>
    public sealed class CrearAyudaCommand {
        public string Nombre { get; init; } = default!;
    }
}