using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using App_Hospital_Clinica.Domain.Pacientes.Entities;

namespace App_Hospital_Clinica.Infrastructure_SqlServer.EfDb.Configurations {
    /// <summary>
    /// Mapea Paciente (PK IdPaciente) + Value Objects (Cedula, Telefono, Email).
    /// Tabla: clinica.Paciente
    /// </summary>
    public class PacienteConfig : IEntityTypeConfiguration<Paciente> {
        public void Configure(EntityTypeBuilder<Paciente> b) {
            b.ToTable("Paciente");                    // esquema por defecto lo pone DbContext
            b.HasKey(x => x.IdPaciente);              // PK: IdPaciente (identity)

            b.Property(x => x.IdPaciente)
             .HasColumnName("IdPaciente");

            b.Property(x => x.NombreCompleto)
             .IsRequired()
             .HasMaxLength(100);

            b.Property(x => x.FechaNac)
             .IsRequired();

            b.Property(x => x.Genero)
             .IsRequired()
             .HasMaxLength(20);

            b.Property(x => x.Direccion)
             .IsRequired()
             .HasMaxLength(120);

            // ---- Owned types ----
            // Cedula → columna "Cedula" (única)
            b.OwnsOne(x => x.Cedula, cb => {
                cb.Property(p => p.Value)
                  .HasColumnName("Cedula")
                  .HasMaxLength(15)
                  .IsRequired();

                cb.HasIndex(p => p.Value).IsUnique(); // unicidad de cédula
            });

            // Telefono → columna "Telefono"
            b.OwnsOne(x => x.Telefono, tb => {
                tb.Property(p => p.Value)
                  .HasColumnName("Telefono")
                  .HasMaxLength(10)
                  .IsRequired();
            });

            // Email (opcional) → columna "Correo"
            b.OwnsOne(x => x.Correo, eb => {
                eb.Property(p => p.Value)
                  .HasColumnName("Correo")
                  .HasMaxLength(100)
                  .IsRequired(false);
            });
        }
    }
}