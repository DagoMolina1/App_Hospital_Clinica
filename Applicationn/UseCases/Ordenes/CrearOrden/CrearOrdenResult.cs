using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Applicationn.UseCases.Ordenes.CrearOrden {
    /// <summary>
    /// ===============================================================
    ///  CREAR ORDEN — Result (DTO de salida) — Guía rápida
    /// ---------------------------------------------------------------
    /// Propósito:
    ///   Confirmar creación de la orden con datos clave para UI.
    /// ===============================================================
    /// </summary>
    public sealed class CrearOrdenResult {
        public string NumeroOrden { get; init; } = default!;
        public string CedulaPaciente { get; init; } = default!;
        public string CedulaMedico { get; init; } = default!;
        public DateTime FechaCreacion { get; init; }
    }
}