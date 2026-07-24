using GimnasioJena.Abstracciones.AccesoADatos.Reservas.RegistrarReserva;
using GimnasioJena.Abstracciones.Modelos.Reservas;
using GimnasioJena.AccesoADatos.Entidades.Reservas;
using System;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Reservas.RegistrarReserva
{
    public class RegistrarReservaAD : IRegistrarReservaAD
    {
        private readonly Contexto _elContexto;

        public RegistrarReservaAD()
        {
            _elContexto = new Contexto();
        }

        public int RegistrarReserva(ReservaCrearDto reserva)
        {
            var reservaExistente = _elContexto.Reservas.FirstOrDefault(r =>
                r.idUsuario == reserva.idUsuario &&
                r.idClaseProgramada == reserva.idClaseProgramada);

            if (reservaExistente != null)
            {
                if (reservaExistente.idEstadoReserva == 1)
                {
                    return 0;
                }

                reservaExistente.idEstadoReserva = 1;
                reservaExistente.fechaReserva = DateTime.Now;
                reservaExistente.observaciones = reserva.observaciones;

                return _elContexto.SaveChanges();
            }

            var reservaAGuardar = new ReservaEntidad
            {
                idUsuario = reserva.idUsuario,
                idClaseProgramada = reserva.idClaseProgramada,
                idEstadoReserva = reserva.idEstadoReserva,
                fechaReserva = DateTime.Now,
                observaciones = reserva.observaciones
            };

            _elContexto.Reservas.Add(reservaAGuardar);
            return _elContexto.SaveChanges();
        }
        public bool DescontarClaseDisponible(int idMembresiaCliente)
        {
            var membresia = _elContexto.Membresias
                .FirstOrDefault(m => m.idMembresiaCliente == idMembresiaCliente);

            if (membresia == null)
                return false;

            if (!membresia.clasesDisponibles.HasValue)
                return true;

            if (membresia.clasesDisponibles.Value <= 0)
                return false;

            membresia.clasesDisponibles = membresia.clasesDisponibles.Value - 1;

            return _elContexto.SaveChanges() > 0;
        }

        public int RegistrarReservaYDescontarClase(
    ReservaCrearDto reserva,
    int idMembresiaCliente)
        {
            using (var transaccion =
                _elContexto.Database.BeginTransaction())
            {
                try
                {
                    var membresia = _elContexto.Membresias
                        .FirstOrDefault(m =>
                            m.idMembresiaCliente ==
                            idMembresiaCliente);

                    if (membresia == null)
                    {
                        transaccion.Rollback();
                        return 0;
                    }

                    /*
                     * Si clasesDisponibles es NULL,
                     * se interpreta como una membresía
                     * con clases ilimitadas.
                     */
                    if (membresia.clasesDisponibles.HasValue)
                    {
                        if (membresia.clasesDisponibles.Value <= 0)
                        {
                            transaccion.Rollback();
                            return 0;
                        }

                        membresia.clasesDisponibles =
                            membresia.clasesDisponibles.Value - 1;
                    }

                    var reservaExistente =
                        _elContexto.Reservas.FirstOrDefault(r =>
                            r.idUsuario == reserva.idUsuario &&
                            r.idClaseProgramada ==
                            reserva.idClaseProgramada);

                    if (reservaExistente != null)
                    {
                        /*
                         * Si ya está activa, no se vuelve
                         * a registrar ni se descuenta otra clase.
                         */
                        if (reservaExistente.idEstadoReserva == 1)
                        {
                            transaccion.Rollback();
                            return 0;
                        }

                        /*
                         * Si anteriormente fue cancelada,
                         * se reactiva el mismo registro.
                         */
                        reservaExistente.idEstadoReserva = 1;
                        reservaExistente.fechaReserva = DateTime.Now;
                        reservaExistente.observaciones =
                            reserva.observaciones;
                    }
                    else
                    {
                        var reservaAGuardar =
                            new ReservaEntidad
                            {
                                idUsuario =
                                    reserva.idUsuario,

                                idClaseProgramada =
                                    reserva.idClaseProgramada,

                                idEstadoReserva = 1,

                                fechaReserva =
                                    DateTime.Now,

                                observaciones =
                                    reserva.observaciones
                            };

                        _elContexto.Reservas.Add(
                            reservaAGuardar
                        );
                    }

                    int registrosAfectados =
                        _elContexto.SaveChanges();

                    if (registrosAfectados <= 0)
                    {
                        transaccion.Rollback();
                        return 0;
                    }

                    transaccion.Commit();

                    return registrosAfectados;
                }
                catch
                {
                    transaccion.Rollback();
                    return 0;
                }
            }
        }
        public ReservaClaseValidacionDto ObtenerClaseParaValidacion(int idClaseProgramada)
        {
            return _elContexto.Clases
                .Where(c => c.idClaseProgramada == idClaseProgramada)
                .Select(c => new ReservaClaseValidacionDto
                {
                    idClaseProgramada = c.idClaseProgramada,
                    idEstadoClase = c.idEstadoClase,
                    fechaClase = c.fechaClase,
                    horaInicio = c.horaInicio,
                    cupoMaximo = c.cupoMaximo
                })
                .FirstOrDefault();
        }

        public bool UsuarioTieneReservaActiva(int idUsuario, int idClaseProgramada)
        {
            return _elContexto.Reservas.Any(r =>
                r.idUsuario == idUsuario &&
                r.idClaseProgramada == idClaseProgramada &&
                r.idEstadoReserva == 1);
        }

        public int ContarReservasActivasPorClase(int idClaseProgramada)
        {
            return _elContexto.Reservas.Count(r =>
                r.idClaseProgramada == idClaseProgramada &&
                r.idEstadoReserva == 1);
        }
    }
}