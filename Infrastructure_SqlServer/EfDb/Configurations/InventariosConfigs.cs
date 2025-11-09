using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using App_Hospital_Clinica.Domain.Inventarios.Entities;

namespace App_Hospital_Clinica.Infrastructure_SqlServer.EfDb.Configurations {
    public sealed class MedicamentoConfig : IEntityTypeConfiguration<Medicamento> {
        public void Configure(EntityTypeBuilder<Medicamento> b) {
            b.ToTable("Medicamento");
            b.HasKey(x => x.IdMedicamento);
            b.Property(x => x.Nombre).IsRequired().HasMaxLength(120);
            b.HasIndex(x => x.Nombre).IsUnique();
        }
    }

    public sealed class ProcedimientoConfig : IEntityTypeConfiguration<Procedimiento> {
        public void Configure(EntityTypeBuilder<Procedimiento> b) {
            b.ToTable("Procedimiento");
            b.HasKey(x => x.IdProcedimiento);
            b.Property(x => x.Nombre).IsRequired().HasMaxLength(120);
            b.HasIndex(x => x.Nombre).IsUnique();
        }
    }

    public sealed class AyudaDiagnosticaConfig : IEntityTypeConfiguration<AyudaDiagnostica> {
        public void Configure(EntityTypeBuilder<AyudaDiagnostica> b) {
            b.ToTable("AyudaDiagnostica");
            b.HasKey(x => x.IdAyuda);
            b.Property(x => x.Nombre).IsRequired().HasMaxLength(120);
            b.HasIndex(x => x.Nombre).IsUnique();
        }
    }

    public sealed class TipoEspecialidadConfig : IEntityTypeConfiguration<TipoEspecialidad> {
        public void Configure(EntityTypeBuilder<TipoEspecialidad> b) {
            b.ToTable("TipoEspecialidad");
            b.HasKey(x => x.IdTipoEspecialidad);
            b.Property(x => x.Nombre).IsRequired().HasMaxLength(120);
            b.HasIndex(x => x.Nombre).IsUnique();
        }
    }
}