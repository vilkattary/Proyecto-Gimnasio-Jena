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

        public int RegistrarPrimerPagoManual(PagoCrearDto pago)
        {
            if (pago == null)
                return 0;

            // Validar los datos necesarios.
            if (pago.idUsuarioNuevo <= 0 ||
                pago.idPlanNuevo <= 0 ||
                pago.idMetodoPago <= 0)
            {
                return 0;
            }

            // El primer pago manual siempre debe quedar como PAGADO.
            pago.idEstadoPago = ESTADO_PAGO_PAGADO;

            // Verificar que el método de pago esté activo.
            if (!_registrarPagoAD.ExisteMetodoPago(pago.idMetodoPago))
                return 0;

            // Verificar que el estado PAGADO esté disponible.
            if (!_registrarPagoAD.ExisteEstadoPago(pago.idEstadoPago))
                return 0;

            // Obtener los datos oficiales del plan seleccionado.
            var plan = _registrarPagoAD.ObtenerDatosPlanActivo(
                pago.idPlanNuevo);

            if (plan == null)
                return 0;

            if (plan.precioPlan <= 0 || plan.duracionDiasPlan <= 0)
                return 0;

            // No confiar en el monto enviado desde la vista.
            pago.monto = plan.precioPlan;

            // Validar la fecha del pago.
            if (pago.fechaPago == default(DateTime))
                pago.fechaPago = DateTime.Now;

            if (pago.fechaPago > DateTime.Now.AddMinutes(1))
                return 0;

            /*
             * La membresía comienza el día en que el administrador
             * registra el pago en el sistema.
             */
            DateTime fechaInicio = DateTime.Today;

            // La fecha de vencimiento es inclusiva.
            DateTime fechaFin = fechaInicio.AddDays(
                plan.duracionDiasPlan - 1);

            // NULL representa clases ilimitadas.
            int? clasesDisponibles = plan.cantidadClasesPlan;

            // Crear membresía y pago en una misma transacción.
            return _registrarPagoAD.RegistrarPrimerPagoManual(
                pago,
                fechaInicio,
                fechaFin,
                clasesDisponibles);
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