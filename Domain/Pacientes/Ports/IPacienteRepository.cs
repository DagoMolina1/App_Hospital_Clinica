using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Domain.Pacientes.Entities;

namespace App_Hospital_Clinica.Domain.Pacientes.Ports {
    public interface IPacienteRepository {
        Task<bool> ExistsByCedulaAsync(string cedula, CancellationToken ct = default);
        Task<Paciente?> FindByCedulaAsync(string cedula, CancellationToken ct = default);
        Task<Paciente?> GetByIdAsync(int idPaciente, CancellationToken ct = default);

        Task AddAsync(Paciente paciente, CancellationToken ct = default);
        Task UpdateAsync(Paciente paciente, CancellationToken ct = default);
        // Delete rara vez en clínica; si lo necesitas:
        // Task DeleteAsync(int idPaciente, CancellationToken ct = default);
    }
}