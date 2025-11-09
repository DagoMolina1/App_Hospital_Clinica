using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Domain.Ordenes.Entities;

namespace App_Hospital_Clinica.Domain.Ordenes.Ports {
    public interface IOrdenRepository {
        Task<bool> ExistsAsync(string numeroOrden, CancellationToken ct = default);
        Task<Orden?> GetByNumeroAsync(string numeroOrden, CancellationToken ct = default);

        Task AddAsync(Orden orden, CancellationToken ct = default);
        Task UpdateAsync(Orden orden, CancellationToken ct = default);

        // Consultas de apoyo
        Task<IReadOnlyList<Orden>> ListByCedulaPacienteAsync(string cedulaPaciente, CancellationToken ct = default);
    }
}