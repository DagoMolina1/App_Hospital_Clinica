using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Domain.Common;
using App_Hospital_Clinica.Domain.Ordenes.Entities;

namespace App_Hospital_Clinica.Domain.Facturacion.Policies {
    /// <summary>
    /// ===============================================================
    ///  FACTURA CREACIÓN POLICY  —  Guía rápida
    /// ---------------------------------------------------------------
    /// Propósito:
    ///   Asegurar que una Orden cumple condiciones mínimas para facturar:
    ///   - Debe tener ítems.
    ///   - El subtotal debe ser > 0.
    ///
    /// Entradas:
    ///   - orden: Orden cargada con sus ítems (para calcular subtotal).
    ///
    /// Salidas:
    ///   - Result.Ok() si se puede emitir factura.
    ///   - Result.Fail(motivo) si no.
    ///
    /// Precondiciones (Application):
    ///   1) La orden existe y fue cargada (incluyendo sus ítems).
    ///   2) Ya pasaste por OrdenValidacionPolicy.PuedeGuardarse si corresponde.
    ///
    /// Postcondiciones:
    ///   - Si falla, NO generar factura.
    ///   - Si pasa, procede a crear Factura y FacturaDetalle(s).
    ///
    /// Checklist (desde Application):
    ///   [ ] var check = FacturaCreacionPolicy.PuedeFacturarse(orden);
    ///   [ ] if (!check.IsSuccess) return check;
    ///   [ ] var factura = Factura.Create(...); // + detalles según ítems
    ///   [ ] await _facturaRepo.AddAsync(factura); await _uow.SaveChangesAsync();
    /// ===============================================================
    /// </summary>
    public static class FacturaCreacionPolicy {
        public static Result PuedeFacturarse(Orden orden) {
            if (orden is null)
                return Result.Fail("No existe la orden.");

            if (!orden.Items.Any())
                return Result.Fail("No se puede facturar una orden sin ítems.");

            var subtotal = orden.Items.Sum(i => i.Costo);
            if (subtotal <= 0)
                return Result.Fail("El subtotal de la orden debe ser mayor a 0 para facturar.");

            return Result.Ok();
        }
    }
}