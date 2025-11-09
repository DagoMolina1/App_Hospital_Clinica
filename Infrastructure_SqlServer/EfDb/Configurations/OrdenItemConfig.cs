using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using App_Hospital_Clinica.Domain.Ordenes.Entities;

namespace App_Hospital_Clinica.Infrastructure_SqlServer.EfDb.Configurations {
    public sealed class OrdenItemConfig : IEntityTypeConfiguration<OrdenItem> {
        public void Configure(EntityTypeBuilder<OrdenItem> b) {
            b.ToTable("OrdenItem");

            // Clave compuesta
            b.HasKey(x => new { x.NumeroOrden, x.ItemN });

            b.Property(x => x.NumeroOrden).HasMaxLength(6).IsRequired();
            b.Property(x => x.ItemN).IsRequired();

            // Discriminador simple por tipo
            b.Property(x => x.ItemType)
             .HasColumnName("item_type")
             .HasMaxLength(10)
             .IsRequired();

            // FKs opcionales a catálogos
            b.Property(x => x.IdMedicamento);
            b.Property(x => x.IdProcedimiento);
            b.Property(x => x.IdAyuda);

            b.HasOne<App_Hospital_Clinica.Domain.Inventarios.Entities.Medicamento>()
             .WithMany()
             .HasForeignKey(x => x.IdMedicamento)
             .OnDelete(DeleteBehavior.Restrict);

            b.HasOne<App_Hospital_Clinica.Domain.Inventarios.Entities.Procedimiento>()
             .WithMany()
             .HasForeignKey(x => x.IdProcedimiento)
             .OnDelete(DeleteBehavior.Restrict);

            b.HasOne<App_Hospital_Clinica.Domain.Inventarios.Entities.AyudaDiagnostica>()
             .WithMany()
             .HasForeignKey(x => x.IdAyuda)
             .OnDelete(DeleteBehavior.Restrict);

            // Campos por tipo (NULL si no aplica)
            b.Property(x => x.Dosis).HasMaxLength(50);
            b.Property(x => x.Duracion).HasMaxLength(50);
            b.Property(x => x.Veces);
            b.Property(x => x.Frecuencia).HasMaxLength(50);
            b.Property(x => x.Cantidad);
            b.Property(x => x.Costo).HasColumnType("decimal(12,2)").IsRequired();

            b.Property(x => x.RequiereEspecialista);
            b.Property(x => x.IdTipoEspecialidad);

            b.HasOne<App_Hospital_Clinica.Domain.Inventarios.Entities.TipoEspecialidad>()
             .WithMany()
             .HasForeignKey(x => x.IdTipoEspecialidad)
             .OnDelete(DeleteBehavior.Restrict);

            // Índices útiles
            b.HasIndex(x => x.NumeroOrden);
            b.HasIndex(x => x.ItemType);
            b.HasIndex(x => x.IdMedicamento).HasFilter("[IdMedicamento] IS NOT NULL");
            b.HasIndex(x => x.IdProcedimiento).HasFilter("[IdProcedimiento] IS NOT NULL");
            b.HasIndex(x => x.IdAyuda).HasFilter("[IdAyuda] IS NOT NULL");
        }
    }
}