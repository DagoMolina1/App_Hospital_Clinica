using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Application.UseCases.Pacientes.RegistrarContactoEmergencia {
    /// <summary>
    /// ===============================================================
    ///  REGISTRAR/ACTUALIZAR CONTACTO — Result — Guía rápida
    /// ---------------------------------------------------------------
    /// Propósito:
    ///   Devolver a UI los datos clave confirmando la operación.
    /// ===============================================================
    /// </summary>
    public sealed class RegistrarContactoEmergenciaResult {
        public int IdPaciente { get; init; }
        public string Nombre { get; init; } = default!;
        public string Apellidos { get; init; } = default!;
        public string Relacion { get; init; } = default!;
        public string Telefono { get; init; } = default!;
    }
}