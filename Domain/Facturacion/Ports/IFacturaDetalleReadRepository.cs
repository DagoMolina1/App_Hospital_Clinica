using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Domain.Facturacion.Entities;

namespace App_Hospital_Clinica.Domain.Facturacion.Ports {
    public interface IFacturaDetalleReadRepository {
        Task<IReadOnlyList<FacturaDetalle>> ListByFacturaIdAsync(int idFactura, CancellationToken ct = default);
        Task<IReadOnlyList<FacturaDetalle>> ListByNumeroOrdenAsync(string numeroOrden, CancellationToken ct = default);
    }
}