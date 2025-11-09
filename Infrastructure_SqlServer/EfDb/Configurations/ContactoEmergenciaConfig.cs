using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using App_Hospital_Clinica.Domain.Pacientes.Entities;

namespace App_Hospital_Clinica.Infrastructure_SqlServer.EfDb.Configurations {
    public sealed class ContactoEmergenciaConfig : IEntityTypeConfiguration<ContactoEmergencia> {
        public void Configure(EntityTypeBuilder<ContactoEmergencia> b) {
            b.ToTable("ContactoEmergencia");

            // PK = IdPaciente (modelo 1:1 fuerte)
            b.HasKey(x => x.IdPaciente);

            b.Property(x => x.IdPaciente);

            b.Property(x => x.Nombre).IsRequired().HasMaxLength(60);
            b.Property(x => x.Apellidos).IsRequired().HasMaxLength(60);
            b.Property(x => x.Relacion).IsRequired().HasMaxLength(50);

            // Teléfono como VO simple (si en dominio es string, cambia a Property)
            b.OwnsOne(x => x.Telefono, tb => {
                tb.Property(p => p.Value)
                  .HasColumnName("Telefono")
                  .HasMaxLength(10)
                  .IsRequired();
            });

            b.HasOne<Paciente>()
             .WithOne()
             .HasForeignKey<ContactoEmergencia>(x => x.IdPaciente)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}