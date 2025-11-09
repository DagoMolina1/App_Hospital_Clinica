using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Domain.Common;

namespace App_Hospital_Clinica.Domain.RRHH.Entities {
    /// <summary>
    /// Usuario del sistema (persona que puede autenticarse y tener roles).
    /// </summary>
    public class Usuario {
        public int IdUsuario { get; private set; }
        public string Cedula { get; private set; } = default!;
        public string NombreCompleto { get; private set; } = default!;
        public string? Correo { get; private set; }

        private readonly List<Rol> _roles = new();
        public IReadOnlyCollection<Rol> Roles => _roles.AsReadOnly();

        protected Usuario() { }

        private Usuario(string cedula, string nombreCompleto, string? correo)
        {
            Cedula = cedula;
            NombreCompleto = nombreCompleto;
            Correo = correo;
        }

        /// <summary>
        /// Fábrica protegida con invariantes del dominio.
        /// </summary>
        public static Result<Usuario> Create(string cedula, string nombreCompleto, string? correo = null)
        {
            if (string.IsNullOrWhiteSpace(cedula))
                return Result<Usuario>.Fail("La cédula es requerida.");

            if (!cedula.All(char.IsDigit))
                return Result<Usuario>.Fail("La cédula solo puede contener números.");

            if (string.IsNullOrWhiteSpace(nombreCompleto))
                return Result<Usuario>.Fail("El nombre completo es requerido.");

            if (nombreCompleto.Length > 100)
                return Result<Usuario>.Fail("El nombre completo no debe superar los 100 caracteres.");

            if (correo != null && (correo.Length > 100 || !correo.Contains('@')))
                return Result<Usuario>.Fail("El correo electrónico no es válido.");

            var usuario = new Usuario(cedula.Trim(), nombreCompleto.Trim(), correo?.Trim());
            return Result<Usuario>.Ok(usuario);
        }

        /// <summary>
        /// Asigna un rol al usuario, evitando duplicados.
        /// </summary>
        public Result AgregarRol(Rol rol)
        {
            if (rol == null)
                return Result.Fail("El rol no puede ser nulo.");

            if (_roles.Any(r => r.IdRol == rol.IdRol))
                return Result.Fail($"El usuario ya tiene asignado el rol '{rol.NombreRol}'.");

            _roles.Add(rol);
            return Result.Ok();
        }

        /// <summary>
        /// Quita un rol existente del usuario.
        /// </summary>
        public Result RemoverRol(int idRol)
        {
            var rol = _roles.FirstOrDefault(r => r.IdRol == idRol);
            if (rol == null)
                return Result.Fail("El usuario no tiene ese rol asignado.");

            _roles.Remove(rol);
            return Result.Ok();
        }
    }
}