using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Application.Abstractions;
using App_Hospital_Clinica.Domain.Common;
using App_Hospital_Clinica.Domain.Ordenes.Entities;
using App_Hospital_Clinica.Domain.Ordenes.Ports;
using App_Hospital_Clinica.Domain.Pacientes.Ports;
using App_Hospital_Clinica.Domain.Pacientes.ValueObjects;

namespace App_Hospital_Clinica.Application.UseCases.Ordenes.CrearOrden {
    /// <summary>
    /// ===============================================================
    ///  CREAR ORDEN — Handler — Guía rápida
    /// ---------------------------------------------------------------
    /// Propósito:
    ///   Orquestar la creación de la cabecera de una Orden:
    ///   [1] Validar shape del DTO (longitudes/formato).
    ///   [2] Verificar existencia del Paciente por cédula.
    ///   [3] Chequear unicidad del NumeroOrden.
    ///   [4] Crear VO CedulaPaciente y entidad Orden (Dominio).
    ///   [5] Persistir con IOrdenRepository + IUnitOfWork.
    ///   [6] Devolver Result con datos clave.
    ///
    /// Entradas:
    ///   - CrearOrdenCommand
    ///
    /// Salidas:
    ///   - Result<CrearOrdenResult>
    ///
    /// Precondiciones:
    ///   - Repos inyectados: IPacienteRepository, IOrdenRepository.
    ///   - IUnitOfWork e ILogger disponibles.
    ///   - (Opcional) IClock si deseas usar la hora centralizada.
    ///
    /// Postcondiciones:
    ///   - Orden persistida sin ítems aún (se agregan en otro caso de uso).
    ///
    /// Checklist (desde Controller/UI):
    ///   [ ] var r = await handler.Handle(cmd);
    ///   [ ] if (!r.IsSuccess) -> 400 con r.Error; else 200 OK.
    /// ===============================================================
    /// </summary>
    public sealed class CrearOrdenHandler {
        private readonly IPacienteRepository _pacientes;
        private readonly IOrdenRepository _ordenes;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<CrearOrdenHandler> _logger;
        private readonly IClock _clock;

        public CrearOrdenHandler(IPacienteRepository pacientes, IOrdenRepository ordenes, IUnitOfWork uow, ILogger<CrearOrdenHandler> logger, IClock clock) {
            _pacientes = pacientes;
            _ordenes = ordenes;
            _uow = uow;
            _logger = logger;
            _clock = clock;
        }

        public async Task<Result<CrearOrdenResult>> Handle(CrearOrdenCommand cmd, CancellationToken ct = default) {
            _logger.LogInformation("Creando orden {Numero} para paciente {Cedula}", cmd.NumeroOrden, cmd.CedulaPaciente);

            // [1] Validación mínima del DTO (shape)
            if (string.IsNullOrWhiteSpace(cmd.NumeroOrden) || cmd.NumeroOrden.Length != 6 || !cmd.NumeroOrden.All(char.IsDigit))
                return Result<CrearOrdenResult>.Fail("El número de orden debe tener exactamente 6 dígitos.");

            if (string.IsNullOrWhiteSpace(cmd.CedulaMedico) || !cmd.CedulaMedico.All(char.IsDigit))
                return Result<CrearOrdenResult>.Fail("La cédula del médico es requerida y debe ser numérica.");

            // [2] Paciente existente
            var paciente = await _pacientes.FindByCedulaAsync(cmd.CedulaPaciente.Trim(), ct);
            if (paciente is null)
                return Result<CrearOrdenResult>.Fail("No existe un paciente con la cédula indicada.");

            // [3] Unicidad de la orden
            if (await _ordenes.ExistsAsync(cmd.NumeroOrden.Trim(), ct))
                return Result<CrearOrdenResult>.Fail("Ya existe una orden con ese número.");

            // [4] VO + Dominio
            var cedVO = Cedula.Create(cmd.CedulaPaciente);
            if (!cedVO.IsSuccess) return Result<CrearOrdenResult>.Fail(cedVO.Error!);

            var ordenResult = Orden.Create(cmd.NumeroOrden.Trim(), cedVO.Value!, cmd.CedulaMedico.Trim());
            if (!ordenResult.IsSuccess) return Result<CrearOrdenResult>.Fail(ordenResult.Error!);

            var orden = ordenResult.Value!;

            // [5] Persistir + confirmar
            await _ordenes.AddAsync(orden, ct);
            await _uow.SaveChangesAsync(ct);

            _logger.LogInformation("Orden {Numero} creada correctamente.", orden.NumeroOrden);

            // [6] Salida
            var result = new CrearOrdenResult {
                NumeroOrden = orden.NumeroOrden,
                CedulaPaciente = orden.CedulaPaciente.Value,
                CedulaMedico = orden.CedulaMedico,
                FechaCreacion = _clock.Now.Date
            };
            return Result<CrearOrdenResult>.Ok(result);
        }
    }
}