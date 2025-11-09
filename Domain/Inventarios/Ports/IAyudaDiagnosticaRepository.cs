using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Domain.Inventarios.Entities;

namespace App_Hospital_Clinica.Domain.Inventarios.Ports {
    public interface IAyudaDiagnosticaRepository {
        Task<AyudaDiagnostica?> GetByIdAsync(int idAyuda, CancellationToken ct = default);
        Task<AyudaDiagnostica?> GetByNombreAsync(string nombre, CancellationToken ct = default);
        Task<IReadOnlyList<AyudaDiagnostica>> ListAllAsync(CancellationToken ct = default);

        Task AddAsync(AyudaDiagnostica ayuda, CancellationToken ct = default);
        Task UpdateAsync(AyudaDiagnostica ayuda, CancellationToken ct = default);
    }
}