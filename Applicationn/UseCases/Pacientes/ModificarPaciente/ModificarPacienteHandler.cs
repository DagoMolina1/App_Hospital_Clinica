using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Applicationn.Abstractions;
using App_Hospital_Clinica.Domain.Common;
using App_Hospital_Clinica.Domain.Pacientes.Ports;
using App_Hospital_Clinica.Domain.Pacientes.ValueObjects;

namespace App_Hospital_Clinica.Applicationn.UseCases.Pacientes.ModificarPaciente {
    /// <summary>
    /// ===============================================================
    ///  MODIFICAR PACIENTE — Handler — Guía rápida
    /// ---------------------------------------------------------------
    /// Flujo:
    ///  [1] Validar DTO básico (cedula requerida; longitudes si vienen).
    ///  [2] Obtener paciente por cédula.
    ///  [3] Construir VOs solo para campos que cambien (Teléfono/Email).
    ///  [4] Invocar método de dominio para aplicar cambios (invariantes).
    ///  [5] Persistir cambios (repo + UoW) y devolver Result.
    ///
    /// Precondiciones:
    ///  - IPacienteRepository, IUnitOfWork, ILogger inyectados.
    ///  - La entidad de Dominio expone un método ActualizarDatos(...)
    ///    que acepta nulos para “no cambiar”.
    ///
    /// Postcondiciones:
    ///  - Datos actualizados; identidad (Cédula, FechaNac) inmutable.
    /// ===============================================================
    /// </summary>
    public sealed class ModificarPacienteHandler {
        private readonly IPacienteRepository _pacientes;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<ModificarPacienteHandler> _logger;

        public ModificarPacienteHandler(IPacienteRepository pacientes, IUnitOfWork uow, ILogger<ModificarPacienteHandler> logger) {
            _pacientes = pacientes;
            _uow = uow;
            _logger = logger;
        }

        public async Task<Result<ModificarPacienteResult>> Handle(ModificarPacienteCommand cmd, CancellationToken ct = default) {
            _logger.LogInformation("Modificando paciente {Cedula}", cmd.Cedula);

            if (string.IsNullOrWhiteSpace(cmd.Cedula))
                return Result<ModificarPacienteResult>.Fail("La cédula es requerida.");

            if (cmd.NombreCompleto is { Length: > 100 })
                return Result<ModificarPacienteResult>.Fail("Nombre supera 100 caracteres.");
            if (cmd.Genero is { Length: > 20 })
                return Result<ModificarPacienteResult>.Fail("Género supera 20 caracteres.");
            if (cmd.Direccion is { Length: > 120 })
                return Result<ModificarPacienteResult>.Fail("Dirección supera 120 caracteres.");

            var paciente = await _pacientes.FindByCedulaAsync(cmd.Cedula.Trim(), ct);
            if (paciente is null)
                return Result<ModificarPacienteResult>.Fail("No existe un paciente con la cédula indicada.");

            // VOs solo si cambian
            var telVO = cmd.Telefono is not null ? Telefono.Create(cmd.Telefono) : Result<Telefono>.Ok(default!);
            if (cmd.Telefono is not null && !telVO.IsSuccess)
                return Result<ModificarPacienteResult>.Fail(telVO.Error!);

            var emailVO = cmd.Correo is not null ? Email.Create(cmd.Correo) : Result<Email>.Ok(default!);
            if (cmd.Correo is not null && !emailVO.IsSuccess)
                return Result<ModificarPacienteResult>.Fail(emailVO.Error!);

            // Dominio: aplicar cambios (método sugerido en la Entidad Paciente)
            // Firma sugerida en Dominio:
            // Result ActualizarDatos(string? nombre, string? genero, string? direccion, Telefono? tel, Email? email)
            var actualizar = paciente.ActualizarDatos(
                nombre: cmd.NombreCompleto?.Trim(),
                genero: cmd.Genero?.Trim(),
                direccion: cmd.Direccion?.Trim(),
                tel: cmd.Telefono is null ? null : telVO.Value!,
                email: cmd.Correo is null ? null : emailVO.Value!
            );
            if (!actualizar.IsSuccess)
                return Result<ModificarPacienteResult>.Fail(actualizar.Error!);

            await _pacientes.UpdateAsync(paciente, ct);
            await _uow.SaveChangesAsync(ct);

            return Result<ModificarPacienteResult>.Ok(new ModificarPacienteResult
            {
                Cedula = paciente.Cedula.Value,
                NombreCompleto = paciente.NombreCompleto,
                Genero = paciente.Genero,
                Direccion = paciente.Direccion,
                Telefono = paciente.Telefono.Value,
                Correo = paciente.Correo?.Value
            });
        }
    }
}