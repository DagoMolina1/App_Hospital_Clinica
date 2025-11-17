using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Domain.Facturacion.Entities;
using App_Hospital_Clinica.Domain.Inventarios.Entities;
using App_Hospital_Clinica.Domain.Ordenes.Entities;
using App_Hospital_Clinica.Domain.Pacientes.Entities;
using App_Hospital_Clinica.Domain.RRHH.Entities;
using Microsoft.EntityFrameworkCore;

namespace App_Hospital_Clinica.Infrastructure_SqlServer.EfDb {
    /// <summary>
    /// ===============================================================
    ///  CLINICADBContext — Guía rápida
    /// ---------------------------------------------------------------
    /// - Usa esquema `clinica`.
    /// - Expone DbSet<> alineados con tus tablas.
    /// - Aplica Configurations por assembly.
    /// - Punto central para UnitOfWork/Transacciones.
    /// ===============================================================
    /// </summary>
    public class ClinicaDbContext : DbContext {
        public const string Schema = "clinica";

        public ClinicaDbContext(DbContextOptions<ClinicaDbContext> options) : base(options) { }

        // Pacientes
        public DbSet<Paciente> Pacientes => Set<Paciente>();
        public DbSet<ContactoEmergencia> Contactos => Set<ContactoEmergencia>();
        public DbSet<Poliza> Polizas => Set<Poliza>();
        public DbSet<Aseguradora> Aseguradoras => Set<Aseguradora>();

        // Inventarios
        public DbSet<Medicamento> Medicamentos => Set<Medicamento>();
        public DbSet<Procedimiento> Procedimientos => Set<Procedimiento>();
        public DbSet<AyudaDiagnostica> AyudasDiagnosticas => Set<AyudaDiagnostica>();
        public DbSet<TipoEspecialidad> TiposEspecialidad => Set<TipoEspecialidad>();

        // Órdenes
        public DbSet<Orden> Ordenes => Set<Orden>();
        public DbSet<OrdenItem> OrdenItems => Set<OrdenItem>();

        // Facturación
        public DbSet<Factura> Facturas => Set<Factura>();
        public DbSet<FacturaDetalle> FacturaDetalles => Set<FacturaDetalle>();

        // RRHH
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Rol> Roles => Set<Rol>();
        public DbSet<UsuarioRol> UsuarioRoles => Set<UsuarioRol>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);
            // Aplica todas las IEntityTypeConfiguration<> del ensamble de infraestructura
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClinicaDbContext).Assembly);
        }
    }
}