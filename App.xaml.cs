using System.Configuration;
using System.Data;
using System.Windows;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using App_Hospital_Clinica.DI;               // nuestra clase de DI
using App_Hospital_Clinica.Infrastructure_SqlServer.EfDb;

namespace App_Hospital_Clinica {
    public partial class App : System.Windows.Application {
        public static IServiceProvider Services { get; private set; } = default!;

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            // 1) Construir configuración leyendo appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // 2) Crear ServiceCollection
            var services = new ServiceCollection();

            // 3) Registrar IConfiguration para inyectarla si hace falta
            services.AddSingleton<IConfiguration>(configuration);

            // 4) Registrar infraestructura SQL Server (nuestro método de extensión)
            services.AddSqlServerInfrastructure(configuration);

            // 5) Registrar ventana principal
            services.AddSingleton<MainWindow>();

            // 6) Build del contenedor
            Services = services.BuildServiceProvider();

            // 7) Resolver y mostrar MainWindow con DI
            var mainWindow = Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}