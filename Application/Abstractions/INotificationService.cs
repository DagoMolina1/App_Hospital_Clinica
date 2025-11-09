using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Application.Abstractions {
    /// <summary>
    /// ===============================================================
    ///  INOTIFICATIONSERVICE — Guía rápida
    /// ---------------------------------------------------------------
    /// Propósito:
    ///   Abstraer el envío de notificaciones del sistema hacia usuarios o
    ///   servicios externos. Puede implementarse con correos, SMS, WhatsApp,
    ///   notificaciones push o sistemas internos de mensajería.
    ///
    /// Cuándo usarlo:
    ///   - Cuando un Caso de Uso deba informar a un paciente, médico o
    ///     administrativo sobre un evento relevante:
    ///       • Registro exitoso de paciente.
    ///       • Creación de orden médica.
    ///       • Emisión de factura.
    ///       • Vencimiento de póliza.
    ///
    /// Entradas/Salidas:
    ///   - SendNotificationAsync(): envía una notificación con título, mensaje y destino.
    ///   - Los canales concretos (correo, SMS, push) se implementan en Infrastructure.
    ///
    /// Precondiciones:
    ///   - El mensaje y el destinatario son válidos.
    ///   - Se inyecta en los casos de uso que requieren comunicación.
    ///
    /// Postcondiciones:
    ///   - La notificación se entrega (o se registra el intento fallido).
    ///
    /// Checklist de uso (UseCase):
    ///   [ ] Recibir INotificationService en el constructor.
    ///   [ ] await _notifier.SendNotificationAsync("Nuevo paciente", "Se registró Juan Pérez", "admin@clinica.com");
    ///   [ ] Manejar los errores con _logger.LogError() si algo falla.
    /// ===============================================================
    /// </summary>
    public interface INotificationService {
        Task SendNotificationAsync(string title, string message, string recipient, CancellationToken ct = default);
    }
}