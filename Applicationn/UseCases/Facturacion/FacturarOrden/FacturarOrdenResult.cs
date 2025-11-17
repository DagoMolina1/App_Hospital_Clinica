using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Applicationn.UseCases.Facturacion.FacturarOrden {
    /// <summary>
    /// ===============================================================
    ///  FACTURAR ORDEN — Result — Guía rápida
    /// ---------------------------------------------------------------
    /// Propósito:
    ///   Devolver a UI/API los datos clave de la factura emitida.
    /// ===============================================================
    /// </summary>
    public sealed class FacturarOrdenResult {
        public int IdFactura { get; init; }
        public string NumeroOrden { get; init; } = default!;
        public string CedulaPaciente { get; init; } = default!;
        public decimal CopagoCobrado { get; init; }
        public decimal CargoAseguradora { get; init; }
        public decimal TotalFactura { get; init; }
        public int CantidadDetalles { get; init; }
    }
}