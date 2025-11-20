using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Applicationn.Abstractions;

namespace App_Hospital_Clinica.Infrastructure_SqlServer.Common {
    /// <summary>
    /// Reloj del sistema utilizado por la capa de Applicationn.
    /// Permite testear más fácil si algún día quieres un reloj falso.
    /// Reloj del sistema (UTC) usado por la capa Applicationn.
    /// </summary>
    public sealed class SystemClock : IClock {
        public DateTime Now => DateTime.UtcNow;
    }
}