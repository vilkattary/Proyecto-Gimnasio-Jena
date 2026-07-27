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

            if (pago.monto <= 0)
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

            if (pago.fechaPago == default(DateTime))
            {
                pago.fechaPago = DateTime.Now;
            }

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
                 * Si la membresía todavía tiene días disponibles,
                 * la nueva vigencia comenzará después de su fecha final.
                 *
                 * Si ya venció, comenzará hoy.
                 */
                if (membresia.fechaFin >= hoy)
                {
                    nuevaFechaInicio =
                        membresia.fechaFin.AddDays(1);
                }
                else
                {
                    nuevaFechaInicio = hoy;
                }

                /*
                 * Como fechaFin es un campo DATE y se considera
                 * inclusiva, una membresía de 31 días iniciada el
                 * día 1 finaliza el día 31.
                 */
                nuevaFechaFin =
                    nuevaFechaInicio.Value.AddDays(
                        membresia.duracionDiasPlan - 1
                    );

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
    }
}