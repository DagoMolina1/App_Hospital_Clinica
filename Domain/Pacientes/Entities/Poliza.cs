using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Domain.Common;

namespace App_Hospital_Clinica.Domain.Pacientes.Entities {
    /// <summary>
    /// Póliza de seguro asociada a un paciente.
    /// Mantiene su vigencia y estado activo según la fecha de expiración.
    /// </summary>
    public class Poliza {
        public int IdPoliza { get; private set; }
        public int IdPaciente { get; private set; }
        public int? IdAseguradora { get; private set; }   // Puede ser null si el paciente no tiene seguro
        public string NumeroPoliza { get; private set; } = default!;
        public bool Activa { get; private set; }
        public DateTime FechaFin { get; private set; }

        // Requerido por EF Core
        protected Poliza() { }

        private Poliza(int idPaciente, int? idAseguradora, string numero, bool activa, DateTime fechaFin) {
            IdPaciente = idPaciente;
            IdAseguradora = idAseguradora;
            NumeroPoliza = numero;
            Activa = activa;
            FechaFin = fechaFin;
        }

        /// <summary>
        /// Fábrica protegida por invariantes del dominio.
        /// </summary>
        public static Result<Poliza> Create(int idPaciente, int? idAseguradora, string numeroPoliza, DateTime fechaFin) {
            if (idPaciente <= 0)
                return Result<Poliza>.Fail("Paciente inválido.");

            if (string.IsNullOrWhiteSpace(numeroPoliza))
                return Result<Poliza>.Fail("El número de póliza es requerido.");

            var n = numeroPoliza.Trim();
            if (n.Length > 40)
                return Result<Poliza>.Fail("El número de póliza no debe superar 40 caracteres.");

            if (fechaFin < DateTime.UtcNow.Date)
                return Result<Poliza>.Fail("La fecha de finalización no puede ser pasada.");

            var poliza = new Poliza(
                idPaciente,
                idAseguradora,
                n,
                activa: true,         // Se crea activa por defecto
                fechaFin: fechaFin.Date
            );

            return Result<Poliza>.Ok(poliza);
        }

        /// <summary>
        /// Actualiza la vigencia de la póliza. Si la nueva fecha ya expiró, la desactiva automáticamente.
        /// </summary>
        public Result ActualizarVigencia(DateTime nuevaFechaFin) {
            if (nuevaFechaFin < DateTime.UtcNow.Date) {
                Activa = false;
                return Result.Fail("La fecha de finalización no puede ser pasada. La póliza fue marcada como inactiva.");
            }

            FechaFin = nuevaFechaFin.Date;
            Activa = true;
            return Result.Ok();
        }

        /// <summary>
        /// Desactiva manualmente la póliza.
        /// </summary>
        public void Desactivar() => Activa = false;

        /// <summary>
        /// Verifica automáticamente si la póliza está vigente.
        /// </summary>
        public bool EstaVigente() => Activa && FechaFin >= DateTime.UtcNow.Date;
    }
}