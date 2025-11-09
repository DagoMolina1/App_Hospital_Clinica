using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Application.Abstractions {
    /// <summary>
    /// ===============================================================
    ///  IUNITOFWORK — Guía rápida
    /// ---------------------------------------------------------------
    /// Propósito:
    ///   Coordinar la persistencia atómica de cambios a través de múltiples
    ///   repositorios/puertos durante la ejecución de un Caso de Uso.
    ///
    /// Cuándo usarlo:
    ///   - Al final de un UseCase, después de invocar Add/Update/Delete en repos.
    ///   - Para asegurar que todo se guarda o nada se guarda (transacción).
    ///
    /// Entradas/Salidas:
    ///   - SaveChangesAsync(): confirma cambios.
    ///   - BeginTransactionAsync()/Commit/Rollback: opcional para escenarios
    ///     avanzados (orquestaciones de múltiples pasos).
    ///
    /// Precondiciones (Application):
    ///   - Ya ejecutaste las operaciones en los repositorios.
    ///   - Validaciones y Policies pasaron correctamente.
    ///
    /// Postcondiciones:
    ///   - Cambios confirmados en la infraestructura (BD).
    ///
    /// Checklist de uso (UseCase):
    ///   [ ] Ejecutar reglas + repositorios.
    ///   [ ] await _uow.SaveChangesAsync(ct);
    ///   (Opcional con transacción)
    ///   [ ] await using var tx = await _uow.BeginTransactionAsync(ct);
    ///   [ ] ... repos ...
    ///   [ ] await _uow.SaveChangesAsync(ct);
    ///   [ ] await tx.CommitAsync(ct);
    /// ===============================================================
    /// </summary>
    public interface IUnitOfWork {
        Task<int> SaveChangesAsync(CancellationToken ct = default);

        /// <summary>
        /// Inicia una transacción explícita (opcional).
        /// Infra puede devolver un wrapper que implemente IAsyncDisposable.
        /// </summary>
        Task<IUnitOfWorkTransaction> BeginTransactionAsync(CancellationToken ct = default);
    }

    /// <summary>
    /// Wrapper mínimo de transacción para usarse con `await using`.
    /// La implementación real vive en Infrastructure.
    /// </summary>
    public interface IUnitOfWorkTransaction : IAsyncDisposable {
        Task CommitAsync(CancellationToken ct = default);
        Task RollbackAsync(CancellationToken ct = default);
    }
}