using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Application.UseCases.RRHH.CrearUsuario {
    /// <summary>
    /// ===============================================================
    ///  CREAR USUARIO — Command — Guía rápida
    /// ---------------------------------------------------------------
    /// Propósito:
    ///   Transportar los datos para crear un usuario de RRHH y (opcional)
    ///   asignarle roles existentes.
    ///
    /// Campos:
    ///   - Cedula          : requerido, único.
    ///   - NombreCompleto  : requerido (máx 100).
    ///   - Correo          : opcional (si viene, formato válido).
    ///   - Roles           : opcional, lista de nombres de rol a asignar.
    ///
    /// Notas:
    ///   - Los roles deben existir previamente (no se crean aquí).
    /// ===============================================================
    /// </summary>
    public sealed class CrearUsuarioCommand {
        public string Cedula { get; init; } = default!;
        public string NombreCompleto { get; init; } = default!;
        public string? Correo { get; init; }
        public string[]? Roles { get; init; }
    }
}