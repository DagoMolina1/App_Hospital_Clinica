using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using App_Hospital_Clinica.Domain.Ordenes.Entities;

namespace App_Hospital_Clinica.Infrastructure_SqlServer.EfDb.Configurations {
    public sealed class OrdenConfig : IEntityTypeConfiguration<Orden> {
        public void Configure(EntityTypeBuilder<Orden> b) {
            b.ToTable("Orden");
            // PK = NumeroOrden (char(6))
            b.HasKey(x => x.NumeroOrden);

            b.Property(x => x.NumeroOrden)
             .HasMaxLength(6)
             .IsRequired();

            // CedulaPaciente como VO
            b.OwnsOne(x => x.CedulaPaciente, cb =>
            {
                cb.Property(p => p.Value)
                  .HasColumnName("CedulaPaciente")
                  .HasMaxLength(15)
                  .IsRequired();
            });

            b.Property(x => x.CedulaMedico)
             .HasMaxLength(15)
             .IsRequired();

            b.Property(x => x.FechaCreacion)
             .HasColumnType("date")
             .IsRequired();

            // Relación con Items
            b.HasMany<OrdenItem>("_items")   // si tienes backing field; si no, usa .HasMany(x => x.Items)
             .WithOne()
             .HasForeignKey(x => x.NumeroOrden)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}