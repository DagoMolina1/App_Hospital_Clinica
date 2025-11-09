using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Domain.Common;

namespace App_Hospital_Clinica.Domain.Pacientes.ValueObjects {
    /// <summary>
    /// Teléfono de paciente/contacto. Inmutable y validado.
    /// Alineado con la BD (hasta 10 dígitos). Acepta fijos (>=7) y móviles (10).
    /// </summary>
    public sealed class Telefono {
        public string Value { get; } // valor inmutable
        private Telefono(string value) {
            Value = value;
        }
        public static Result<Telefono> Create(string? telefono) {
            if (string.IsNullOrWhiteSpace(telefono)) {
                return Result<Telefono>.Fail("El teléfono no puede estar vacío.");
            }
            var v = telefono.Trim();
            // Longitud: fijos 7–8 (según ciudad) y móviles 10; BD soporta hasta 10
            if (v.Length < 7 || v.Length > 10) {
                return Result<Telefono>.Fail("El teléfono debe tener entre 7 y 10 dígitos.");
            }
            // Solo dígitos
            if (!v.All(char.IsDigit)) {
                return Result<Telefono>.Fail("El teléfono solo puede contener números.");
            }
            return Result<Telefono>.Ok(new Telefono(v));
        }
        public override string ToString() => Value;
    }
}