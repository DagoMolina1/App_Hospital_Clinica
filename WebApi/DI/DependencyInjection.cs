using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using App_Hospital_Clinica.Domain.Historia_Clinica.Ports;
using App_Hospital_Clinica.Infrastructure_NoSql.Adapters;
using App_Hospital_Clinica.Infrastructure_NoSql.Mongo.Collections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace App_Hospital_Clinica.WebApi.DI {
    /// <summary>
    /// Registro de servicios NoSQL (MongoDB).
    /// </summary>
    public static class DependencyInjection {
        public static IServiceCollection AddNoSql(this IServiceCollection services, IConfiguration configuration) {
            // 1) Bind settings
            services.Configure<MongoSettings>(configuration.GetSection("MongoSettings"));

            // 2) Cliente Mongo (Singleton)
            services.AddSingleton<IMongoClient>(sp => {
                var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
                return new MongoClient(settings.ConnectionString);
            });

            // 3) Database (Scoped)
            services.AddScoped<IMongoDatabase>(sp => {
                var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase(settings.DatabaseName);
            });

            // 4) Repo documental
            services.AddScoped<IHistoriaClinicaRepository, HistoriaClinicaRepository>();

            return services;
        }
    }
}