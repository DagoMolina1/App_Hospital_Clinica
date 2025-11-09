using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Domain.Common;
using App_Hospital_Clinica.Domain.Pacientes.ValueObjects;

namespace App_Hospital_Clinica.Domain.Pacientes.Entities {
    /// <summary>
    /// Contacto de emergencia del paciente (relación 1:1).
    /// Protege invariantes básicas del modelo.
    /// </summary>
    public class ContactoEmergencia {
        // PK = IdPaciente (1:1 con Paciente). Si prefieres PK propio, ajusta mapping luego.
        public int IdPaciente { get; private set; }

        public string Nombre { get; private set; } = default!;
        public string Apellidos { get; private set; } = default!;
        public string Relacion { get; private set; } = default!;
        public Telefono Telefono { get; private set; } = default!;

        protected ContactoEmergencia() { } // EF

        private ContactoEmergencia(int idPaciente, string nombre, string apellidos, string relacion, Telefono telefono) {
            IdPaciente = idPaciente;
            Nombre = nombre;
            Apellidos = apellidos;
            Relacion = relacion;
            Telefono = telefono;
        }

        /// <summary>
        /// Fábrica controlada por invariantes del dominio.
        /// </summary>
        public static Result<ContactoEmergencia> Create(int idPaciente, string nombre, string apellidos, string relacion, Telefono telefono) {
            if (idPaciente <= 0)
                return Result<ContactoEmergencia>.Fail("Paciente inválido.");

            if (string.IsNullOrWhiteSpace(nombre) || nombre.Trim().Length > 60)
                return Result<ContactoEmergencia>.Fail("Nombre inválido (requerido, máx 60).");

            if (string.IsNullOrWhiteSpace(apellidos) || apellidos.Trim().Length > 60)
                return Result<ContactoEmergencia>.Fail("Apellidos inválidos (requerido, máx 60).");

            if (string.IsNullOrWhiteSpace(relacion) || relacion.Trim().Length > 50)
                return Result<ContactoEmergencia>.Fail("Relación inválida (requerida, máx 50).");

            var entity = new ContactoEmergencia(
                idPaciente,
                nombre.Trim(),
                apellidos.Trim(),
                relacion.Trim(),
                telefono // ya validado por VO
            );

            return Result<ContactoEmergencia>.Ok(entity);
        }

        /// <summary>
        /// Cambio controlado del contacto (revalida invariantes).
        /// </summary>
        public Result Actualizar(string nombre, string apellidos, string relacion, Telefono telefono) {
            if (string.IsNullOrWhiteSpace(nombre) || nombre.Trim().Length > 60)
                return Result.Fail("Nombre inválido (requerido, máx 60).");
            if (string.IsNullOrWhiteSpace(apellidos) || apellidos.Trim().Length > 60)
                return Result.Fail("Apellidos inválidos (requerido, máx 60).");
            if (string.IsNullOrWhiteSpace(relacion) || relacion.Trim().Length > 50)
                return Result.Fail("Relación inválida (requerida, máx 50).");

            Nombre = nombre.Trim();
            Apellidos = apellidos.Trim();
            Relacion = relacion.Trim();
            Telefono = telefono; // VO ya garantiza formato
            return Result.Ok();
        }
    }
}