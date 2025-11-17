using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Applicationn.UseCases.Pacientes.ModificarPaciente {
    /// <summary>
    /// ===============================================================
    ///  MODIFICAR PACIENTE — Command — Guía rápida
    /// ---------------------------------------------------------------
    /// Propósito:
    ///   Transportar los cambios “blandos” del paciente. La identidad
    ///   (Cédula) NO se modifica.
    ///
    /// Entradas:
    ///   - Cedula            : lookup obligatorio.
    ///   - NombreCompleto?   : opcional (máx 100).
    ///   - Genero?           : opcional (máx 20).
    ///   - Direccion?        : opcional (máx 120).
    ///   - Telefono?         : opcional (VO).
    ///   - Correo?           : opcional (VO).
    ///
    /// Notas:
    ///   - Solo envía lo que cambie. Campos null => no tocar.
    /// ===============================================================
    /// </summary>
    public sealed class ModificarPacienteCommand {
        public string Cedula { get; init; } = default!;
        public string? NombreCompleto { get; init; }
        public string? Genero { get; init; }
        public string? Direccion { get; init; }
        public string? Telefono { get; init; }
        public string? Correo { get; init; }
    }
}