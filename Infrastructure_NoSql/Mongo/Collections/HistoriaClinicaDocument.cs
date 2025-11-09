using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using App_Hospital_Clinica.Domain.Historia_Clinica.Aggregates;
using App_Hospital_Clinica.Domain.Pacientes.ValueObjects; // Cedula VO

namespace App_Hospital_Clinica.Infrastructure_NoSql.Mongo.Collections {
    /// <summary>
    /// Documento MongoDB para Historia Clínica.
    /// </summary>
    public sealed class HistoriaClinicaDocument {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("idHistoria")]
        public string IdHistoria { get; set; } = default!;

        // En BD documental se guarda como string; en dominio es VO Cedula
        [BsonElement("cedulaPaciente")]
        public string CedulaPaciente { get; set; } = default!;

        [BsonElement("diagnostico")]
        public string Diagnostico { get; set; } = default!;

        [BsonElement("tratamiento")]
        public string Tratamiento { get; set; } = default!;

        // Si tu entidad usa otro nombre (p. ej. Fecha), ajústalo en FromEntity/ToEntity
        [BsonElement("fechaRegistro")]
        public DateTime FechaRegistro { get; set; }

        // ---- Entity -> Document ----
        public static HistoriaClinicaDocument FromEntity(HistoriaClinica e) => new() {
            IdHistoria = e.IdHistoria,
            CedulaPaciente = e.CedulaPaciente.Value,  // VO -> string
            Diagnostico = e.Diagnostico,
            Tratamiento = e.Tratamiento,
            // Si en tu entidad se llama Fecha (o FechaCreacion), úsala aquí:
            FechaRegistro = e.FechaRegistro
        };

        // ---- Document -> Entity ----
        public HistoriaClinica ToEntity()
            => HistoriaClinica.Create(
                idHistoria: IdHistoria,
                cedulaPaciente: Cedula.Create(CedulaPaciente).Value!, // string -> VO
                diagnostico: Diagnostico,
                tratamiento: Tratamiento,
                fechaRegistro: FechaRegistro
            ).Value!;
    }
}