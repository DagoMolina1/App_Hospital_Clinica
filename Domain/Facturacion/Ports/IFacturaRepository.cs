using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Domain.Facturacion.Entities;

namespace App_Hospital_Clinica.Domain.Facturacion.Ports {
    public interface IFacturaRepository {
        Task<Factura?> GetByIdAsync(int idFactura, CancellationToken ct = default);
        Task<Factura?> GetByNumeroOrdenAsync(string numeroOrden, CancellationToken ct = default);

        Task AddAsync(Factura factura, CancellationToken ct = default);
        Task UpdateAsync(Factura factura, CancellationToken ct = default);
    }
}