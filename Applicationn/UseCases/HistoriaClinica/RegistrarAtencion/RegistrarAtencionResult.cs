using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Applicationn.UseCases.HistoriaClinica.RegistrarAtencion {
    public sealed class RegistrarAtencionResult {
        public string IdHistoria { get; init; } = default!;
        public DateTime FechaAtencion { get; init; }
        public string MedicoResponsable { get; init; } = default!;
        public string IdNotaEvolucion { get; init; } = default!;
    }
}