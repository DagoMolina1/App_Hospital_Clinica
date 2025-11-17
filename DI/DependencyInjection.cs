using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Infrastructure_SqlServer.EfDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App_Hospital_Clinica.DI {
    /*public static class DependencyInjection {
        public static IServiceCollection AddSqlServerInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration) {
            // Lee la cadena de conexión del appsettings.json
            var connectionString = configuration.GetConnectionString("SqlServerClinica");

            // Registra el DbContext de EF Core
            services.AddDbContext<ClinicaDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            return services;
        }
        // Aquí mismo puedes tener el AddNoSql(...) que vimos antes, si usas Mongo.*/
    /// <summary>
    /// Punto central para registrar infraestructura en el contenedor DI.
    /// - SQL Server (EF Core)
    /// - MongoDB (NoSql) para Historia Clínica
    /// </summary>
    public static class DependencyInjection {
        /// <summary>
        /// Registra la infraestructura de SQL Server (DbContext + EF Core).
        /// Lee la cadena de conexión "SqlServerClinica" desde appsettings.json.
        /// </summary>
        public static IServiceCollection AddSqlServerInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration) {
            // Lee la cadena de conexión del appsettings.json
            var connectionString = configuration.GetConnectionString("SqlServerClinica");

            // Registra el DbContext de EF Core apuntando a SQL Server
            services.AddDbContext<ClinicaDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            return services;
        }

        /// <summary>
        /// Registra la infraestructura NoSql (MongoDB) para Historia Clínica.
        /// Usa la sección "MongoSettings" de appsettings.json.
        /// </summary>
        /*public static IServiceCollection AddNoSql(
            this IServiceCollection services,
            IConfiguration configuration) {
            // 1) Bind de configuración (MongoSettings en appsettings.json)
            services.Configure<MongoSettings>(configuration.GetSection("MongoSettings"));

            // 2) Cliente MongoDB (Singleton)
            services.AddSingleton<IMongoClient>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
                return new MongoClient(settings.ConnectionString);
            });

            // 3) Base de datos Mongo (Scoped)
            services.AddScoped<IMongoDatabase>(sp => {
                var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase(settings.DatabaseName);
            });

            // 4) Repositorio documental de Historia Clínica
            services.AddScoped<IHistoriaClinicaRepository, HistoriaClinicaRepository>();

            return services;
        }*/
    }
}