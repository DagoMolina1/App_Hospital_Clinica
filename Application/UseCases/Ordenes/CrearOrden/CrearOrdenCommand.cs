using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Hospital_Clinica.Application.UseCases.Ordenes.CrearOrden {
    /// <summary>
    /// ===============================================================
    ///  CREAR ORDEN — Command (DTO de entrada) — Guía rápida
    /// ---------------------------------------------------------------
    /// Propósito:
    ///   Transportar los datos necesarios para crear la cabecera de una Orden.
    ///
    /// Campos:
    ///   - NumeroOrden  : string de 6 dígitos (único).
    ///   - CedulaPaciente: cédula del paciente (lookup).
    ///   - CedulaMedico : cédula del médico emisor (numérica).
    ///
    /// Notas:
    ///   - Los ítems se agregan en casos de uso separados.
    /// ===============================================================
    /// </summary>
    public sealed class CrearOrdenCommand {
        public string NumeroOrden { get; init; } = default!;
        public string CedulaPaciente { get; init; } = default!;
        public string CedulaMedico { get; init; } = default!;
    }
}