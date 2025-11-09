using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Domain.Common;
using App_Hospital_Clinica.Domain.Ordenes.Entities;

namespace App_Hospital_Clinica.Domain.Facturacion.Entities {
    /// <summary>
    /// Factura emitida a un paciente, basada en una orden médica.
    /// Contiene montos de copago, cargo a aseguradora y total.
    /// </summary>
    public class Factura {
        public int IdFactura { get; private set; }
        public string NumeroOrden { get; private set; } = default!;
        public string CedulaPaciente { get; private set; } = default!;
        public DateTime FechaFactura { get; private set; }
        public decimal CopagoCobrado { get; private set; }
        public decimal CargoAseguradora { get; private set; }
        public decimal TotalFactura { get; private set; }

        private readonly List<FacturaDetalle> _detalles = new();
        public IReadOnlyCollection<FacturaDetalle> Detalles => _detalles.AsReadOnly();

        protected Factura() { }

        private Factura(string numeroOrden, string cedulaPaciente, decimal copago, decimal cargoAseguradora, DateTime fecha)
        {
            NumeroOrden = numeroOrden;
            CedulaPaciente = cedulaPaciente;
            CopagoCobrado = copago;
            CargoAseguradora = cargoAseguradora;
            FechaFactura = fecha;
            TotalFactura = copago + cargoAseguradora;
        }

        /// <summary>
        /// Fábrica protegida por invariantes del dominio.
        /// </summary>
        public static Result<Factura> Create(string numeroOrden, string cedulaPaciente, decimal copago, decimal cargoAseguradora)
        {
            if (string.IsNullOrWhiteSpace(numeroOrden) || numeroOrden.Length != 6)
                return Result<Factura>.Fail("El número de orden es inválido.");

            if (string.IsNullOrWhiteSpace(cedulaPaciente))
                return Result<Factura>.Fail("La cédula del paciente es requerida.");

            if (copago < 0)
                return Result<Factura>.Fail("El copago no puede ser negativo.");

            if (cargoAseguradora < 0)
                return Result<Factura>.Fail("El cargo a la aseguradora no puede ser negativo.");

            var factura = new Factura(
                numeroOrden.Trim(),
                cedulaPaciente.Trim(),
                copago,
                cargoAseguradora,
                DateTime.UtcNow.Date
            );

            return Result<Factura>.Ok(factura);
        }

        /// <summary>
        /// Agrega un detalle a la factura.
        /// </summary>
        public Result AgregarDetalle(FacturaDetalle detalle)
        {
            if (detalle == null)
                return Result.Fail("El detalle no puede ser nulo.");

            _detalles.Add(detalle);
            RecalcularTotal();
            return Result.Ok();
        }

        /// <summary>
        /// Recalcula el total con base en los detalles actuales.
        /// </summary>
        private void RecalcularTotal()
        {
            var subtotal = _detalles.Sum(d => d.Costo);
            TotalFactura = CopagoCobrado + CargoAseguradora + subtotal;
        }
    }
}
