using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using App_Hospital_Clinica.Infrastructure_SqlServer.EfDb;
using App_Hospital_Clinica.Domain.Pacientes.Entities;
using App_Hospital_Clinica.Domain.Pacientes.Ports;

namespace App_Hospital_Clinica.Infrastructure_SqlServer.Adapters.Pacientes {
    /// <summary>
    /// Acceso a Póliza 1:1 por IdPaciente y búsqueda por número de póliza.
    /// </summary>
    public sealed class PolizaRepository : IPolizaRepository
    {
        private readonly ClinicaDbContext _db;
        public PolizaRepository(ClinicaDbContext db) => _db = db;

        public Task AddAsync(Poliza entity, CancellationToken ct = default)
            => _db.Polizas.AddAsync(entity, ct).AsTask();

        public Task UpdateAsync(Poliza entity, CancellationToken ct = default) // NUEVO
        {
            _db.Polizas.Update(entity);
            return Task.CompletedTask;
        }

        public Task<Poliza?> GetByPacienteIdAsync(int idPaciente, CancellationToken ct = default)
            => _db.Polizas.FirstOrDefaultAsync(x => x.IdPaciente == idPaciente, ct);

        public Task<Poliza?> GetByNumeroAsync(string numero, CancellationToken ct = default)
            => _db.Polizas.FirstOrDefaultAsync(x => x.NumeroPoliza == numero, ct);
    }
}