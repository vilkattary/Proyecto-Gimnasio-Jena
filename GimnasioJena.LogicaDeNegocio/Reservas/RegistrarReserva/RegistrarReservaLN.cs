using GimnasioJena.Abstracciones.AccesoADatos.Reservas.RegistrarReserva;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.ObtenerMembresiaPorCliente;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.RegistrarReserva;
using GimnasioJena.Abstracciones.Modelos.Reservas;
using GimnasioJena.AccesoADatos.Reservas.RegistrarReserva;
using GimnasioJena.LogicaDeNegocio.Membresias.ObtenerMembresiaPorCliente;
using System;

namespace GimnasioJena.LogicaDeNegocio.Reservas.RegistrarReserva
{
    public class RegistrarReservaLN : IRegistrarReservaLN
    {
        private readonly IRegistrarReservaAD _registrarReservaAD;
        private readonly IObtenerMembresiaPorClienteLN _obtenerMembresiaPorClienteLN;

        public RegistrarReservaLN()
        {
            _registrarReservaAD = new RegistrarReservaAD();
            _obtenerMembresiaPorClienteLN = new ObtenerMembresiaPorClienteLN();
        }

        public ResultadoReservaDto RegistrarReserva(ReservaCrearDto reserva)
        {
            if (reserva == null)
            {
                return CrearResultadoError(
                    "No se recibió la información necesaria para registrar la reserva."
                );
            }

            if (reserva.idUsuario <= 0)
            {
                return CrearResultadoError(
                    "No se pudo identificar al usuario que intenta realizar la reserva."
                );
            }

            if (reserva.idClaseProgramada <= 0)
            {
                return CrearResultadoError(
                    "La clase seleccionada no es válida."
                );
            }

            var clase = _registrarReservaAD
                .ObtenerClaseParaValidacion(reserva.idClaseProgramada);

            if (clase == null)
            {
                return CrearResultadoError(
                    "La clase seleccionada no existe."
                );
            }

            if (clase.idEstadoClase != 1)
            {
                return CrearResultadoError(
                    "La clase seleccionada no se encuentra activa."
                );
            }

            DateTime fechaHoraClase =
                clase.fechaClase.Date.Add(clase.horaInicio);

            DateTime ahora = DateTime.Now;

            DateTime aperturaReserva =
                fechaHoraClase.AddHours(-24);

            DateTime cierreReserva =
                fechaHoraClase.AddMinutes(-10);

            // TESTING: validaciones de ventana horaria deshabilitadas temporalmente
            //if (ahora < aperturaReserva)
            //{
            //    return CrearResultadoError(
            //        "La reserva todavía no está habilitada. " +
            //        "Podrás reservar a partir de 24 horas antes del inicio de la clase."
            //    );
            //}

            //if (ahora >= cierreReserva)
            //{
            //    return CrearResultadoError(
            //        "El periodo para reservar esta clase ya finalizó. " +
            //        "Las reservas cierran 10 minutos antes del inicio."
            //    );
            //}

            var membresia =
                _obtenerMembresiaPorClienteLN
                    .ObtenerMembresiaActivaPorCliente(
                        reserva.idUsuario
                    );

            if (membresia == null)
            {
                return CrearResultadoError(
                    "No tienes una membresía activa para realizar reservas."
                );
            }

            if (membresia.clasesDisponibles.HasValue &&
                membresia.clasesDisponibles.Value <= 0)
            {
                return CrearResultadoError(
                    "Tu membresía no tiene clases disponibles."
                );
            }

            bool yaTieneReservaActiva =
                _registrarReservaAD.UsuarioTieneReservaActiva(
                    reserva.idUsuario,
                    reserva.idClaseProgramada
                );

            if (yaTieneReservaActiva)
            {
                return CrearResultadoError(
                    "Ya tienes una reserva activa para esta clase."
                );
            }

            int reservasActivas =
                _registrarReservaAD
                    .ContarReservasActivasPorClase(
                        reserva.idClaseProgramada
                    );

            if (reservasActivas >= clase.cupoMaximo)
            {
                return CrearResultadoError(
                    "La clase ya alcanzó el cupo máximo permitido."
                );
            }

            reserva.idEstadoReserva = 1;

            int resultado =
                _registrarReservaAD
                    .RegistrarReservaYDescontarClase(
                        reserva,
                        membresia.idMembresiaCliente
                    );

            if (resultado <= 0)
            {
                return CrearResultadoError(
                    "No fue posible registrar la reserva. " +
                    "Intenta nuevamente."
                );
            }

            return CrearResultadoExitoso(
                "La reserva se registró correctamente."
            );
        }

        private ResultadoReservaDto CrearResultadoExitoso(
            string mensaje)
        {
            return new ResultadoReservaDto
            {
                fueExitosa = true,
                mensaje = mensaje
            };
        }

        private ResultadoReservaDto CrearResultadoError(
            string mensaje)
        {
            return new ResultadoReservaDto
            {
                fueExitosa = false,
                mensaje = mensaje
            };
        }
    }
}