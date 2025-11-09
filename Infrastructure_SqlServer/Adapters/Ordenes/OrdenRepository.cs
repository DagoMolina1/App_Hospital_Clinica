using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using App_Hospital_Clinica.Infrastructure_SqlServer.EfDb;
using App_Hospital_Clinica.Domain.Ordenes.Entities;
using App_Hospital_Clinica.Domain.Ordenes.Ports;

namespace App_Hospital_Clinica.Infrastructure_SqlServer.Adapters.Ordenes {
    /// <summary>
    /// ===============================================================
    /// ORDEN REPOSITORY — EF Core / SQL Server
    /// ---------------------------------------------------------------
    /// Responsabilidad:
    ///   Persistir y consultar el agregado Orden + sus Items.
    /// 
    /// Notas:
    ///  - NO invoca SaveChanges (eso lo hace IUnitOfWork).
    ///  - Incluye Items cuando carga por número (para Handlers).
    /// ===============================================================
    /// </summary>
    public sealed class OrdenRepository : IOrdenRepository
    {
        private readonly ClinicaDbContext _db;
        public OrdenRepository(ClinicaDbContext db) => _db = db;

        public Task AddAsync(Orden orden, CancellationToken ct = default)
            => _db.Ordenes.AddAsync(orden, ct).AsTask();

        public Task UpdateAsync(Orden orden, CancellationToken ct = default)
        {
            _db.Ordenes.Update(orden);
            return Task.CompletedTask;
        }

        public Task<Orden?> GetByNumeroAsync(string numeroOrden, CancellationToken ct = default)
            => _db.Ordenes
                  .Include(o => o.Items)
                  .FirstOrDefaultAsync(o => o.NumeroOrden == numeroOrden, ct);

        // NUEVO
        public Task<bool> ExistsAsync(string numeroOrden, CancellationToken ct = default)
            => _db.Ordenes.AnyAsync(o => o.NumeroOrden == numeroOrden, ct);

        // Ajuste a IReadOnlyList
        public async Task<IReadOnlyList<Orden>> ListByCedulaPacienteAsync(string cedula, CancellationToken ct = default)
        {
            // Si CedulaPaciente es string en dominio, usa o.CedulaPaciente == cedula
            var list = await _db.Ordenes.AsNoTracking()
                         .Where(o => o.CedulaPaciente.Value == cedula)
                         .ToListAsync(ct);
            return list;
        }
    }
}