using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using App_Hospital_Clinica.Infrastructure_SqlServer.EfDb;
using App_Hospital_Clinica.Domain.RRHH.Entities;
using App_Hospital_Clinica.Domain.RRHH.Ports;

namespace App_Hospital_Clinica.Infrastructure_SqlServer.Adapters.RRHH {
    /// <summary>
    /// Lectura de Roles por nombre (para asignación).
    /// </summary>
    public sealed class RolRepository : IRolRepository
    {
        private readonly ClinicaDbContext _db;
        public RolRepository(ClinicaDbContext db) => _db = db;

        public Task AddAsync(Rol rol, CancellationToken ct = default) // NUEVO
            => _db.Roles.AddAsync(rol, ct).AsTask();

        public Task UpdateAsync(Rol rol, CancellationToken ct = default) // NUEVO
        {
            _db.Roles.Update(rol);
            return Task.CompletedTask;
        }

        public Task<Rol?> GetByIdAsync(int id, CancellationToken ct = default) // NUEVO
            => _db.Roles.FirstOrDefaultAsync(r => r.IdRol == id, ct);

        public Task<Rol?> GetByNombreAsync(string nombre, CancellationToken ct = default)
            => _db.Roles.FirstOrDefaultAsync(r => r.NombreRol == nombre, ct);

        public async Task<IReadOnlyList<Rol>> ListAllAsync(CancellationToken ct = default) // NUEVO
        {
            var list = await _db.Roles.AsNoTracking().ToListAsync(ct);
            return list;
        }
    }
}