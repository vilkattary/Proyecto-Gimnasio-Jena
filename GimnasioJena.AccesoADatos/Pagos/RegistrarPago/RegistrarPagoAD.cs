using GimnasioJena.Abstracciones.AccesoADatos.Pagos.RegistrarPago;
using GimnasioJena.Abstracciones.Modelos.Pagos;
using GimnasioJena.AccesoADatos.Entidades.Pagos;
using System;
using System.Data.Entity;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Pagos.RegistrarPago
{
    public class RegistrarPagoAD : IRegistrarPagoAD
    {
        private readonly Contexto _contexto;

        public RegistrarPagoAD()
        {
            _contexto = new Contexto();
        }

        public PagoMembresiaValidacionDto ObtenerDatosMembresia(
            int idMembresiaCliente)
        {
            return
                (from membresia in _contexto.Membresias
                 join plan in _contexto.PlanesMembresia
                    on membresia.idPlanMembresia equals plan.idPlanMembresia
                 where membresia.idMembresiaCliente == idMembresiaCliente
                 select new PagoMembresiaValidacionDto
                 {
                     idMembresiaCliente =
                         membresia.idMembresiaCliente,

                     idPlanMembresia =
                         membresia.idPlanMembresia,

                     idEstadoMembresia =
                         membresia.idEstadoMembresia,

                     fechaInicio =
                         membresia.fechaInicio,

                     fechaFin =
                         membresia.fechaFin,

                     cantidadClasesPlan =
                         plan.cantidadClases,

                     duracionDiasPlan =
                         plan.duracionDias,

                     precioPlan =
                         plan.precio
                 })
                .FirstOrDefault();
        }

        public int RegistrarPagoYActualizarMembresia(
            PagoCrearDto pago,
            DateTime? nuevaFechaInicio,
            DateTime? nuevaFechaFin,
            int? nuevasClasesDisponibles,
            int? nuevoEstadoMembresia)
        {
            using (DbContextTransaction transaccion =
                   _contexto.Database.BeginTransaction())
            {
                try
                {
                    var membresia = _contexto.Membresias
                        .FirstOrDefault(m =>
                            m.idMembresiaCliente ==
                            pago.idMembresiaCliente);

                    if (membresia == null)
                    {
                        transaccion.Rollback();
                        return 0;
                    }

                    var pagoEntidad = new PagoEntidad
                    {
                        idMembresiaCliente =
                            pago.idMembresiaCliente,

                        idMetodoPago =
                            pago.idMetodoPago,

                        idEstadoPago =
                            pago.idEstadoPago,

                        monto =
                            pago.monto,

                        fechaPago =
                            pago.fechaPago,

                        referenciaPago =
                            pago.referenciaPago,

                        observaciones =
                            pago.observaciones
                    };

                    _contexto.Pagos.Add(pagoEntidad);

                    /*
                     * Solo se reciben valores de actualización
                     * cuando el pago fue confirmado como Pagado.
                     */
                    if (nuevoEstadoMembresia.HasValue)
                    {
                        membresia.idEstadoMembresia =
                            nuevoEstadoMembresia.Value;

                        membresia.fechaInicio =
                            nuevaFechaInicio.Value;

                        membresia.fechaFin =
                            nuevaFechaFin.Value;

                        membresia.clasesDisponibles =
                            nuevasClasesDisponibles;

                        string detalleRenovacion =
                            "Membresía activada o renovada por el pago " +
                            "registrado el " +
                            DateTime.Now.ToString("dd/MM/yyyy HH:mm") +
                            ".";

                        if (string.IsNullOrWhiteSpace(
                            membresia.observaciones))
                        {
                            membresia.observaciones =
                                detalleRenovacion;
                        }
                        else
                        {
                            membresia.observaciones +=
                                Environment.NewLine +
                                detalleRenovacion;
                        }
                    }

                    _contexto.SaveChanges();
                    transaccion.Commit();

                    return pagoEntidad.idPago;
                }
                catch
                {
                    transaccion.Rollback();
                    return 0;
                }
            }
        }

        public bool ExisteMetodoPago(int idMetodoPago)
        {
            return _contexto.MetodoPagos.Any(m =>
                m.idMetodoPago == idMetodoPago &&
                m.estado);
        }

        public bool ExisteEstadoPago(int idEstadoPago)
        {
            return _contexto.EstadoPagos.Any(e =>
                e.idEstadoPago == idEstadoPago &&
                e.estado);
        }
    }
}