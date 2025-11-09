using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Application.Abstractions {
    /// <summary>
    /// ===============================================================
    ///  ICLOCK — Guía rápida
    /// ---------------------------------------------------------------
    /// Propósito:
    ///   Proveer una fuente de tiempo abstracta (inyectable) para todos los
    ///   casos de uso y servicios de la aplicación. Permite pruebas y
    ///   simulaciones sin depender del sistema local.
    ///
    /// Cuándo usarlo:
    ///   - Cuando un UseCase necesite comparar fechas o registrar “hoy”,
    ///     sin depender directamente de DateTime.UtcNow.
    ///   - Para pruebas unitarias (puedes inyectar un reloj fijo o simulado).
    ///
    /// Entradas/Salidas:
    ///   - Now => fecha/hora actual en UTC (o controlada por tests).
    ///
    /// Precondiciones:
    ///   - Se inyecta en constructores de casos de uso o servicios que lo requieran.
    ///   - La infraestructura implementa esta interfaz (por ejemplo, SystemClock).
    ///
    /// Postcondiciones:
    ///   - Todos los cálculos de tiempo del sistema son consistentes y trazables.
    ///
    /// Checklist de uso (UseCase):
    ///   [ ] Recibir IClock en el constructor.
    ///   [ ] var hoy = _clock.Now.Date;
    ///   [ ] Evitar DateTime.Now o DateTime.UtcNow directamente.
    /// ===============================================================
    /// </summary>
    public interface IClock {
        DateTime Now { get; }
    }
}