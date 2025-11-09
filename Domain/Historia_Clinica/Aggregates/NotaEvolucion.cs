using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Domain.Historia_Clinica.Aggregates {
    /// <summary>
    /// Subdocumento que representa una nota de evolución médica.
    /// Se embebe dentro del documento de Historia Clínica.
    /// </summary>
    public class NotaEvolucion {
        public string IdNotaEvolucion { get; private set; } = default!;
        public DateTime Fecha { get; private set; }
        public string Medico { get; private set; } = default!;
        public string Nota { get; private set; } = default!;

        protected NotaEvolucion() { }

        public NotaEvolucion(string id, DateTime fecha, string medico, string nota) {
            IdNotaEvolucion = id;
            Fecha = fecha;
            Medico = medico;
            Nota = nota;
        }
    }
}