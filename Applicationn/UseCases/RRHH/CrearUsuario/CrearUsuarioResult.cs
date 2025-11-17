using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Applicationn.UseCases.RRHH.CrearUsuario {
    public sealed class CrearUsuarioResult {
        public int IdUsuario { get; init; }
        public string Cedula { get; init; } = default!;
        public string NombreCompleto { get; init; } = default!;
        public string? Correo { get; init; }
        public string[] RolesAsignados { get; init; } = Array.Empty<string>();
    }
}