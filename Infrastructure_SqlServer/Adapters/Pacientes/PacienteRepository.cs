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
    /// Persistencia del agregado Paciente.
    /// - Usa VO: Cedula/Telefono/Email (ver Config con OwnsOne).
    /// - Consultas por cédula comparan contra VO.Value.
    /// </summary>
    public sealed class PacienteRepository : IPacienteRepository
    {
        private readonly ClinicaDbContext _db;
        public PacienteRepository(ClinicaDbContext db) => _db = db;

        public Task AddAsync(Paciente paciente, CancellationToken ct = default)
            => _db.Pacientes.AddAsync(paciente, ct).AsTask();

        public Task UpdateAsync(Paciente paciente, CancellationToken ct = default)
        {
            _db.Pacientes.Update(paciente);
            return Task.CompletedTask;
        }

        public Task<Paciente?> GetByIdAsync(int id, CancellationToken ct = default) // NUEVO
            => _db.Pacientes.FirstOrDefaultAsync(p => p.IdPaciente == id, ct);

        public Task<bool> ExistsByCedulaAsync(string cedula, CancellationToken ct = default)
            => _db.Pacientes.AnyAsync(p => p.Cedula.Value == cedula, ct);

        public Task<Paciente?> FindByCedulaAsync(string cedula, CancellationToken ct = default)
            => _db.Pacientes.FirstOrDefaultAsync(p => p.Cedula.Value == cedula, ct);
    }
}