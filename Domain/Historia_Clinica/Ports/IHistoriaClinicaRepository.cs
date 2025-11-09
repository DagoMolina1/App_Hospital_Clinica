using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Domain.Historia_Clinica.Aggregates;

namespace App_Hospital_Clinica.Domain.Historia_Clinica.Ports {
    public interface IHistoriaClinicaRepository {
        Task<HistoriaClinica?> GetByIdAsync(string idHistoria, CancellationToken ct = default);
        Task<IReadOnlyList<HistoriaClinica>> ListByCedulaPacienteAsync(string cedulaPaciente, CancellationToken ct = default);

        Task AddAsync(HistoriaClinica doc, CancellationToken ct = default);
        Task UpdateAsync(HistoriaClinica doc, CancellationToken ct = default);
    }
}