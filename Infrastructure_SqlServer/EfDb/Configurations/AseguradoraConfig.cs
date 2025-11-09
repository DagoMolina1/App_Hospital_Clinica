using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using App_Hospital_Clinica.Domain.Pacientes.Entities;

namespace App_Hospital_Clinica.Infrastructure_SqlServer.EfDb.Configurations {
    public sealed class AseguradoraConfig : IEntityTypeConfiguration<Aseguradora> {
        public void Configure(EntityTypeBuilder<Aseguradora> b) {
            b.ToTable("Aseguradora");
            b.HasKey(x => x.IdAseguradora);
            b.Property(x => x.Nombre).IsRequired().HasMaxLength(100);
            b.HasIndex(x => x.Nombre).IsUnique();
        }
    }
}