using App_Hospital_Clinica.Applicationn.Abstractions;
using App_Hospital_Clinica.Applicationn.UseCases.Pacientes.RegistrarPaciente;

// Puertos de dominio
using App_Hospital_Clinica.Domain.Pacientes.Ports;
using App_Hospital_Clinica.Domain.Inventarios.Ports;
using App_Hospital_Clinica.Domain.Ordenes.Ports;
using App_Hospital_Clinica.Domain.Facturacion.Ports;
using App_Hospital_Clinica.Domain.RRHH.Ports;

// Infraestructura SQL Server
using App_Hospital_Clinica.Infrastructure_SqlServer.EfDb;
using App_Hospital_Clinica.Infrastructure_SqlServer.Adapters.Pacientes;
using App_Hospital_Clinica.Infrastructure_SqlServer.Adapters.Inventarios;
using App_Hospital_Clinica.Infrastructure_SqlServer.Adapters.Ordenes;
using App_Hospital_Clinica.Infrastructure_SqlServer.Adapters.Facturacion;
using App_Hospital_Clinica.Infrastructure_SqlServer.Adapters.RRHH;
using App_Hospital_Clinica.Infrastructure_SqlServer.Common;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App_Hospital_Clinica.DI {
    /// <summary>
    /// Registro central de servicios de la clínica:
    /// - DbContext (SQL Server)
    /// - UnitOfWork
    /// - Repositorios
    /// - Handlers de casos de uso
    /// - Reloj del sistema
    /// </summary>
    public static class DependencyInjection {
        public static IServiceCollection AddClinicaServices(
            this IServiceCollection services,
            IConfiguration configuration) {
            // 1) DbContext (EF Core + SQL Server)
            var connectionString = configuration.GetConnectionString("SqlServerClinica");

            services.AddDbContext<ClinicaDbContext>(options => {
                options.UseSqlServer(connectionString);
            });

            // 2) Logging simple (sin AddDebug/AddConsole para evitar paquetes extra)
            services.AddLogging();

            // 3) Servicios transversales
            services.AddSingleton<IClock, App_Hospital_Clinica.Infrastructure_SqlServer.Common.SystemClock>();

            // 4) Unit Of Work
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();

            // 5) Repositorios
            // --- Pacientes ---
            services.AddScoped<IPacienteRepository, PacienteRepository>();
            services.AddScoped<IContactoEmergenciaRepository, ContactoEmergenciaRepository>();
            services.AddScoped<IPolizaRepository, PolizaRepository>();

            // --- Inventarios ---
            services.AddScoped<IMedicamentoRepository, MedicamentoRepository>();
            services.AddScoped<IProcedimientoRepository, ProcedimientoRepository>();
            services.AddScoped<IAyudaDiagnosticaRepository, AyudaDiagnosticaRepository>();
            services.AddScoped<ITipoEspecialidadRepository, TipoEspecialidadRepository>();

            // --- Órdenes ---
            services.AddScoped<IOrdenRepository, OrdenRepository>();

            // --- Facturación (solo factura por ahora) ---
            services.AddScoped<IFacturaRepository, FacturaRepository>();
            // OJO: por ahora NO registramos IFacturaDetalleRepository / FacturaDetalleRepository
            // hasta que comprobemos que existen en Domain/Infrastructure y los necesitemos.

            // --- RRHH ---
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IRolRepository, RolRepository>();

            // 6) Handlers de casos de uso
            services.AddScoped<RegistrarPacienteHandler>();

            return services;
        }
    }
}