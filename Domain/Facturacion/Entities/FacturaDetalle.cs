using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Hospital_Clinica.Domain.Common;

namespace App_Hospital_Clinica.Domain.Facturacion.Entities {
    /// <summary>
    /// Detalle de una factura (ítems facturados).
    /// Cada registro representa un concepto cobrado al paciente o aseguradora.
    /// </summary>
    public class FacturaDetalle {
        public int IdFacturaDetalle { get; private set; }
        public int IdFactura { get; private set; }
        public string NumeroOrden { get; private set; } = default!;
        public int ItemN { get; private set; }
        public string Descripcion { get; private set; } = default!;
        public decimal Costo { get; private set; }

        protected FacturaDetalle() { }

        private FacturaDetalle(int idFactura, string numeroOrden, int itemN, string descripcion, decimal costo)
        {
            IdFactura = idFactura;
            NumeroOrden = numeroOrden;
            ItemN = itemN;
            Descripcion = descripcion;
            Costo = costo;
        }

        /// <summary>
        /// Fábrica protegida por invariantes de dominio.
        /// </summary>
        public static Result<FacturaDetalle> Create(int idFactura, string numeroOrden, int itemN, string descripcion, decimal costo)
        {
            if (idFactura <= 0)
                return Result<FacturaDetalle>.Fail("El ID de factura es inválido.");

            if (string.IsNullOrWhiteSpace(numeroOrden) || numeroOrden.Length != 6)
                return Result<FacturaDetalle>.Fail("El número de orden es inválido (6 caracteres requeridos).");

            if (itemN <= 0)
                return Result<FacturaDetalle>.Fail("El número del ítem debe ser mayor a 0.");

            if (string.IsNullOrWhiteSpace(descripcion))
                return Result<FacturaDetalle>.Fail("La descripción es requerida.");

            var desc = descripcion.Trim();
            if (desc.Length > 200)
                return Result<FacturaDetalle>.Fail("La descripción no debe superar 200 caracteres.");

            if (costo <= 0)
                return Result<FacturaDetalle>.Fail("El costo debe ser mayor a 0.");

            var detalle = new FacturaDetalle(idFactura, numeroOrden.Trim(), itemN, desc, costo);
            return Result<FacturaDetalle>.Ok(detalle);
        }

        /// <summary>
        /// Permite actualizar el costo de un ítem facturado.
        /// </summary>
        public Result ActualizarCosto(decimal nuevoCosto)
        {
            if (nuevoCosto <= 0)
                return Result.Fail("El costo debe ser mayor a 0.");

            Costo = nuevoCosto;
            return Result.Ok();
        }
    }
}
