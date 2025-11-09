using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using App_Hospital_Clinica.Infrastructure_SqlServer.EfDb;
using App_Hospital_Clinica.Domain.Facturacion.Entities;
using App_Hospital_Clinica.Domain.Facturacion.Ports;

namespace App_Hospital_Clinica.Infrastructure_SqlServer.Adapters.Facturacion {
    /// <summary>
    /// ===============================================================
    /// FACTURA REPOSITORY — EF Core / SQL Server
    /// ---------------------------------------------------------------
    /// Responsabilidad:
    ///   Persistir Factura y sus Detalles; consultar por orden.
    /// 
    /// Tips de uso en Handler:
    ///  1) AddAsync(cabecera) + UoW.SaveChanges() para materializar Id.
    ///  2) Agregar Detalles al agregado y UpdateAsync.
    ///  3) UoW.SaveChanges() final.
    /// ===============================================================
    /// </summary>
    public sealed class FacturaRepository : IFacturaRepository
    {
        private readonly ClinicaDbContext _db;
        public FacturaRepository(ClinicaDbContext db) => _db = db;

        public Task AddAsync(Factura factura, CancellationToken ct = default)
            => _db.Facturas.AddAsync(factura, ct).AsTask();

        public Task UpdateAsync(Factura factura, CancellationToken ct = default)
        {
            _db.Facturas.Update(factura);
            return Task.CompletedTask;
        }

        // NUEVO
        public Task<Factura?> GetByIdAsync(int id, CancellationToken ct = default)
            => _db.Facturas
                  .Include(f => f.Detalles)
                  .FirstOrDefaultAsync(f => f.IdFactura == id, ct);

        public Task<Factura?> GetByNumeroOrdenAsync(string numeroOrden, CancellationToken ct = default)
            => _db.Facturas
                  .Include(f => f.Detalles)
                  .FirstOrDefaultAsync(f => f.NumeroOrden == numeroOrden, ct);
    }
}