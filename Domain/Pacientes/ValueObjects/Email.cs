using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Domain.Common;

namespace App_Hospital_Clinica.Domain.Pacientes.ValueObjects {
    /// <summary>
    /// Value Object que representa un correo electrónico.
    /// Puede ser nulo o vacío, pero si existe, debe tener un formato válido.
    /// </summary>
    public sealed class Email {
        public string Value { get; } // valor inmutable
        private Email(string value) {
            Value = value;
        }
        public static Result<Email> Create(string? email) {
            // Permitimos nulo o vacío (campo opcional)
            if (string.IsNullOrWhiteSpace(email)) {
                return Result<Email>.Fail("El correo electrónico no puede estar vacío.");
            }
            var v = email.Trim();
            // Validación básica: contiene '@' y un dominio
            if (!v.Contains("@") || !v.Contains(".")) {
                return Result<Email>.Fail("El correo electrónico no tiene un formato válido.");
            }
            if (v.Length < 5 || v.Length > 120) {
                return Result<Email>.Fail("El correo electrónico debe tener entre 5 y 120 caracteres.");
            }
            return Result<Email>.Ok(new Email(v));
        }
        public override string ToString() => Value;
    }
}