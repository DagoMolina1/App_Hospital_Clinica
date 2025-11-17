using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Applicationn.UseCases.HistoriaClinica.CompletarDiagnosticoYTratamiento {
    public sealed class CompletarDiagnosticoYTratamientoResult {
        public string IdHistoria { get; init; } = default!;
        public string Diagnostico { get; init; } = default!;
        public string Tratamiento { get; init; } = default!;
        public DateTime FechaActualizacion { get; init; }
    }
}