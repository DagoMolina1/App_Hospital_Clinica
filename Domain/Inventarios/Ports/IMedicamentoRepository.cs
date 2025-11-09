using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Domain.Inventarios.Entities;

namespace App_Hospital_Clinica.Domain.Inventarios.Ports {
    public interface IMedicamentoRepository {
        Task<Medicamento?> GetByIdAsync(int idMedicamento, CancellationToken ct = default);
        Task<Medicamento?> GetByNombreAsync(string nombre, CancellationToken ct = default);
        Task<IReadOnlyList<Medicamento>> ListAllAsync(CancellationToken ct = default);

        Task AddAsync(Medicamento med, CancellationToken ct = default);
        Task UpdateAsync(Medicamento med, CancellationToken ct = default);
    }
}