using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Domain.Common;

namespace App_Hospital_Clinica.Domain.Pacientes.ValueObjects {
    /// <summary>
    /// Representa el valor de la cédula de identidad.
    /// Es inmutable y garantiza su validez al crearse.
    /// </summary>
    public sealed class Cedula {
        public string Value { get; } // valor inmutable
        private Cedula(string value) {
            Value = value;
        }
        public static Result<Cedula> Create(string? cedula) {
            if (string.IsNullOrWhiteSpace(cedula)) {
                return Result<Cedula>.Fail("La cédula no puede estar vacía.");
            }
            var v = cedula.Trim();
            if (v.Length < 6 || v.Length > 15) {
                return Result<Cedula>.Fail("La cédula debe tener entre 6 y 15 dígitos.");
            }
            if (!v.All(char.IsDigit)) {
                return Result<Cedula>.Fail("La cédula solo puede contener números.");
            }

            return Result<Cedula>.Ok(new Cedula(v));
        }

        public override string ToString() => Value;
    }
}