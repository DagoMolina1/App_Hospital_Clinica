using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Applicationn.Abstractions;
using App_Hospital_Clinica.Domain.Common;
using App_Hospital_Clinica.Domain.Inventarios.Entities;
using App_Hospital_Clinica.Domain.Inventarios.Ports;

namespace App_Hospital_Clinica.Applicationn.UseCases.Inventarios.CrearAyuda {
    /// <summary>
    /// ===============================================================
    ///  CREAR AYUDA DIAGNÓSTICA — Handler — Guía rápida
    /// ---------------------------------------------------------------
    /// Flujo:
    ///  [1] Validar nombre.
    ///  [2] Verificar duplicado.
    ///  [3] Dominio: AyudaDiagnostica.Create(nombre).
    ///  [4] Repo.AddAsync + UoW.SaveChangesAsync.
    ///  [5] Devolver Id y Nombre.
    /// ===============================================================
    /// </summary>
    public sealed class CrearAyudaHandler {
        private readonly IAyudaDiagnosticaRepository _repo;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<CrearAyudaHandler> _logger;

        public CrearAyudaHandler(IAyudaDiagnosticaRepository repo, IUnitOfWork uow, ILogger<CrearAyudaHandler> logger) {
            _repo = repo;
            _uow = uow;
            _logger = logger;
        }

        public async Task<Result<CrearAyudaResult>> Handle(CrearAyudaCommand cmd, CancellationToken ct = default) {
            _logger.LogInformation("Creando ayuda diagnóstica {Nombre}", cmd.Nombre);

            if (string.IsNullOrWhiteSpace(cmd.Nombre) || cmd.Nombre.Trim().Length > 120)
                return Result<CrearAyudaResult>.Fail("Nombre inválido (requerido, máx 120).");

            var existente = await _repo.GetByNombreAsync(cmd.Nombre.Trim(), ct);
            if (existente is not null)
                return Result<CrearAyudaResult>.Fail("Ya existe una ayuda diagnóstica con ese nombre.");

            var crear = AyudaDiagnostica.Create(cmd.Nombre.Trim());
            if (!crear.IsSuccess) return Result<CrearAyudaResult>.Fail(crear.Error!);

            await _repo.AddAsync(crear.Value!, ct);
            await _uow.SaveChangesAsync(ct);

            return Result<CrearAyudaResult>.Ok(new CrearAyudaResult {
                IdAyuda = crear.Value!.IdAyuda,
                Nombre = crear.Value!.Nombre
            });
        }
    }
}