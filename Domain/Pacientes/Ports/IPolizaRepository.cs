using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Domain.Pacientes.Entities;

namespace App_Hospital_Clinica.Domain.Pacientes.Ports {
    public interface IPolizaRepository {
        Task<Poliza?> GetByPacienteIdAsync(int idPaciente, CancellationToken ct = default);
        Task<Poliza?> GetByNumeroAsync(string numeroPoliza, CancellationToken ct = default);

        Task AddAsync(Poliza poliza, CancellationToken ct = default);
        Task UpdateAsync(Poliza poliza, CancellationToken ct = default);
    }
}