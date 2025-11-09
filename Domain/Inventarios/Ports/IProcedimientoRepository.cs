using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Domain.Inventarios.Entities;

namespace App_Hospital_Clinica.Domain.Inventarios.Ports {
    public interface IProcedimientoRepository {
        Task<Procedimiento?> GetByIdAsync(int idProcedimiento, CancellationToken ct = default);
        Task<Procedimiento?> GetByNombreAsync(string nombre, CancellationToken ct = default);
        Task<IReadOnlyList<Procedimiento>> ListAllAsync(CancellationToken ct = default);

        Task AddAsync(Procedimiento proc, CancellationToken ct = default);
        Task UpdateAsync(Procedimiento proc, CancellationToken ct = default);
    }
}