using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Domain.Common {
    public sealed class Result {
        public bool IsSuccess { get; }
        public string? Error { get; }

        private Result(bool ok, string? error) {
            IsSuccess = ok;
            Error = error;
        }

        public static Result Ok() => new(true, null);

        public static Result Fail(string error) {
            if (string.IsNullOrWhiteSpace(error))
                throw new ArgumentException("El mensaje de error es requerido.", nameof(error));
            return new Result(false, error);
        }
    }

    public sealed class Result<T> {
        public bool IsSuccess { get; }
        public string? Error { get; }
        public T? Value { get; }

        private Result(bool ok, T? value, string? error) {
            IsSuccess = ok;
            Value = value;
            Error = error;
        }

        public static Result<T> Ok(T value) {
            if (value is null)
                throw new ArgumentNullException(nameof(value), "El valor OK no puede ser null.");
            return new Result<T>(true, value, null);
        }

        public static Result<T> Fail(string error) {
            if (string.IsNullOrWhiteSpace(error))
                throw new ArgumentException("El mensaje de error es requerido.", nameof(error));
            return new Result<T>(false, default, error);
        }
    }
}