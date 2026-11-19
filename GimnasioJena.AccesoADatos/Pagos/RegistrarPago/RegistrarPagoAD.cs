using GimnasioJena.Abstracciones.AccesoADatos.Pagos.RegistrarPago;
using GimnasioJena.Abstracciones.Modelos.Pagos;
using GimnasioJena.AccesoADatos.Entidades.Membresias;
using GimnasioJena.AccesoADatos.Entidades.Pagos;
using System;
using System.Data.Entity;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Pagos.RegistrarPago
{
    public class RegistrarPagoAD : IRegistrarPagoAD
    {
        private const int LONGITUD_MAXIMA_REFERENCIA_PAGO = 255;

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

        public PagoMembresiaValidacionDto ObtenerDatosPlanActivo(
               int idPlanMembresia)
        {
            return _contexto.PlanesMembresia
                .Where(p =>
                    p.idPlanMembresia == idPlanMembresia &&
                    p.estado)
                .Select(p => new PagoMembresiaValidacionDto
                {
                    idPlanMembresia = p.idPlanMembresia,
                    cantidadClasesPlan = p.cantidadClases,
                    duracionDiasPlan = p.duracionDias,
                    precioPlan = p.precio
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
                            TruncarReferenciaPago(pago.referenciaPago),

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

        public int RegistrarPrimerPagoManual(
            PagoCrearDto pago,
            DateTime fechaInicio,
            DateTime fechaFin,
            int? clasesDisponibles)
        {
            using (var contexto = new Contexto())
            using (DbContextTransaction transaccion =
                contexto.Database.BeginTransaction())
            {
                try
                {
                    // Evitar crear otra membresía para el mismo cliente.
                    bool tieneMembresia = contexto.Membresias.Any(m =>
                        m.idUsuario == pago.idUsuarioNuevo);

                    if (tieneMembresia)
                    {
                        transaccion.Rollback();
                        return 0;
                    }

                    // Validar que el usuario exista y esté activo.
                    bool usuarioValido = contexto.Usuarios.Any(u =>
                        u.idUsuario == pago.idUsuarioNuevo &&
                        u.estado);

                    if (!usuarioValido)
                    {
                        transaccion.Rollback();
                        return 0;
                    }

                    // Obtener el plan activo y su precio oficial.
                    var plan = contexto.PlanesMembresia
                        .FirstOrDefault(p =>
                            p.idPlanMembresia == pago.idPlanNuevo &&
                            p.estado);

                    if (plan == null || plan.precio <= 0 ||
                        plan.duracionDias <= 0)
                    {
                        transaccion.Rollback();
                        return 0;
                    }

                    // Validar el método de pago.
                    bool metodoValido = contexto.MetodoPagos.Any(m =>
                        m.idMetodoPago == pago.idMetodoPago &&
                        m.estado);

                    if (!metodoValido)
                    {
                        transaccion.Rollback();
                        return 0;
                    }

                    // Crear la primera membresía activa.
                    var membresia = new MembresiaEntidad
                    {
                        idUsuario = pago.idUsuarioNuevo,
                        idPlanMembresia = plan.idPlanMembresia,
                        idEstadoMembresia = 1,
                        fechaInicio = fechaInicio,
                        fechaFin = fechaFin,
                        clasesDisponibles = clasesDisponibles,
                        fechaCreacion = DateTime.Now,
                        observaciones = "Membresía activada mediante pago manual."
                    };

                    contexto.Membresias.Add(membresia);
                    contexto.SaveChanges();

                    // Registrar el pago asociado a la membresía creada.
                    var pagoEntidad = new PagoEntidad
                    {
                        idMembresiaCliente = membresia.idMembresiaCliente,
                        idMetodoPago = pago.idMetodoPago,
                        idEstadoPago = 2,
                        monto = plan.precio,
                        fechaPago = pago.fechaPago,
                        referenciaPago =
                            TruncarReferenciaPago(pago.referenciaPago),
                        observaciones = pago.observaciones
                    };

                    contexto.Pagos.Add(pagoEntidad);
                    contexto.SaveChanges();

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

        public bool ExisteReferenciaPago(string referenciaPago)
        {
            if (string.IsNullOrWhiteSpace(referenciaPago))
                return false;

            string referencia =
                TruncarReferenciaPago(referenciaPago);

            return _contexto.Pagos.Any(p =>
                p.referenciaPago == referencia);
        }

        private static string TruncarReferenciaPago(string referenciaPago)
        {
            if (string.IsNullOrEmpty(referenciaPago))
                return referenciaPago;

            return referenciaPago.Length > LONGITUD_MAXIMA_REFERENCIA_PAGO
                ? referenciaPago.Substring(0, LONGITUD_MAXIMA_REFERENCIA_PAGO)
                : referenciaPago;
        }
    }
}