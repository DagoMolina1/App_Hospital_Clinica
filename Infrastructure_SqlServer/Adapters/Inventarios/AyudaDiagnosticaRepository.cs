using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using App_Hospital_Clinica.Infrastructure_SqlServer.EfDb;
using App_Hospital_Clinica.Domain.Inventarios.Entities;
using App_Hospital_Clinica.Domain.Inventarios.Ports;

namespace App_Hospital_Clinica.Infrastructure_SqlServer.Adapters.Inventarios {
    /// <summary>CRUD básico para catálogo de Ayudas Diagnósticas.</summary>
    public sealed class AyudaDiagnosticaRepository : IAyudaDiagnosticaRepository
    {
        private readonly ClinicaDbContext _db;
        public AyudaDiagnosticaRepository(ClinicaDbContext db) => _db = db;

        public Task AddAsync(AyudaDiagnostica entity, CancellationToken ct = default)
            => _db.AyudasDiagnosticas.AddAsync(entity, ct).AsTask();

        public Task UpdateAsync(AyudaDiagnostica entity, CancellationToken ct = default) // NUEVO
        {
            _db.AyudasDiagnosticas.Update(entity);
            return Task.CompletedTask;
        }

        public Task<AyudaDiagnostica?> GetByIdAsync(int id, CancellationToken ct = default)
            => _db.AyudasDiagnosticas.FirstOrDefaultAsync(x => x.IdAyuda == id, ct);

        public Task<AyudaDiagnostica?> GetByNombreAsync(string nombre, CancellationToken ct = default)
            => _db.AyudasDiagnosticas.FirstOrDefaultAsync(x => x.Nombre == nombre, ct);

        public async Task<IReadOnlyList<AyudaDiagnostica>> ListAllAsync(CancellationToken ct = default) // NUEVO
        {
            var list = await _db.AyudasDiagnosticas.AsNoTracking().ToListAsync(ct);
            return list;
        }
    }
}