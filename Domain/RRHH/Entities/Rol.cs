using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Domain.Common;

namespace App_Hospital_Clinica.Domain.RRHH.Entities {
    /// <summary>
    /// Rol del sistema (Administrativo, Médico, Auxiliar, etc.).
    /// Catálogo simple con invariante de nombre.
    /// </summary>
    public class Rol {
        public int IdRol { get; private set; }
        public string NombreRol { get; private set; } = default!;

        protected Rol() { } // EF

        private Rol(string nombre) => NombreRol = nombre;

        public static Result<Rol> Create(string nombre) {
            if (string.IsNullOrWhiteSpace(nombre))
                return Result<Rol>.Fail("El nombre del rol es requerido.");

            var v = nombre.Trim();
            if (v.Length > 50)
                return Result<Rol>.Fail("El nombre del rol no debe superar 50 caracteres.");

            return Result<Rol>.Ok(new Rol(v));
        }

        public Result Renombrar(string nuevoNombre) {
            if (string.IsNullOrWhiteSpace(nuevoNombre))
                return Result.Fail("El nombre del rol es requerido.");

            var v = nuevoNombre.Trim();
            if (v.Length > 50)
                return Result.Fail("El nombre del rol no debe superar 50 caracteres.");

            NombreRol = v;
            return Result.Ok();
        }
    }
}