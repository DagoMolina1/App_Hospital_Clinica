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
    /// <summary>
    /// CRUD básico para catálogo de Medicamentos.
    /// - Garantiza unicidad por Nombre (ver Config).
    /// </summary>
    public sealed class MedicamentoRepository : IMedicamentoRepository
    {
        private readonly ClinicaDbContext _db;
        public MedicamentoRepository(ClinicaDbContext db) => _db = db;

        public Task AddAsync(Medicamento entity, CancellationToken ct = default)
            => _db.Medicamentos.AddAsync(entity, ct).AsTask();

        public Task UpdateAsync(Medicamento entity, CancellationToken ct = default)   // NUEVO
        {
            _db.Medicamentos.Update(entity);
            return Task.CompletedTask;
        }

        public Task<Medicamento?> GetByIdAsync(int id, CancellationToken ct = default)
            => _db.Medicamentos.FirstOrDefaultAsync(x => x.IdMedicamento == id, ct);

        public Task<Medicamento?> GetByNombreAsync(string nombre, CancellationToken ct = default)
            => _db.Medicamentos.FirstOrDefaultAsync(x => x.Nombre == nombre, ct);

        public async Task<IReadOnlyList<Medicamento>> ListAllAsync(CancellationToken ct = default) // NUEVO
        {
            var list = await _db.Medicamentos.AsNoTracking().ToListAsync(ct);
            return list;
        }
    }
}