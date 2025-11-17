using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Applicationn.Abstractions;
using App_Hospital_Clinica.Domain.Common;
using App_Hospital_Clinica.Domain.Inventarios.Entities;
using App_Hospital_Clinica.Domain.Inventarios.Ports;

namespace App_Hospital_Clinica.Applicationn.UseCases.Inventarios.CrearProcedimiento {
    /// <summary>
    /// ===============================================================
    ///  CREAR PROCEDIMIENTO — Handler — Guía rápida
    /// ---------------------------------------------------------------
    /// Flujo:
    ///  [1] Validar nombre.
    ///  [2] Verificar duplicado.
    ///  [3] Dominio: Procedimiento.Create(nombre).
    ///  [4] Repo.AddAsync + UoW.SaveChangesAsync.
    ///  [5] Devolver Id y Nombre.
    /// ===============================================================
    /// </summary>
    public sealed class CrearProcedimientoHandler {
        private readonly IProcedimientoRepository _repo;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<CrearProcedimientoHandler> _logger;

        public CrearProcedimientoHandler(IProcedimientoRepository repo, IUnitOfWork uow, ILogger<CrearProcedimientoHandler> logger) {
            _repo = repo;
            _uow = uow;
            _logger = logger;
        }

        public async Task<Result<CrearProcedimientoResult>> Handle(CrearProcedimientoCommand cmd, CancellationToken ct = default) {
            _logger.LogInformation("Creando procedimiento {Nombre}", cmd.Nombre);

            if (string.IsNullOrWhiteSpace(cmd.Nombre) || cmd.Nombre.Trim().Length > 120)
                return Result<CrearProcedimientoResult>.Fail("Nombre inválido (requerido, máx 120).");

            var existente = await _repo.GetByNombreAsync(cmd.Nombre.Trim(), ct);
            if (existente is not null)
                return Result<CrearProcedimientoResult>.Fail("Ya existe un procedimiento con ese nombre.");

            var crear = Procedimiento.Create(cmd.Nombre.Trim());
            if (!crear.IsSuccess) return Result<CrearProcedimientoResult>.Fail(crear.Error!);

            await _repo.AddAsync(crear.Value!, ct);
            await _uow.SaveChangesAsync(ct);

            return Result<CrearProcedimientoResult>.Ok(new CrearProcedimientoResult {
                IdProcedimiento = crear.Value!.IdProcedimiento,
                Nombre = crear.Value!.Nombre
            });
        }
    }
}