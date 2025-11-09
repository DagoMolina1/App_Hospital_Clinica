using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Application.Abstractions;
using App_Hospital_Clinica.Domain.Common;
using App_Hospital_Clinica.Domain.RRHH.Entities;
using App_Hospital_Clinica.Domain.RRHH.Ports;

namespace App_Hospital_Clinica.Application.UseCases.RRHH.CrearUsuario {
    /// <summary>
    /// ===============================================================
    ///  CREAR USUARIO — Handler — Guía rápida
    /// ---------------------------------------------------------------
    /// Flujo:
    ///  [1] Validar shape del DTO (longitudes / formato correo).
    ///  [2] Verificar unicidad por cédula (IUsuarioRepository).
    ///  [3] Dominio: Usuario.Create(...).
    ///  [4] Guardar usuario (repo + UoW).
    ///  [5] (Opcional) Asignar roles existentes por nombre.
    ///  [6] Confirmar cambios y devolver Result.
    ///
    /// Entradas : CrearUsuarioCommand
    /// Salidas  : Result<CrearUsuarioResult>
    ///
    /// Precondiciones:
    ///  - Repos inyectados: IUsuarioRepository, IRolRepository.
    ///  - IUnitOfWork e ILogger disponibles.
    ///
    /// Postcondiciones:
    ///  - Usuario creado y roles asignados si fueron provistos.
    /// ===============================================================
    /// </summary>
    public sealed class CrearUsuarioHandler {
        private readonly IUsuarioRepository _usuarios;
        private readonly IRolRepository _roles;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<CrearUsuarioHandler> _logger;

        public CrearUsuarioHandler(IUsuarioRepository usuarios, IRolRepository roles, IUnitOfWork uow, ILogger<CrearUsuarioHandler> logger) {
            _usuarios = usuarios;
            _roles = roles;
            _uow = uow;
            _logger = logger;
        }

        public async Task<Result<CrearUsuarioResult>> Handle(CrearUsuarioCommand cmd, CancellationToken ct = default) {
            _logger.LogInformation("Creando usuario con cédula {Cedula}", cmd.Cedula);

            // [1] Validaciones mínimas del DTO
            if (string.IsNullOrWhiteSpace(cmd.Cedula))
                return Result<CrearUsuarioResult>.Fail("La cédula es requerida.");

            if (string.IsNullOrWhiteSpace(cmd.NombreCompleto) || cmd.NombreCompleto.Trim().Length > 100)
                return Result<CrearUsuarioResult>.Fail("Nombre inválido (requerido, máx 100).");

            if (cmd.Correo is not null && cmd.Correo.Length > 100)
                return Result<CrearUsuarioResult>.Fail("El correo supera la longitud máxima (100).");

            // Validación simple de correo (opcional; el Dominio puede reforzar)
            if (!string.IsNullOrWhiteSpace(cmd.Correo) && !cmd.Correo.Contains('@'))
                return Result<CrearUsuarioResult>.Fail("El formato del correo es inválido.");

            // [2] Unicidad por cédula
            if (await _usuarios.ExistsByCedulaAsync(cmd.Cedula.Trim(), ct))
                return Result<CrearUsuarioResult>.Fail("Ya existe un usuario con esa cédula.");

            // [3] Dominio: crear entidad protegida por invariantes
            var crear = Usuario.Create(
                cedula: cmd.Cedula.Trim(),
                nombreCompleto: cmd.NombreCompleto.Trim(),
                correo: string.IsNullOrWhiteSpace(cmd.Correo) ? null : cmd.Correo.Trim()
            );
            if (!crear.IsSuccess)
                return Result<CrearUsuarioResult>.Fail(crear.Error!);

            var usuario = crear.Value!;

            // [4] Guardar usuario
            await _usuarios.AddAsync(usuario, ct);
            await _uow.SaveChangesAsync(ct); // materializa IdUsuario

            // [5] Asignación de roles (si se enviaron)
            var asignados = new List<string>();
            if (cmd.Roles is { Length: > 0 }) {
                foreach (var nombreRol in cmd.Roles) {
                    if (string.IsNullOrWhiteSpace(nombreRol)) continue;

                    var rol = await _roles.GetByNombreAsync(nombreRol.Trim(), ct);
                    if (rol is null) {
                        _logger.LogWarning("Rol {Rol} no existe. Se omite asignación.", nombreRol);
                        continue; // no creamos roles aquí
                    }

                    await _usuarios.AssignRoleAsync(usuario.IdUsuario, rol.IdRol, ct);
                    asignados.Add(rol.NombreRol);
                }

                await _uow.SaveChangesAsync(ct);
            }

            _logger.LogInformation("Usuario {Id} creado con {N} rol(es).", usuario.IdUsuario, asignados.Count);

            // [6] Salida
            return Result<CrearUsuarioResult>.Ok(new CrearUsuarioResult {
                IdUsuario = usuario.IdUsuario,
                Cedula = usuario.Cedula,
                NombreCompleto = usuario.NombreCompleto,
                Correo = usuario.Correo,
                RolesAsignados = asignados.ToArray()
            });
        }
    }
}