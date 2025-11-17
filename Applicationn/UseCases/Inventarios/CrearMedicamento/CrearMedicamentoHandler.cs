using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Applicationn.Abstractions;
using App_Hospital_Clinica.Domain.Common;
using App_Hospital_Clinica.Domain.Inventarios.Entities;
using App_Hospital_Clinica.Domain.Inventarios.Ports;

namespace App_Hospital_Clinica.Applicationn.UseCases.Inventarios.CrearMedicamento {
    /// <summary>
    /// ===============================================================
    ///  CREAR MEDICAMENTO — Handler — Guía rápida
    /// ---------------------------------------------------------------
    /// Flujo:
    ///  [1] Validar nombre (requerido, longitud).
    ///  [2] Verificar duplicado por nombre.
    ///  [3] Dominio: Medicamento.Create(nombre).
    ///  [4] Repo.AddAsync + UoW.SaveChangesAsync.
    ///  [5] Devolver Id y Nombre.
    /// ===============================================================
    /// </summary>
    public sealed class CrearMedicamentoHandler {
        private readonly IMedicamentoRepository _repo;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<CrearMedicamentoHandler> _logger;

        public CrearMedicamentoHandler(IMedicamentoRepository repo, IUnitOfWork uow, ILogger<CrearMedicamentoHandler> logger) {
            _repo = repo;
            _uow = uow;
            _logger = logger;
        }

        public async Task<Result<CrearMedicamentoResult>> Handle(CrearMedicamentoCommand cmd, CancellationToken ct = default) {
            _logger.LogInformation("Creando medicamento {Nombre}", cmd.Nombre);

            if (string.IsNullOrWhiteSpace(cmd.Nombre) || cmd.Nombre.Trim().Length > 120)
                return Result<CrearMedicamentoResult>.Fail("Nombre inválido (requerido, máx 120).");

            var existente = await _repo.GetByNombreAsync(cmd.Nombre.Trim(), ct);
            if (existente is not null)
                return Result<CrearMedicamentoResult>.Fail("Ya existe un medicamento con ese nombre.");

            var crear = Medicamento.Create(cmd.Nombre.Trim());
            if (!crear.IsSuccess) return Result<CrearMedicamentoResult>.Fail(crear.Error!);

            await _repo.AddAsync(crear.Value!, ct);
            await _uow.SaveChangesAsync(ct);

            return Result<CrearMedicamentoResult>.Ok(new CrearMedicamentoResult
            {
                IdMedicamento = crear.Value!.IdMedicamento,
                Nombre = crear.Value!.Nombre
            });
        }
    }
}