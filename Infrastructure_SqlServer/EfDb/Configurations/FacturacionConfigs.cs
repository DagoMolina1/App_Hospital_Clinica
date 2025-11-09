using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using App_Hospital_Clinica.Domain.Facturacion.Entities;

namespace App_Hospital_Clinica.Infrastructure_SqlServer.EfDb.Configurations {
    /*public sealed class FacturaConfig : IEntityTypeConfiguration<Factura> {
        public void Configure(EntityTypeBuilder<Factura> b) {
            b.ToTable("Factura");
            b.HasKey(x => x.IdFactura);

            b.Property(x => x.NumeroOrden).HasMaxLength(6).IsRequired();

            // Si CedulaPaciente es VO en dominio, mapea con OwnsOne
            b.OwnsOne(x => x.CedulaPaciente, cb => {
                cb.Property(p => p.Value)
                  .HasColumnName("CedulaPaciente")
                  .HasMaxLength(15)
                  .IsRequired();
            });

            b.Property(x => x.FechaFactura).HasColumnType("date").IsRequired();
            b.Property(x => x.CopagoCobrado).HasColumnType("decimal(12,2)").IsRequired();
            b.Property(x => x.CargoAseguradora).HasColumnType("decimal(12,2)").IsRequired();
            b.Property(x => x.TotalFactura).HasColumnType("decimal(12,2)").IsRequired();

            b.HasMany<FacturaDetalle>("_detalles") // o .HasMany(x => x.Detalles)
             .WithOne()
             .HasForeignKey(x => x.IdFactura)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public sealed class FacturaDetalleConfig : IEntityTypeConfiguration<FacturaDetalle> {
        public void Configure(EntityTypeBuilder<FacturaDetalle> b) {
            b.ToTable("FacturaDetalle");
            b.HasKey(x => x.IdFacturaDetalle);

            b.Property(x => x.IdFactura).IsRequired();
            b.Property(x => x.NumeroOrden).HasMaxLength(6).IsRequired();
            b.Property(x => x.ItemN).IsRequired();
            b.Property(x => x.Descripcion).IsRequired().HasMaxLength(200);
            b.Property(x => x.Costo).HasColumnType("decimal(12,2)").IsRequired();
        }
    }*/

    public sealed class FacturaConfig : IEntityTypeConfiguration<Factura> {
        public void Configure(EntityTypeBuilder<Factura> b) {
            b.ToTable("Factura");
            b.HasKey(x => x.IdFactura);

            b.Property(x => x.NumeroOrden)
             .HasMaxLength(6)
             .IsRequired();

            // CedulaPaciente es string en tu Dominio → mapear como Property
            b.Property(x => x.CedulaPaciente)
             .HasColumnName("CedulaPaciente")
             .HasMaxLength(15)
             .IsRequired();

            b.Property(x => x.FechaFactura).HasColumnType("date").IsRequired();
            b.Property(x => x.CopagoCobrado).HasColumnType("decimal(12,2)").IsRequired();
            b.Property(x => x.CargoAseguradora).HasColumnType("decimal(12,2)").IsRequired();
            b.Property(x => x.TotalFactura).HasColumnType("decimal(12,2)").IsRequired();

            b.HasMany<FacturaDetalle>("_detalles") // o .HasMany(x => x.Detalles)
             .WithOne()
             .HasForeignKey(x => x.IdFactura)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}