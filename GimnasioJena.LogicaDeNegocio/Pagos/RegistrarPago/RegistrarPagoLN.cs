using GimnasioJena.Abstracciones.AccesoADatos.Pagos.RegistrarPago;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Pagos.RegistrarPago;
using GimnasioJena.Abstracciones.Modelos.Pagos;
using GimnasioJena.AccesoADatos.Pagos.RegistrarPago;
using System;

namespace GimnasioJena.LogicaDeNegocio.Pagos.RegistrarPago
{
    public class RegistrarPagoLN : IRegistrarPagoLN
    {
        private const int ESTADO_PAGO_PAGADO = 2;
        private const int ESTADO_MEMBRESIA_ACTIVA = 1;

        private readonly IRegistrarPagoAD _registrarPagoAD;

        public RegistrarPagoLN()
        {
            _registrarPagoAD = new RegistrarPagoAD();
        }

        public int RegistrarPago(PagoCrearDto pago)
        {
            if (pago == null)
                return 0;

            if (pago.idMembresiaCliente <= 0)
                return 0;

            if (pago.idMetodoPago <= 0)
                return 0;

            if (pago.idEstadoPago <= 0)
                return 0;

            if (!_registrarPagoAD.ExisteMetodoPago(
                pago.idMetodoPago))
            {
                return 0;
            }

            if (!_registrarPagoAD.ExisteEstadoPago(
                pago.idEstadoPago))
            {
                return 0;
            }

            var membresia =
                _registrarPagoAD.ObtenerDatosMembresia(
                    pago.idMembresiaCliente);

            if (membresia == null)
                return 0;

            /*
             * El monto recibido desde la vista no se considera confiable.
             * Siempre se utiliza el precio oficial almacenado en el plan.
             */
            pago.monto = membresia.precioPlan;

            if (pago.monto <= 0)
                return 0;

            if (pago.fechaPago == default(DateTime))
            {
                pago.fechaPago = DateTime.Now;
            }

            /*
             * No se permite registrar una fecha de pago futura.
             * Se deja un minuto de tolerancia por diferencias mínimas
             * entre el navegador y el servidor.
             */
            if (pago.fechaPago > DateTime.Now.AddMinutes(1))
                return 0;

            DateTime? nuevaFechaInicio = null;
            DateTime? nuevaFechaFin = null;
            int? nuevasClasesDisponibles = null;
            int? nuevoEstadoMembresia = null;

            /*
             * Únicamente un pago confirmado como Pagado
             * puede activar o renovar una membresía.
             */
            if (pago.idEstadoPago == ESTADO_PAGO_PAGADO)
            {
                DateTime hoy = DateTime.Today;

                /*
                 * Regla oficial:
                 * no se permite renovar antes del vencimiento.
                 *
                 * fechaFin == hoy:
                 * sí se permite renovar.
                 *
                 * fechaFin < hoy:
                 * sí se permite renovar.
                 */
                if (membresia.fechaFin > hoy)
                {
                    return 0;
                }

                /*
                 * La nueva vigencia siempre comienza el día
                 * en que se confirma el pago.
                 */
                nuevaFechaInicio = hoy;

                /*
                 * La fecha final es inclusiva.
                 * Un plan de 31 días iniciado hoy termina
                 * 30 días después.
                 */
                nuevaFechaFin =
                    nuevaFechaInicio.Value.AddDays(
                        membresia.duracionDiasPlan - 1
                    );

                /*
                 * ROOT obtiene 12 clases.
                 * TOP conserva NULL, lo cual representa
                 * clases ilimitadas.
                 */
                nuevasClasesDisponibles =
                    membresia.cantidadClasesPlan;

                nuevoEstadoMembresia =
                    ESTADO_MEMBRESIA_ACTIVA;
            }

            return _registrarPagoAD
                .RegistrarPagoYActualizarMembresia(
                    pago,
                    nuevaFechaInicio,
                    nuevaFechaFin,
                    nuevasClasesDisponibles,
                    nuevoEstadoMembresia
                );
        }

        public bool ExisteReferenciaPago(string referenciaPago)
        {
            if (string.IsNullOrWhiteSpace(referenciaPago))
                return false;

            return _registrarPagoAD
                .ExisteReferenciaPago(referenciaPago);
        }
    }
}