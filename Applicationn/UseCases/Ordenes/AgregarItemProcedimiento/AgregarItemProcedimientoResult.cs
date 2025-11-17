using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Applicationn.UseCases.Ordenes.AgregarItemProcedimiento {
    public sealed class AgregarItemProcedimientoResult {
        public string NumeroOrden { get; init; } = default!;
        public int ItemN { get; init; }
        public int IdProcedimiento { get; init; }
        public bool RequiereEspecialista { get; init; }
        public int? IdTipoEspecialidad { get; init; }
        public decimal Costo { get; init; }
        public decimal TotalOrden { get; init; }
    }
}