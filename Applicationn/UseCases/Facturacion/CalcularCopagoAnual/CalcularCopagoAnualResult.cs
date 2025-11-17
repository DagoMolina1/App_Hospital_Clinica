using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Applicationn.UseCases.Facturacion.CalcularCopagoAnual {
    /// <summary>
    /// ===============================================================
    ///  CALCULAR COPAGO ANUAL — Result — Guía rápida
    /// ---------------------------------------------------------------
    /// Propósito:
    ///   Devolver a UI/API los agregados del año y un desglose mensual.
    /// Contiene:
    ///   - TotalCopago    : suma de copagos del año.
    ///   - CantFacturas   : número de facturas consideradas.
    ///   - PromedioCopago : total / cantidad (si hay).
    ///   - Mensual        : arreglo 12 posiciones (ene..dic).
    /// ===============================================================
    /// </summary>
    public sealed class CalcularCopagoAnualResult {
        public decimal TotalCopago { get; init; }
        public int CantFacturas { get; init; }
        public decimal PromedioCopago { get; init; }
        public decimal[] Mensual { get; init; } = new decimal[12];
    }
}