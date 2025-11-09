using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Application.UseCases.Pacientes.ModificarPaciente {
    public sealed class ModificarPacienteResult {
        public string Cedula { get; init; } = default!;
        public string NombreCompleto { get; init; } = default!;
        public string Genero { get; init; } = default!;
        public string Direccion { get; init; } = default!;
        public string Telefono { get; init; } = default!;
        public string? Correo { get; init; }
    }
}