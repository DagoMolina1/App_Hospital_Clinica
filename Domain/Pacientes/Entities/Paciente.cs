using App_Hospital_Clinica.Domain.Common;
using App_Hospital_Clinica.Domain.Pacientes.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace App_Hospital_Clinica.Domain.Pacientes.Entities {
    /// <summary>
    /// Entidad raíz del módulo de Pacientes.
    /// Invariantes protegidas en el dominio.
    /// </summary>
    public class Paciente {
        // Mapeo 1:1 con BD (Id identity + Cedula única)
        public int IdPaciente { get; private set; }
        public Cedula Cedula { get; private set; } = default!;
        public string NombreCompleto { get; private set; } = default!;
        public DateTime FechaNac { get; private set; }
        public string Genero { get; private set; } = default!;
        public string Direccion { get; private set; } = default!;
        public Telefono Telefono { get; private set; } = default!;
        public Email Correo { get; private set; } = default!; // opcional (ValueObject permite vacío)

        // Requerido por EF Core
        protected Paciente() { }

        private Paciente(Cedula cedula, string nombre, DateTime fechaNac, string genero, string direccion, Telefono telefono, Email correo) {
            Cedula = cedula;
            NombreCompleto = nombre;
            FechaNac = fechaNac;
            Genero = genero;
            Direccion = direccion;
            Telefono = telefono;
            Correo = correo;
        }

        /// <summary>
        /// Fábrica protegida por invariantes de dominio.
        /// Asume que Application ya hizo validación de “shape” del DTO.
        /// </summary>
        public static Result<Paciente> Create(Cedula cedula, string nombre, DateTime fechaNac, string genero, string direccion, Telefono telefono, Email correo) {
            // Invariantes del dominio (no negociables)
            if (fechaNac > DateTime.UtcNow.Date)
                return Result<Paciente>.Fail("La fecha de nacimiento no puede ser futura.");

            if (string.IsNullOrWhiteSpace(nombre) || nombre.Trim().Length > 100)
                return Result<Paciente>.Fail("Nombre inválido (requerido, máx 100).");

            if (string.IsNullOrWhiteSpace(genero) || genero.Trim().Length > 20)
                return Result<Paciente>.Fail("Género inválido (requerido, máx 20).");

            if (string.IsNullOrWhiteSpace(direccion) || direccion.Trim().Length > 120)
                return Result<Paciente>.Fail("Dirección inválida (requerida, máx 120).");

            var paciente = new Paciente(
                cedula,
                nombre.Trim(),
                fechaNac.Date,
                genero.Trim(),
                direccion.Trim(),
                telefono,
                correo // puede venir vacío (Email VO lo permite)
            );

            return Result<Paciente>.Ok(paciente);
        }

        /// <summary>
        /// Cambio controlado de contacto (sigue invariantes del dominio).
        /// </summary>
        public Result ActualizarContacto(Telefono nuevoTelefono, Email nuevoCorreo) {
            // No hace falta revalidar formato: ya lo garantizan los Value Objects
            Telefono = nuevoTelefono;
            Correo = nuevoCorreo;
            return Result.Ok();
        }

        /// <summary>
        /// Cambio controlado de dirección (aplica misma invariante de longitud).
        /// </summary>
        public Result ActualizarDireccion(string nuevaDireccion) {
            if (string.IsNullOrWhiteSpace(nuevaDireccion) || nuevaDireccion.Trim().Length > 120)
                return Result.Fail("Dirección inválida (requerida, máx 120).");

            Direccion = nuevaDireccion.Trim();
            return Result.Ok();
        }

        /// <summary>
        /// Cambio controlado de datos “blandos”.
        /// </summary>
        public Result ActualizarDatos(string? nombre, string? genero, string? direccion, Telefono? tel, Email? email) {
            if (nombre is not null) {
                if (string.IsNullOrWhiteSpace(nombre) || nombre.Length > 100)
                    return Result.Fail("Nombre inválido.");
                NombreCompleto = nombre;
            }
            if (genero is not null) {
                if (string.IsNullOrWhiteSpace(genero) || genero.Length > 20)
                    return Result.Fail("Género inválido.");
                Genero = genero;
            }
            if (direccion is not null) {
                if (string.IsNullOrWhiteSpace(direccion) || direccion.Length > 120)
                    return Result.Fail("Dirección inválida.");
                Direccion = direccion;
            }
            if (tel is not null) Telefono = tel;
            if (email is not null) Correo = email;

            return Result.Ok();
        }
    }
}