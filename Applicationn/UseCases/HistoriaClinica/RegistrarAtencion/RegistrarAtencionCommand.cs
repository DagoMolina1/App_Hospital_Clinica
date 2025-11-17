using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Applicationn.UseCases.HistoriaClinica.RegistrarAtencion {
    /// <summary>
    /// ===============================================================
    ///  REGISTRAR ATENCIÓN — Command — Guía rápida
    /// ---------------------------------------------------------------
    /// Propósito:
    ///   Crear un registro de atención médica (episodio) asociado a una
    ///   historia clínica existente de un paciente.
    ///
    /// Campos:
    ///   - IdHistoria         : ID de la historia clínica existente.
    ///   - FechaAtencion      : Fecha de la atención (por defecto hoy).
    ///   - MedicoResponsable  : Profesional que atiende.
    ///   - MotivoConsulta     : Texto descriptivo.
    ///   - Observaciones      : Opcional, notas o evolución inicial.
    ///
    /// Notas:
    ///   - No crea nueva historia, sino un episodio dentro de ella.
    ///   - Valida existencia de la historia y del médico responsable.
    /// ===============================================================
    /// </summary>
    public sealed class RegistrarAtencionCommand {
        public string IdHistoria { get; init; } = default!;
        public DateTime FechaAtencion { get; init; } = DateTime.UtcNow;
        public string MedicoResponsable { get; init; } = default!;
        public string MotivoConsulta { get; init; } = default!;
        public string? Observaciones { get; init; }
    }
}