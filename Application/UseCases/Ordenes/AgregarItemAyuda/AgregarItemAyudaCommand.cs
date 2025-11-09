using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Application.UseCases.Ordenes.AgregarItemAyuda {
    /// <summary>
    /// ===============================================================
    ///  AGREGAR ÍTEM (AYUDA DIAGNÓSTICA) — Command — Guía rápida
    /// ---------------------------------------------------------------
    /// Campos:
    ///   - NumeroOrden, ItemN
    ///   - IdAyuda (catálogo)
    ///   - Cantidad (≥1)
    ///   - Costo (>0)
    /// ===============================================================
    /// </summary>
    public sealed class AgregarItemAyudaCommand {
        public string NumeroOrden { get; init; } = default!;
        public int ItemN { get; init; }
        public int IdAyuda { get; init; }
        public int Cantidad { get; init; }
        public decimal Costo { get; init; }
    }
}