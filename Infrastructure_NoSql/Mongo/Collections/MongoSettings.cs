using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Infrastructure_NoSql.Mongo.Collections {
    /// <summary>
    /// ===============================================================
    /// CONFIGURACIÓN DE MONGO — Conexión y base de datos
    /// ---------------------------------------------------------------
    /// Define las propiedades necesarias para conectar con MongoDB.
    /// Estas se leen normalmente desde `appsettings.json`.
    /// ===============================================================
    /// </summary>
    public sealed class MongoSettings {
        public string ConnectionString { get; set; } = "mongodb://localhost:27017";
        public string DatabaseName { get; set; } = "ClinicaIPS";
    }
}