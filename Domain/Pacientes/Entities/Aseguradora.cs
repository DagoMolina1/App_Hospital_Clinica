using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Domain.Common;

namespace App_Hospital_Clinica.Domain.Pacientes.Entities {
    /// <summary>
    /// Catálogo de aseguradoras. Entidad simple con nombre único.
    /// </summary>
    public class Aseguradora {
        public int IdAseguradora { get; private set; }
        public string Nombre { get; private set; } = default!;

        // Requerido por EF Core
        protected Aseguradora() { }

        private Aseguradora(string nombre) {
            Nombre = nombre;
        }

        /// <summary>
        /// Fábrica con invariantes mínimas según BD (NOT NULL, UNIQUE, máx 100).
        /// </summary>
        public static Result<Aseguradora> Create(string nombre) {
            if (string.IsNullOrWhiteSpace(nombre))
                return Result<Aseguradora>.Fail("El nombre de la aseguradora es requerido.");

            var v = nombre.Trim();
            if (v.Length > 100)
                return Result<Aseguradora>.Fail("El nombre de la aseguradora no debe superar 100 caracteres.");

            return Result<Aseguradora>.Ok(new Aseguradora(v));
        }

        /// <summary>
        /// Cambio controlado del nombre (mantiene invariantes).
        /// </summary>
        public Result Renombrar(string nuevoNombre) {
            if (string.IsNullOrWhiteSpace(nuevoNombre))
                return Result.Fail("El nombre es requerido.");

            var v = nuevoNombre.Trim();
            if (v.Length > 100)
                return Result.Fail("El nombre no debe superar 100 caracteres.");

            Nombre = v;
            return Result.Ok();
        }
    }
}