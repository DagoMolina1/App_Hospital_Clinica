using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Domain.Pacientes.Entities;

namespace App_Hospital_Clinica.Domain.Pacientes.Ports {
    public interface IContactoEmergenciaRepository {
        Task<ContactoEmergencia?> GetByPacienteIdAsync(int idPaciente, CancellationToken ct = default);

        /// <summary>
        /// Crea o reemplaza el contacto 1:1 del paciente.
        /// </summary>
        Task UpsertAsync(ContactoEmergencia contacto, CancellationToken ct = default);

        // Si prefieres separar:
        // Task AddAsync(ContactoEmergencia contacto, CancellationToken ct = default);
        // Task UpdateAsync(ContactoEmergencia contacto, CancellationToken ct = default);
    }
}