using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Applicationn.UseCases.Ordenes.AgregarItemMedicamento {
    /// <summary>
    /// ===============================================================
    ///  AGREGAR ÍTEM (MEDICAMENTO) — Command — Guía rápida
    /// ---------------------------------------------------------------
    /// Propósito:
    ///   Transportar los datos para agregar un medicamento a una Orden existente.
    ///
    /// Campos:
    ///   - NumeroOrden  : orden destino (CHAR(6)).
    ///   - ItemN        : posición del ítem (≥ 1, único dentro de la orden).
    ///   - IdMedicamento: catálogo existente.
    ///   - Dosis/Duracion: detalles del medicamento.
    ///   - Costo        : > 0.
    ///
    /// Notas:
    ///   - Se valida existencia de la orden y del medicamento en repos.
    ///   - La entidad Orden previene ItemN duplicado.
    /// ===============================================================
    /// </summary>
    public sealed class AgregarItemMedicamentoCommand {
        public string NumeroOrden { get; init; } = default!;
        public int ItemN { get; init; }
        public int IdMedicamento { get; init; }
        public string? Dosis { get; init; }
        public string? Duracion { get; init; }
        public decimal Costo { get; init; }
    }
}