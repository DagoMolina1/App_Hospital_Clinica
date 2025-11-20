using System;
using System.Threading;
using System.Threading.Tasks;
using App_Hospital_Clinica.Applicationn.Abstractions;
using App_Hospital_Clinica.Infrastructure_SqlServer.EfDb;

namespace App_Hospital_Clinica.Infrastructure_SqlServer.Common {
    /// <summary>
    /// Implementación de IUnitOfWork usando ClinicaDbContext.
    /// Se encarga de confirmar cambios en la BD y expone un hook
    /// para manejar transacciones explícitas si algún caso de uso lo requiere.
    /// </summary>
    public sealed class EfUnitOfWork : IUnitOfWork {
        private readonly ClinicaDbContext _db;

        public EfUnitOfWork(ClinicaDbContext db) {
            _db = db;
        }

        /// <summary>
        /// Guarda los cambios pendientes en la base de datos.
        /// Devuelve el número de filas afectadas (igual que EF Core).
        /// </summary>
        public Task<int> SaveChangesAsync(CancellationToken ct = default) {
            return _db.SaveChangesAsync(ct);
        }

        /// <summary>
        /// Inicio de transacción explícita.
        /// De momento no está implementado porque ningún flujo actual
        /// la necesita; si más adelante quieres usar transacciones
        /// reales, aquí haremos el adaptador a IUnitOfWorkTransaction.
        /// </summary>
        public Task<IUnitOfWorkTransaction> BeginTransactionAsync(CancellationToken ct = default) {
            throw new NotImplementedException(
                "BeginTransactionAsync aún no está implementado en EfUnitOfWork. " +
                "Si algún caso de uso lo requiere, habrá que crear un adaptador " +
                "que envuelva la transacción de EF Core en un IUnitOfWorkTransaction."
            );
        }
    }
}