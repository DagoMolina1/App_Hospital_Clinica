using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Domain.Common;

namespace App_Hospital_Clinica.Domain.Ordenes.Entities {
    /// <summary>
    /// Ítem de una orden médica.
    /// Puede ser un medicamento, procedimiento o ayuda diagnóstica.
    /// </summary>
    public class OrdenItem
    {
        public string NumeroOrden { get; private set; } = default!;
        public int ItemN { get; private set; }

        /// <summary>
        /// Tipo de ítem: "MED", "PROC" o "AYUDA"
        /// </summary>
        public string ItemType { get; private set; } = default!;

        // IDs opcionales según tipo
        public int? IdMedicamento { get; private set; }
        public int? IdProcedimiento { get; private set; }
        public int? IdAyuda { get; private set; }
        public int? IdTipoEspecialidad { get; private set; }

        // Atributos variables por tipo
        public string? Dosis { get; private set; }          // MED
        public string? Duracion { get; private set; }       // MED
        public int? Veces { get; private set; }             // PROC
        public string? Frecuencia { get; private set; }     // PROC
        public int? Cantidad { get; private set; }          // AYUDA
        public bool? RequiereEspecialista { get; private set; } // PROC
        public decimal Costo { get; private set; }

        protected OrdenItem() { }

        private OrdenItem(string numeroOrden, int itemN, string itemType, decimal costo)
        {
            NumeroOrden = numeroOrden;
            ItemN = itemN;
            ItemType = itemType;
            Costo = costo;
        }

        /// <summary>
        /// Crea un ítem tipo medicamento.
        /// </summary>
        public static Result<OrdenItem> CrearMedicamento(string numeroOrden, int itemN, int idMedicamento, string dosis, string duracion, decimal costo)
        {
            if (idMedicamento <= 0)
                return Result<OrdenItem>.Fail("El medicamento es obligatorio.");
            if (costo <= 0)
                return Result<OrdenItem>.Fail("El costo debe ser mayor a 0.");

            var item = new OrdenItem(numeroOrden, itemN, "MED", costo)
            {
                IdMedicamento = idMedicamento,
                Dosis = dosis?.Trim(),
                Duracion = duracion?.Trim()
            };
            return Result<OrdenItem>.Ok(item);
        }

        /// <summary>
        /// Crea un ítem tipo procedimiento.
        /// </summary>
        public static Result<OrdenItem> CrearProcedimiento(string numeroOrden, int itemN, int idProcedimiento,
            int veces, string frecuencia, bool requiereEspecialista, int? idTipoEspecialidad, decimal costo)
        {
            if (idProcedimiento <= 0)
                return Result<OrdenItem>.Fail("El procedimiento es obligatorio.");
            if (veces <= 0)
                return Result<OrdenItem>.Fail("Las veces deben ser mayores a 0.");
            if (costo <= 0)
                return Result<OrdenItem>.Fail("El costo debe ser mayor a 0.");

            var item = new OrdenItem(numeroOrden, itemN, "PROC", costo)
            {
                IdProcedimiento = idProcedimiento,
                Veces = veces,
                Frecuencia = frecuencia?.Trim(),
                RequiereEspecialista = requiereEspecialista,
                IdTipoEspecialidad = idTipoEspecialidad
            };
            return Result<OrdenItem>.Ok(item);
        }

        /// <summary>
        /// Crea un ítem tipo ayuda diagnóstica.
        /// </summary>
        public static Result<OrdenItem> CrearAyuda(string numeroOrden, int itemN, int idAyuda, int cantidad, decimal costo)
        {
            if (idAyuda <= 0)
                return Result<OrdenItem>.Fail("La ayuda diagnóstica es obligatoria.");
            if (cantidad <= 0)
                return Result<OrdenItem>.Fail("La cantidad debe ser mayor a 0.");
            if (costo <= 0)
                return Result<OrdenItem>.Fail("El costo debe ser mayor a 0.");

            var item = new OrdenItem(numeroOrden, itemN, "AYUDA", costo)
            {
                IdAyuda = idAyuda,
                Cantidad = cantidad
            };
            return Result<OrdenItem>.Ok(item);
        }

        /// <summary>
        /// Permite actualizar el costo (por cambios de tarifas).
        /// </summary>
        public Result ActualizarCosto(decimal nuevoCosto)
        {
            if (nuevoCosto <= 0)
                return Result.Fail("El costo debe ser mayor a 0.");

            Costo = nuevoCosto;
            return Result.Ok();
        }
    }
}