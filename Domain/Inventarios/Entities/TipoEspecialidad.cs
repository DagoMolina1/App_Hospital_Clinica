using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Domain.Common;

namespace App_Hospital_Clinica.Domain.Inventarios.Entities {
    /// <summary>
    /// Catálogo de tipos de especialidades médicas.
    /// Entidad simple, con nombre único y requerido.
    /// </summary>
    public class TipoEspecialidad
    {
        public int IdTipoEspecialidad { get; private set; }
        public string Nombre { get; private set; } = default!;

        protected TipoEspecialidad() { }

        private TipoEspecialidad(string nombre)
        {
            Nombre = nombre;
        }

        /// <summary>
        /// Crea una especialidad válida (nombre requerido, máx 120 caracteres).
        /// </summary>
        public static Result<TipoEspecialidad> Create(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return Result<TipoEspecialidad>.Fail("El nombre de la especialidad es requerido.");

            var v = nombre.Trim();
            if (v.Length > 120)
                return Result<TipoEspecialidad>.Fail("El nombre de la especialidad no debe superar 120 caracteres.");

            return Result<TipoEspecialidad>.Ok(new TipoEspecialidad(v));
        }

        /// <summary>
        /// Permite actualizar el nombre manteniendo las invariantes del dominio.
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
