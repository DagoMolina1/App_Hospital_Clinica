using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Application.UseCases.Pacientes.RegistrarPaciente {
    /// <summary>
    /// ===============================================================
    ///  REGISTRAR PACIENTE — Result (DTO de salida) — Guía rápida
    /// ---------------------------------------------------------------
    /// Propósito:
    ///   Devolver a la capa superior los datos clave del registro exitoso
    ///   (útiles para confirmar o redirigir en UI).
    ///
    /// Nota:
    ///   - IdPaciente normalmente lo completa la Infra (EF Core) al guardar.
    ///   - Si tu Infra no lo recupera, puedes omitirlo y quedarte con Cedula.
    /// ===============================================================
    /// </summary>
    public sealed class RegistrarPacienteResult
    {
        public int? IdPaciente { get; init; }
        public string Cedula { get; init; } = default!;
        public string NombreCompleto { get; init; } = default!;
    }
}