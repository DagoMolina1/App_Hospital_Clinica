using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using App_Hospital_Clinica.Infrastructure_SqlServer.EfDb;
using App_Hospital_Clinica.Domain.RRHH.Entities;
using App_Hospital_Clinica.Domain.RRHH.Ports;

namespace App_Hospital_Clinica.Infrastructure_SqlServer.Adapters.RRHH
{
    /// <summary>
    /// Persistencia de Usuario (RRHH).
    /// - Cedula única (índice).
    /// - Correo opcional (string o VO según tu dominio + config).
    /// </summary>
    public sealed class UsuarioRepository : IUsuarioRepository
    {
        private readonly ClinicaDbContext _db;
        public UsuarioRepository(ClinicaDbContext db) => _db = db;

        public Task AddAsync(Usuario u, CancellationToken ct = default)
            => _db.Usuarios.AddAsync(u, ct).AsTask();

        public Task UpdateAsync(Usuario u, CancellationToken ct = default)
        {
            _db.Usuarios.Update(u);
            return Task.CompletedTask;
        }

        public Task<Usuario?> GetByIdAsync(int id, CancellationToken ct = default) // NUEVO
            => _db.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == id, ct);

        public Task<Usuario?> GetByCedulaAsync(string cedula, CancellationToken ct = default) // NUEVO
            => _db.Usuarios.FirstOrDefaultAsync(x => x.Cedula == cedula, ct);

        public Task<bool> ExistsByCedulaAsync(string cedula, CancellationToken ct = default)
            => _db.Usuarios.AnyAsync(x => x.Cedula == cedula, ct);

        public async Task AssignRoleAsync(int idUsuario, int idRol, CancellationToken ct = default)
        {
            var link = new UsuarioRol { IdUsuario = idUsuario, IdRol = idRol }; // requiere patch de accesos (abajo)
            await _db.UsuarioRoles.AddAsync(link, ct);
        }

        public async Task RemoveRoleAsync(int idUsuario, int idRol, CancellationToken ct = default) // NUEVO
        {
            var link = await _db.UsuarioRoles.FirstOrDefaultAsync(x => x.IdUsuario == idUsuario && x.IdRol == idRol, ct);
            if (link is not null) _db.UsuarioRoles.Remove(link);
        }

        public async Task<IReadOnlyList<Rol>> GetRolesAsync(int idUsuario, CancellationToken ct = default) // NUEVO
        {
            var roles = await _db.UsuarioRoles
                .Where(ur => ur.IdUsuario == idUsuario)
                .Join(_db.Roles, ur => ur.IdRol, r => r.IdRol, (ur, r) => r)
                .AsNoTracking()
                .ToListAsync(ct);

            return roles;
        }
    }
}
