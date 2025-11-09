using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Domain.Common;

namespace App_Hospital_Clinica.Domain.Inventarios.Entities
{
    /// <summary>
    /// Catálogo de procedimientos médicos.
    /// Entidad simple: solo valida nombre (requerido, único, máx 120).
    /// </summary>
    public class Procedimiento
    {
        public int IdProcedimiento { get; private set; }
        public string Nombre { get; private set; } = default!;

        protected Procedimiento() { }

        private Procedimiento(string nombre)
        {
            Nombre = nombre;
        }

        /// <summary>
        /// Fábrica de creación protegida por invariantes.
        /// </summary>
        public static Result<Procedimiento> Create(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return Result<Procedimiento>.Fail("El nombre del procedimiento es requerido.");

            var v = nombre.Trim();
            if (v.Length > 120)
                return Result<Procedimiento>.Fail("El nombre del procedimiento no debe superar 120 caracteres.");

            return Result<Procedimiento>.Ok(new Procedimiento(v));
        }

        /// <summary>
        /// Permite actualizar el nombre manteniendo las mismas reglas.
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