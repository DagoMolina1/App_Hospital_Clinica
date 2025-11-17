using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Applicationn.Abstractions {
    /// <summary>
    /// ===============================================================
    ///  IEMAILSERVICE — Guía rápida
    /// ---------------------------------------------------------------
    /// Propósito:
    ///   Proveer una abstracción de envío de correos electrónicos para
    ///   notificaciones de la aplicación (confirmaciones, avisos, reportes, etc.).
    ///   Esto evita acoplar la lógica de negocio con servicios externos (SMTP, SendGrid, etc.).
    ///
    /// Cuándo usarlo:
    ///   - Cuando un Caso de Uso deba notificar a un usuario (por ejemplo,
    ///     al registrar un paciente o generar una factura).
    ///
    /// Entradas/Salidas:
    ///   - SendAsync(): envía un correo con destinatario, asunto y cuerpo.
    ///   - La implementación concreta en Infrastructure maneja el protocolo real.
    ///
    /// Precondiciones:
    ///   - Se inyecta en los casos de uso que lo requieran.
    ///   - Los datos del correo (email, asunto, cuerpo) fueron previamente validados.
    ///
    /// Postcondiciones:
    ///   - El mensaje queda entregado al sistema de correo o registrado como intento fallido.
    ///
    /// Checklist de uso (UseCase):
    ///   [ ] Recibir IEmailService en el constructor.
    ///   [ ] await _emailService.SendAsync(correoDestino, asunto, cuerpo);
    ///   [ ] Capturar posibles fallos o excepciones con _logger.LogError().
    /// ===============================================================
    /// </summary>
    public interface IEmailService {
        Task SendAsync(string to, string subject, string body, CancellationToken ct = default);
    }
}