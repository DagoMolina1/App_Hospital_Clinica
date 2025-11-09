using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Domain.Inventarios.Entities;

namespace App_Hospital_Clinica.Domain.Inventarios.Ports {
    public interface ITipoEspecialidadRepository {
        Task<TipoEspecialidad?> GetByIdAsync(int idTipoEspecialidad, CancellationToken ct = default);
        Task<TipoEspecialidad?> GetByNombreAsync(string nombre, CancellationToken ct = default);
        Task<IReadOnlyList<TipoEspecialidad>> ListAllAsync(CancellationToken ct = default);

        Task AddAsync(TipoEspecialidad esp, CancellationToken ct = default);
        Task UpdateAsync(TipoEspecialidad esp, CancellationToken ct = default);
    }
}