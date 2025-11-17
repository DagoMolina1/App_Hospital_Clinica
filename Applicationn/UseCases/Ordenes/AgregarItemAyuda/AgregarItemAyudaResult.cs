using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Applicationn.UseCases.Ordenes.AgregarItemAyuda {
    public sealed class AgregarItemAyudaResult {
        public string NumeroOrden { get; init; } = default!;
        public int ItemN { get; init; }
        public int IdAyuda { get; init; }
        public int Cantidad { get; init; }
        public decimal Costo { get; init; }
        public decimal TotalOrden { get; init; }
    }
}