using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Application.UseCases.Ordenes.AgregarItemMedicamento {
    /// <summary>
    /// ===============================================================
    ///  AGREGAR ÍTEM (MEDICAMENTO) — Result — Guía rápida
    /// ---------------------------------------------------------------
    /// Devuelve confirmación y totales útiles para UI.
    /// ===============================================================
    /// </summary>
    public sealed class AgregarItemMedicamentoResult {
        public string NumeroOrden { get; init; } = default!;
        public int ItemN { get; init; }
        public int IdMedicamento { get; init; }
        public decimal Costo { get; init; }
        public decimal TotalOrden { get; init; }
    }
}