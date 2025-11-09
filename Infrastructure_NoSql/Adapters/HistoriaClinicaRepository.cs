using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using App_Hospital_Clinica.Domain.Historia_Clinica.Aggregates;
using App_Hospital_Clinica.Domain.Historia_Clinica.Ports;
using App_Hospital_Clinica.Infrastructure_NoSql.Mongo.Collections;

namespace App_Hospital_Clinica.Infrastructure_NoSql.Adapters {
    /// <summary>
    /// Repositorio documental para Historias Clínicas (MongoDB).
    /// </summary>
    public sealed class HistoriaClinicaRepository : IHistoriaClinicaRepository
    {
        private readonly IMongoCollection<HistoriaClinicaDocument> _collection;

        public HistoriaClinicaRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<HistoriaClinicaDocument>("historias");
        }

        public async Task AddAsync(HistoriaClinica entity, CancellationToken ct = default)
        {
            var doc = HistoriaClinicaDocument.FromEntity(entity);
            await _collection.InsertOneAsync(doc, cancellationToken: ct);
        }

        public async Task<HistoriaClinica?> GetByIdAsync(string id, CancellationToken ct = default)
        {
            var doc = await _collection.Find(x => x.IdHistoria == id).FirstOrDefaultAsync(ct);
            return doc?.ToEntity();
        }

        public async Task UpdateAsync(HistoriaClinica entity, CancellationToken ct = default)
        {
            var doc = HistoriaClinicaDocument.FromEntity(entity);
            await _collection.ReplaceOneAsync(x => x.IdHistoria == entity.IdHistoria, doc, cancellationToken: ct);
        }

        public async Task<IReadOnlyList<HistoriaClinica>> ListByCedulaPacienteAsync(string cedula, CancellationToken ct = default)
        {
            var docs = await _collection.Find(x => x.CedulaPaciente == cedula).ToListAsync(ct);
            return docs.Select(d => d.ToEntity()).ToList();
        }
    }
}