using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Domain.RRHH.Entities;

namespace App_Hospital_Clinica.Domain.RRHH.Ports {
    public interface IRolRepository {
        Task<Rol?> GetByIdAsync(int idRol, CancellationToken ct = default);
        Task<Rol?> GetByNombreAsync(string nombreRol, CancellationToken ct = default);

        Task AddAsync(Rol rol, CancellationToken ct = default);
        Task UpdateAsync(Rol rol, CancellationToken ct = default);

        Task<IReadOnlyList<Rol>> ListAllAsync(CancellationToken ct = default);
    }
}