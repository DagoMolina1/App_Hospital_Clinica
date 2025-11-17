using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Applicationn.Abstractions {
    /// <summary>
    /// ===============================================================
    ///  ILOGGER — Guía rápida
    /// ---------------------------------------------------------------
    /// Propósito:
    ///   Proveer una abstracción central de registro de logs (información,
    ///   advertencias y errores) accesible desde los Casos de Uso, sin depender
    ///   directamente de librerías externas como Serilog, NLog o Console.
    ///
    /// Cuándo usarlo:
    ///   - Cuando un UseCase necesite registrar eventos de negocio o excepciones.
    ///   - Para mantener trazabilidad del flujo de aplicación sin acoplarla
    ///     a una implementación específica.
    ///
    /// Entradas/Salidas:
    ///   - LogInformation, LogWarning, LogError: graban mensajes contextualizados.
    ///   - La infraestructura provee la implementación concreta (ej: ConsoleLogger,
    ///     SerilogAdapter, etc.).
    ///
    /// Precondiciones:
    ///   - Se inyecta vía constructor en los casos de uso.
    ///   - No lanza excepciones (debe ser tolerante a fallos).
    ///
    /// Postcondiciones:
    ///   - Los eventos quedan almacenados en el backend de logs (archivo, consola,
    ///     base de datos, etc.).
    ///
    /// Checklist de uso (UseCase):
    ///   [ ] Recibir ILogger en el constructor.
    ///   [ ] _logger.LogInformation("Registrando nuevo paciente {Cedula}", cedula);
    ///   [ ] _logger.LogError(ex, "Error creando factura");
    /// ===============================================================
    /// </summary>
    public interface ILogger<T> {
        void LogInformation(string message, params object[] args);
        void LogWarning(string message, params object[] args);
        void LogError(Exception exception, string message, params object[] args);
    }
}