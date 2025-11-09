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
    /// <summary>CRUD básico para catálogo de Procedimientos.</summary>
    public sealed class ProcedimientoRepository : IProcedimientoRepository
    {
        private readonly ClinicaDbContext _db;
        public ProcedimientoRepository(ClinicaDbContext db) => _db = db;

        public Task AddAsync(Procedimiento entity, CancellationToken ct = default)
            => _db.Procedimientos.AddAsync(entity, ct).AsTask();

        public Task UpdateAsync(Procedimiento entity, CancellationToken ct = default) // NUEVO
        {
            _db.Procedimientos.Update(entity);
            return Task.CompletedTask;
        }

        public Task<Procedimiento?> GetByIdAsync(int id, CancellationToken ct = default)
            => _db.Procedimientos.FirstOrDefaultAsync(x => x.IdProcedimiento == id, ct);

        public Task<Procedimiento?> GetByNombreAsync(string nombre, CancellationToken ct = default)
            => _db.Procedimientos.FirstOrDefaultAsync(x => x.Nombre == nombre, ct);

        public async Task<IReadOnlyList<Procedimiento>> ListAllAsync(CancellationToken ct = default) // NUEVO
        {
            var list = await _db.Procedimientos.AsNoTracking().ToListAsync(ct);
            return list;
        }
    }
}