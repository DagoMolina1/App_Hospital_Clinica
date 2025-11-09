using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Application.UseCases.Facturacion.FacturarOrden {
    /// <summary>
    /// ===============================================================
    ///  FACTURAR ORDEN — Command — Guía rápida
    /// ---------------------------------------------------------------
    /// Propósito:
    ///   Solicitar la emisión de factura para una orden existente.
    ///
    /// Entradas (DTO):
    ///   - NumeroOrden        : identificador de la orden (CHAR(6)).
    ///   - CedulaPaciente     : redundante con la orden, pero requerido por BD.
    ///   - CopagoCobrado      : monto pagado por el paciente (>= 0).
    ///   - CargoAseguradora   : monto a la aseguradora (>= 0).
    ///
    /// Notas:
    ///   - El subtotal se obtiene de los ítems de la orden.
    ///   - Las reglas mínimas se validan con la Policy del Dominio.
    /// ===============================================================
    /// </summary>
    public sealed class FacturarOrdenCommand {
        public string NumeroOrden { get; init; } = default!;
        public string CedulaPaciente { get; init; } = default!;
        public decimal CopagoCobrado { get; init; }
        public decimal CargoAseguradora { get; init; }
    }
}