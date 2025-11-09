using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using App_Hospital_Clinica.Domain.RRHH.Entities;

namespace App_Hospital_Clinica.Infrastructure_SqlServer.EfDb.Configurations {
    /*public sealed class UsuarioConfig : IEntityTypeConfiguration<Usuario> {
        public void Configure(EntityTypeBuilder<Usuario> b) {
            b.ToTable("Usuario");
            b.HasKey(x => x.IdUsuario);

            // Cedula como string simple (si fuera VO, usar OwnsOne igual que Paciente)
            b.Property(x => x.Cedula).IsRequired().HasMaxLength(15);
            b.HasIndex(x => x.Cedula).IsUnique();

            b.Property(x => x.NombreCompleto).IsRequired().HasMaxLength(100);

            // Correo como VO opcional o string; ajusta según tu dominio
            b.OwnsOne(x => x.Correo, eb => {
                eb.Property(p => p.Value)
                  .HasColumnName("Correo")
                  .HasMaxLength(100)
                  .IsRequired(false);
            });
        }
    }

    public sealed class RolConfig : IEntityTypeConfiguration<Rol> {
        public void Configure(EntityTypeBuilder<Rol> b) {
            b.ToTable("Rol");
            b.HasKey(x => x.IdRol);
            b.Property(x => x.NombreRol).IsRequired().HasMaxLength(50);
            b.HasIndex(x => x.NombreRol).IsUnique();
        }
    }

    public sealed class UsuarioRolConfig : IEntityTypeConfiguration<UsuarioRol> {
        public void Configure(EntityTypeBuilder<UsuarioRol> b) {
            b.ToTable("UsuarioRol");
            b.HasKey(x => new { x.IdUsuario, x.IdRol });

            b.HasOne<Usuario>()
             .WithMany()
             .HasForeignKey(x => x.IdUsuario)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasOne<Rol>()
             .WithMany()
             .HasForeignKey(x => x.IdRol)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }*/
    public sealed class UsuarioConfig : IEntityTypeConfiguration<Usuario> {
        public void Configure(EntityTypeBuilder<Usuario> b) {
            b.ToTable("Usuario");
            b.HasKey(x => x.IdUsuario);

            // Cedula string (si fuera VO -> usar OwnsOne como en Paciente)
            b.Property(x => x.Cedula)
             .IsRequired()
             .HasMaxLength(15);

            b.HasIndex(x => x.Cedula).IsUnique();

            b.Property(x => x.NombreCompleto)
             .IsRequired()
             .HasMaxLength(100);

            // Correo string opcional
            b.Property(x => x.Correo)
             .HasColumnName("Correo")
             .HasMaxLength(100)
             .IsRequired(false);
        }
    }
}