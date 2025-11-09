using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Domain.Common;
using App_Hospital_Clinica.Domain.Pacientes.ValueObjects;

namespace App_Hospital_Clinica.Domain.Historia_Clinica.Aggregates {
    /// <summary>
    /// Documento de historia clínica del paciente.
    /// Representa una atención médica (consulta, urgencia, etc.)
    /// </summary>
    public class HistoriaClinica {
        public string IdHistoria { get; private set; } = default!;  // GUID o ID generado por Mongo
        public Cedula CedulaPaciente { get; private set; } = default!;
        public DateTime FechaAtencion { get; private set; }
        public string MedicoResponsable { get; private set; } = default!;
        public string MotivoConsulta { get; private set; } = default!;
        public string Diagnostico { get; private set; } = default!;
        public string Tratamiento { get; private set; } = default!;
        public List<NotaEvolucion> Evoluciones { get; private set; } = new();

        protected HistoriaClinica() { }

        private HistoriaClinica(Cedula cedulaPaciente, string medico, string motivo, string diagnostico, string tratamiento) {
            IdHistoria = Guid.NewGuid().ToString();
            CedulaPaciente = cedulaPaciente;
            FechaAtencion = DateTime.UtcNow;
            MedicoResponsable = medico;
            MotivoConsulta = motivo;
            Diagnostico = diagnostico;
            Tratamiento = tratamiento;
        }

        /// <summary>
        /// Fábrica protegida con invariantes de negocio.
        /// </summary>
        public static Result<HistoriaClinica> Create(Cedula cedula, string medico, string motivo, string diagnostico, string tratamiento) {
            if (string.IsNullOrWhiteSpace(medico))
                return Result<HistoriaClinica>.Fail("El médico responsable es requerido.");

            if (string.IsNullOrWhiteSpace(motivo))
                return Result<HistoriaClinica>.Fail("El motivo de consulta es requerido.");

            if (string.IsNullOrWhiteSpace(diagnostico))
                return Result<HistoriaClinica>.Fail("El diagnóstico es requerido.");

            if (string.IsNullOrWhiteSpace(tratamiento))
                return Result<HistoriaClinica>.Fail("El tratamiento inicial es requerido.");

            var doc = new HistoriaClinica(
                cedula,
                medico.Trim(),
                motivo.Trim(),
                diagnostico.Trim(),
                tratamiento.Trim()
            );

            return Result<HistoriaClinica>.Ok(doc);
        }

        /// <summary>
        /// Agrega una nueva nota de evolución.
        /// </summary>
        public Result AgregarEvolucion(string nota, string medico) {
            if (string.IsNullOrWhiteSpace(nota))
                return Result.Fail("La nota de evolución es requerida.");
            if (string.IsNullOrWhiteSpace(medico))
                return Result.Fail("Debe registrar el nombre del médico.");

            Evoluciones.Add(new NotaEvolucion(Guid.NewGuid().ToString(), DateTime.UtcNow, medico.Trim(), nota.Trim()));
            return Result.Ok();
        }
        /// <summary>
        /// Completa/actualiza los campos de diagnóstico y tratamiento.
        /// Invariante: ambos textos son obligatorios.
        /// </summary>
        public Result CompletarDiagnosticoYTratamiento(string diagnostico, string tratamiento) {
            if (string.IsNullOrWhiteSpace(diagnostico))
                return Result.Fail("El diagnóstico es requerido.");
            if (string.IsNullOrWhiteSpace(tratamiento))
                return Result.Fail("El tratamiento es requerido.");

            Diagnostico = diagnostico.Trim();
            Tratamiento = tratamiento.Trim();
            return Result.Ok();
        }
    }
}