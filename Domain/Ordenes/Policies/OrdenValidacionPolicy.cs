using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Domain.Common;
using App_Hospital_Clinica.Domain.Ordenes.Entities;

namespace App_Hospital_Clinica.Domain.Ordenes.Policies {
    /// <summary>
    /// ===============================================================
    ///  ORDEN VALIDACIÓN POLICY  —  Guía rápida
    /// ---------------------------------------------------------------
    /// Propósito:
    ///   Validar que una Orden completa esté en condiciones de guardarse:
    ///   - Debe tener al menos un ítem.
    ///   - Ítems con costo > 0.
    ///   - Sin duplicar referencias (mismo tipo + misma referencia).
    ///
    /// Entradas:
    ///   - orden: Agregado de Orden ya armado con sus ítems.
    ///
    /// Salidas:
    ///   - Result.Ok() si se puede persistir.
    ///   - Result.Fail(motivo) si hay problemas.
    ///
    /// Precondiciones (Application):
    ///   1) DTO validado (número, cédulas, etc.).
    ///   2) Cada ítem fue creado con sus fábricas (MED/PROC/AYUDA), por lo que
    ///      las reglas específicas de cada tipo ya se cumplieron.
    ///
    /// Postcondiciones:
    ///   - Si falla, NO persistir. Devuelve el error al usuario.
    ///   - Si pasa, procede con repositorio + UnitOfWork.
    ///
    /// Checklist (desde Application):
    ///   [ ] Construir Orden + Items (fábricas del dominio).
    ///   [ ] var check = OrdenValidacionPolicy.PuedeGuardarse(orden);
    ///   [ ] if (!check.IsSuccess) return check;
    ///   [ ] await _ordenRepo.AddAsync(orden); await _uow.SaveChangesAsync();
    /// ===============================================================
    /// </summary>
    public static class OrdenValidacionPolicy {
        public static Result PuedeGuardarse(Orden orden) {
            if (orden is null) return Result.Fail("La orden es nula.");

            if (!orden.Items.Any())
                return Result.Fail("La orden debe tener al menos un ítem.");

            if (orden.Items.Any(i => i.Costo <= 0))
                return Result.Fail("Existen ítems con costo inválido (<= 0).");

            // Duplicados por referencia (mismo tipo + misma referencia de catálogo)
            var hayDuplicados = orden.Items
                .GroupBy(i => new { i.ItemType, i.IdMedicamento, i.IdProcedimiento, i.IdAyuda })
                .Any(g => g.Count() > 1);

            if (hayDuplicados)
                return Result.Fail("La orden contiene ítems duplicados del mismo tipo y referencia.");

            return Result.Ok();
        }
    }
}