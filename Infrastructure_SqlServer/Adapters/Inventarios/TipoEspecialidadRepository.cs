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
    /// <summary>Lectura por Id para catálogo de Tipos de Especialidad.</summary>
    public sealed class TipoEspecialidadRepository : ITipoEspecialidadRepository
    {
        private readonly ClinicaDbContext _db;
        public TipoEspecialidadRepository(ClinicaDbContext db) => _db = db;

        public Task AddAsync(TipoEspecialidad entity, CancellationToken ct = default) // NUEVO
            => _db.TiposEspecialidad.AddAsync(entity, ct).AsTask();

        public Task UpdateAsync(TipoEspecialidad entity, CancellationToken ct = default) // NUEVO
        {
            _db.TiposEspecialidad.Update(entity);
            return Task.CompletedTask;
        }

        public Task<TipoEspecialidad?> GetByIdAsync(int id, CancellationToken ct = default)
            => _db.TiposEspecialidad.FirstOrDefaultAsync(x => x.IdTipoEspecialidad == id, ct);

        public Task<TipoEspecialidad?> GetByNombreAsync(string nombre, CancellationToken ct = default) // NUEVO
            => _db.TiposEspecialidad.FirstOrDefaultAsync(x => x.Nombre == nombre, ct);

        public async Task<IReadOnlyList<TipoEspecialidad>> ListAllAsync(CancellationToken ct = default) // NUEVO
        {
            var list = await _db.TiposEspecialidad.AsNoTracking().ToListAsync(ct);
            return list;
        }
    }
}