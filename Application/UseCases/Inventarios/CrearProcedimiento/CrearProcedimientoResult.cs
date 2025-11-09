using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Application.UseCases.Inventarios.CrearProcedimiento {
    public sealed class CrearProcedimientoResult {
        public int IdProcedimiento { get; init; }
        public string Nombre { get; init; } = default!;
    }
}