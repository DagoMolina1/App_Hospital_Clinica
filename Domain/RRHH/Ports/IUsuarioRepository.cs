using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Domain.RRHH.Entities;

namespace App_Hospital_Clinica.Domain.RRHH.Ports {
    public interface IUsuarioRepository {
        Task<Usuario?> GetByIdAsync(int idUsuario, CancellationToken ct = default);
        Task<Usuario?> GetByCedulaAsync(string cedula, CancellationToken ct = default);
        Task<bool> ExistsByCedulaAsync(string cedula, CancellationToken ct = default);

        Task AddAsync(Usuario usuario, CancellationToken ct = default);
        Task UpdateAsync(Usuario usuario, CancellationToken ct = default);

        // Manejo de roles del usuario (N:N)
        Task AssignRoleAsync(int idUsuario, int idRol, CancellationToken ct = default);
        Task RemoveRoleAsync(int idUsuario, int idRol, CancellationToken ct = default);
        Task<IReadOnlyList<Rol>> GetRolesAsync(int idUsuario, CancellationToken ct = default);
    }
}