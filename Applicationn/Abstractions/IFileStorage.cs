using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Applicationn.Abstractions {
    /// <summary>
    /// ===============================================================
    ///  IFILESTORAGE — Guía rápida
    /// ---------------------------------------------------------------
    /// Propósito:
    ///   Definir una interfaz genérica para almacenar y recuperar archivos
    ///   (PDFs, imágenes, informes, soportes, etc.) de forma independiente de
    ///   la infraestructura. Permite usar almacenamiento local, en la nube
    ///   (Azure Blob, AWS S3, Google Drive) o una carpeta compartida.
    ///
    /// Cuándo usarlo:
    ///   - Cuando un Caso de Uso requiera guardar o recuperar documentos
    ///     asociados a pacientes, órdenes, facturas o reportes.
    ///   - Por ejemplo: generar y guardar una factura PDF, o adjuntar un
    ///     examen clínico escaneado.
    ///
    /// Entradas/Salidas:
    ///   - SaveAsync(): guarda un archivo y devuelve su ruta o identificador.
    ///   - GetAsync(): recupera un archivo previamente guardado.
    ///   - DeleteAsync(): elimina un archivo si se requiere.
    ///
    /// Precondiciones:
    ///   - El archivo fue generado o validado previamente (no nulo, tamaño correcto).
    ///   - Se inyecta desde Infrastructure con la implementación real (disco, S3, etc.).
    ///
    /// Postcondiciones:
    ///   - El archivo queda almacenado de forma persistente y accesible.
    ///
    /// Checklist de uso (UseCase):
    ///   [ ] Recibir IFileStorage en el constructor.
    ///   [ ] var ruta = await _fileStorage.SaveAsync(stream, "facturas/123.pdf");
    ///   [ ] await _fileStorage.GetAsync("facturas/123.pdf");
    ///   [ ] await _fileStorage.DeleteAsync("facturas/123.pdf");
    /// ===============================================================
    /// </summary>
    public interface IFileStorage {
        Task<string> SaveAsync(Stream fileStream, string path, CancellationToken ct = default);
        Task<Stream?> GetAsync(string path, CancellationToken ct = default);
        Task DeleteAsync(string path, CancellationToken ct = default);
    }
}