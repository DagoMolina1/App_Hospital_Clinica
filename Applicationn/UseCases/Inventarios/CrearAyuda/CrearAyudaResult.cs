using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Applicationn.UseCases.Inventarios.CrearAyuda {
    public sealed class CrearAyudaResult {
        public int IdAyuda { get; init; }
        public string Nombre { get; init; } = default!;
    }
}