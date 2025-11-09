using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using App_Hospital_Clinica.Domain.Pacientes.Entities;

namespace App_Hospital_Clinica.Infrastructure_SqlServer.EfDb.Configurations {
    public sealed class PolizaConfig : IEntityTypeConfiguration<Poliza> {
        public void Configure(EntityTypeBuilder<Poliza> b) {
            b.ToTable("Poliza");

            // PK = IdPaciente (una póliza por paciente)
            b.HasKey(x => x.IdPaciente);

            b.Property(x => x.IdPaciente);
            b.Property(x => x.NumeroPoliza).IsRequired().HasMaxLength(40);
            b.HasIndex(x => x.NumeroPoliza).IsUnique();
            b.Property(x => x.Activa).IsRequired();
            b.Property(x => x.FechaFin).IsRequired();

            b.HasOne<Paciente>()
             .WithOne()
             .HasForeignKey<Poliza>(x => x.IdPaciente)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasOne<Aseguradora>()
             .WithMany()
             .HasForeignKey(x => x.IdAseguradora)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}