using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Application.Abstractions {
    /// <summary>
    /// ===============================================================
    ///  IREPORTGENERATOR — Guía rápida
    /// ---------------------------------------------------------------
    /// Propósito:
    ///   Proveer una abstracción para generar reportes dinámicos (PDF, Excel,
    ///   CSV u otros formatos) a partir de datos del dominio o consultas de negocio.
    ///   Permite crear informes de pacientes, órdenes, facturas o inventarios
    ///   sin acoplarse a una librería o formato específico.
    ///
    /// Cuándo usarlo:
    ///   - Cuando un Caso de Uso requiera exportar datos en formato legible
    ///     (para impresión, correo o almacenamiento).
    ///   - Por ejemplo: generar un reporte de facturación mensual o un resumen
    ///     de órdenes médicas por paciente.
    ///
    /// Entradas/Salidas:
    ///   - GenerateReportAsync(): recibe datos o un modelo y devuelve un flujo (Stream)
    ///     del archivo generado (PDF, Excel, etc.).
    ///
    /// Precondiciones:
    ///   - Los datos fueron previamente validados o formateados por el Caso de Uso.
    ///   - La infraestructura define las implementaciones concretas (por ejemplo,
    ///     PdfReportGenerator, ExcelReportGenerator).
    ///
    /// Postcondiciones:
    ///   - El archivo generado puede ser guardado, enviado por correo o mostrado en UI.
    ///
    /// Checklist de uso (UseCase):
    ///   [ ] Recibir IReportGenerator en el constructor.
    ///   [ ] var pdf = await _reportGen.GenerateReportAsync(datos, "FacturaPaciente.pdf");
    ///   [ ] await _fileStorage.SaveAsync(pdf, "facturas/");
    /// ===============================================================
    /// </summary>
    public interface IReportGenerator {
        /// <summary>
        /// Genera un reporte en un formato determinado (PDF, Excel, etc.).
        /// </summary>
        /// <param name="data">Datos o modelo a exportar.</param>
        /// <param name="fileName">Nombre del archivo a generar.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>Flujo de datos (Stream) del archivo generado.</returns>
        Task<Stream> GenerateReportAsync(object data, string fileName, CancellationToken ct = default);
    }
}