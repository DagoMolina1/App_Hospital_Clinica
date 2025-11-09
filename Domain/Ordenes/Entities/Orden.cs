using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Domain.Common;
using App_Hospital_Clinica.Domain.Pacientes.ValueObjects;
using App_Hospital_Clinica.Domain.Ordenes.Entities;

namespace App_Hospital_Clinica.Domain.Ordenes.Entities {
    /// <summary>
    /// Orden médica emitida para un paciente.
    /// Contiene la cabecera general; los ítems se manejan aparte.
    /// </summary>
    public class Orden
    {
        public string NumeroOrden { get; private set; } = default!;
        public Cedula CedulaPaciente { get; private set; } = default!;
        public string CedulaMedico { get; private set; } = default!;
        public DateTime FechaCreacion { get; private set; }

        private readonly List<OrdenItem> _items = new();
        public IReadOnlyCollection<OrdenItem> Items => _items.AsReadOnly();

        protected Orden() { }

        private Orden(string numeroOrden, Cedula cedulaPaciente, string cedulaMedico)
        {
            NumeroOrden = numeroOrden;
            CedulaPaciente = cedulaPaciente;
            CedulaMedico = cedulaMedico;
            FechaCreacion = DateTime.UtcNow.Date;
        }

        /// <summary>
        /// Fábrica protegida por invariantes del dominio.
        /// </summary>
        public static Result<Orden> Create(string numeroOrden, Cedula cedulaPaciente, string cedulaMedico)
        {
            if (string.IsNullOrWhiteSpace(numeroOrden) || numeroOrden.Length != 6)
                return Result<Orden>.Fail("El número de orden debe tener exactamente 6 dígitos.");

            if (string.IsNullOrWhiteSpace(cedulaMedico) || !cedulaMedico.All(char.IsDigit))
                return Result<Orden>.Fail("La cédula del médico es requerida y debe ser numérica.");

            var orden = new Orden(numeroOrden.Trim(), cedulaPaciente, cedulaMedico.Trim());
            return Result<Orden>.Ok(orden);
        }

        /// <summary>
        /// Agrega un ítem a la orden validando reglas básicas.
        /// </summary>
        public Result AgregarItem(OrdenItem item)
        {
            if (item == null)
                return Result.Fail("El ítem no puede ser nulo.");

            if (_items.Any(x => x.ItemN == item.ItemN))
                return Result.Fail($"Ya existe un ítem con número {item.ItemN}.");

            _items.Add(item);
            return Result.Ok();
        }

        /// <summary>
        /// Calcula el costo total de todos los ítems.
        /// </summary>
        public decimal CalcularTotal() => _items.Sum(x => x.Costo);
    }
}
