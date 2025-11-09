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
    /// Upsert 1:1 de contacto de emergencia por IdPaciente.
    /// - Si no existe, crea; si existe, actualiza.
    /// </summary>
    public sealed class ContactoEmergenciaRepository : IContactoEmergenciaRepository {
        private readonly ClinicaDbContext _db;
        public ContactoEmergenciaRepository(ClinicaDbContext db) => _db = db;

        public async Task UpsertAsync(ContactoEmergencia entity, CancellationToken ct = default)
        {
            var exists = await _db.Contactos.AsNoTracking()
                .AnyAsync(x => x.IdPaciente == entity.IdPaciente, ct);

            if (!exists) await _db.Contactos.AddAsync(entity, ct);
            else _db.Contactos.Update(entity);
        }

        public Task<ContactoEmergencia?> GetByPacienteIdAsync(int idPaciente, CancellationToken ct = default)
            => _db.Contactos.FirstOrDefaultAsync(x => x.IdPaciente == idPaciente, ct);
    }
}