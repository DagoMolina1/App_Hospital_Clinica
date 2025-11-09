using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Domain.Common;

namespace App_Hospital_Clinica.Domain.Inventarios.Entities
{
    /// <summary>
    /// Catálogo de ayudas diagnósticas (exámenes, imágenes, etc.).
    /// Entidad simple con validación de nombre.
    /// </summary>
    public class AyudaDiagnostica
    {
        public int IdAyuda { get; private set; }
        public string Nombre { get; private set; } = default!;

        protected AyudaDiagnostica() { }

        private AyudaDiagnostica(string nombre)
        {
            Nombre = nombre;
        }

        /// <summary>
        /// Fábrica protegida por invariantes: nombre requerido y máx 120 caracteres.
        /// </summary>
        public static Result<AyudaDiagnostica> Create(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return Result<AyudaDiagnostica>.Fail("El nombre de la ayuda diagnóstica es requerido.");

            var v = nombre.Trim();
            if (v.Length > 120)
                return Result<AyudaDiagnostica>.Fail("El nombre de la ayuda diagnóstica no debe superar 120 caracteres.");

            return Result<AyudaDiagnostica>.Ok(new AyudaDiagnostica(v));
        }

        /// <summary>
        /// Permite actualizar el nombre respetando las mismas reglas.
        /// </summary>
        public Result Renombrar(string nuevoNombre)
        {
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