using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Application.UseCases.Ordenes.AgregarItemProcedimiento {
    /// <summary>
    /// ===============================================================
    ///  AGREGAR ÍTEM (PROCEDIMIENTO) — Command — Guía rápida
    /// ---------------------------------------------------------------
    /// Campos:
    ///   - NumeroOrden, ItemN
    ///   - IdProcedimiento (catálogo)
    ///   - Veces (≥1), Frecuencia (texto)
    ///   - RequiereEspecialista (bool), IdTipoEspecialidad (nullable)
    ///   - Costo (>0)
    ///
    /// Nota:
    ///   - Si RequiereEspecialista=true y no viene IdTipoEspecialidad → error.
    /// ===============================================================
    /// </summary>
    public sealed class AgregarItemProcedimientoCommand {
        public string NumeroOrden { get; init; } = default!;
        public int ItemN { get; init; }
        public int IdProcedimiento { get; init; }
        public int Veces { get; init; }
        public string? Frecuencia { get; init; }
        public bool RequiereEspecialista { get; init; }
        public int? IdTipoEspecialidad { get; init; }
        public decimal Costo { get; init; }
    }
}