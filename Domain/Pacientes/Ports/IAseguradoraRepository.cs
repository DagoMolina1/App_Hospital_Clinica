using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Domain.Pacientes.Entities;

namespace App_Hospital_Clinica.Domain.Pacientes.Ports {
    public interface IAseguradoraRepository {
        Task<Aseguradora?> GetByIdAsync(int idAseguradora, CancellationToken ct = default);
        Task<Aseguradora?> GetByNombreAsync(string nombre, CancellationToken ct = default);

        Task AddAsync(Aseguradora aseguradora, CancellationToken ct = default);
        Task UpdateAsync(Aseguradora aseguradora, CancellationToken ct = default);

        Task<IReadOnlyList<Aseguradora>> ListAllAsync(CancellationToken ct = default);
    }
}