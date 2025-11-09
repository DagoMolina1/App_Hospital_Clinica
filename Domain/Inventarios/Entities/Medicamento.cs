using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Domain.Common;

namespace App_Hospital_Clinica.Domain.Inventarios.Entities {
    /// <summary>
    /// Catálogo de Medicamentos. Entidad simple con invariante de nombre.
    /// </summary>
    public class Medicamento {
        public int IdMedicamento { get; private set; }
        public string Nombre { get; private set; } = default!;

        protected Medicamento() { } // Requerido por EF

        private Medicamento(string nombre) {
            Nombre = nombre;
        }

        /// <summary>
        /// Crea un medicamento válido (nombre requerido, máx 120).
        /// </summary>
        public static Result<Medicamento> Create(string nombre) {
            if (string.IsNullOrWhiteSpace(nombre))
                return Result<Medicamento>.Fail("El nombre del medicamento es requerido.");

            var v = nombre.Trim();
            if (v.Length > 120)
                return Result<Medicamento>.Fail("El nombre del medicamento no debe superar 120 caracteres.");

            return Result<Medicamento>.Ok(new Medicamento(v));
        }

        /// <summary>
        /// Renombra el medicamento manteniendo invariantes.
        /// </summary>
        public Result Renombrar(string nuevoNombre) {
            if (string.IsNullOrWhiteSpace(nuevoNombre))
                return Result.Fail("El nombre es requerido.");

            var v = nuevoNombre.Trim();
            if (v.Length > 120)
                return Result.Fail("El nombre no debe superar 120 caracteres.");

            Nombre = v;
            return Result.Ok();
        }
    }
}