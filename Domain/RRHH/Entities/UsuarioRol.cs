using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Domain.RRHH.Entities {
    /// <summary>
    /// Relación muchos-a-muchos entre Usuario y Rol.
    /// Cada registro indica qué rol tiene asignado un usuario.
    /// </summary>
    public class UsuarioRol {
        // Setters públicos para permitir que Infra lo asigne
        public int IdUsuario { get; set; }
        public int IdRol { get; set; }
        /*public int IdUsuario { get; private set; }
        public int IdRol { get; private set; }*/

        // Navegación (opcional, útil para EF Core)
        public Usuario? Usuario { get; private set; }
        public Rol? Rol { get; private set; }

        ///protected UsuarioRol() { }
        /// EF necesita ctor público sin params
        public UsuarioRol() { }

        private UsuarioRol(int idUsuario, int idRol) {
            IdUsuario = idUsuario;
            IdRol = idRol;
        }

        /// <summary>
        /// Fábrica para crear una relación válida.
        /// </summary>
        public static UsuarioRol Create(int idUsuario, int idRol) {
            if (idUsuario <= 0)
                throw new ArgumentException("El ID del usuario es inválido.", nameof(idUsuario));

            if (idRol <= 0)
                throw new ArgumentException("El ID del rol es inválido.", nameof(idRol));

            return new UsuarioRol(idUsuario, idRol);
        }
    }
}