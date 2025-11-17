using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Applicationn.UseCases.Pacientes.RegistrarPaciente {
    /// <summary>
    /// ===============================================================
    ///  REGISTRAR PACIENTE — Command (DTO de entrada) — Guía rápida
    /// ---------------------------------------------------------------
    /// Propósito:
    ///   Transportar los datos crudos desde la UI/API hasta el Handler.
    ///   No contiene lógica; solo datos.
    ///
    /// Cuándo se usa:
    ///   - Al registrar un paciente nuevo.
    ///
    /// Precondiciones (antes del Handler):
    ///   - La capa de presentación ya validó lo básico (no nulos, formatos).
    ///   - Aun así, el Handler revalida lo esencial y crea ValueObjects.
    ///
    /// Contenido:
    ///   - Campos alineados con la entidad de dominio Paciente.
    /// ===============================================================
    /// </summary>
    public sealed class RegistrarPacienteCommand {
        public string Cedula { get; init; } = default!;
        public string NombreCompleto { get; init; } = default!;
        public DateTime FechaNac { get; init; }
        public string Genero { get; init; } = default!;
        public string Direccion { get; init; } = default!;
        public string Telefono { get; init; } = default!;
        public string? Correo { get; init; }
    }
}