using GimnasioJena.Abstracciones.AccesoADatos.Asistencias.RegistrarAsistencia;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Asistencias.RegistrarAsistencia;
using GimnasioJena.Abstracciones.Modelos.Asistencias;
using GimnasioJena.AccesoADatos.Asistencias.RegistrarAsistencia;
using System;

namespace GimnasioJena.LogicaDeNegocio.Asistencias.RegistrarAsistencia
{
    public class RegistrarAsistenciaLN : IRegistrarAsistenciaLN
    {
        private readonly IRegistrarAsistenciaAD _registrarAsistenciaAD;

        public RegistrarAsistenciaLN()
        {
            _registrarAsistenciaAD = new RegistrarAsistenciaAD();
        }

        public ResultadoAsistenciaDto RegistrarAsistencia(
            AsistenciaCrearDto asistencia, int idClaseProgramada)
        {
            if (asistencia == null)
            {
                return CrearResultadoError(
                    "No se recibió la información necesaria para registrar la asistencia."
                );
            }

            if (asistencia.idReserva <= 0)
            {
                return CrearResultadoError(
                    "La reserva seleccionada no es válida."
                );
            }

            if (idClaseProgramada <= 0)
            {
                return CrearResultadoError(
                    "La clase seleccionada no es válida."
                );
            }

            var datosReserva =
                _registrarAsistenciaAD
                    .ObtenerAsistenciaParaValidacion(
                        asistencia.idReserva
                    );

            if (datosReserva == null)
            {
                return CrearResultadoError(
                    "La reserva seleccionada no existe."
                );
            }

            if (datosReserva.idClaseProgramada != idClaseProgramada)
            {
                return CrearResultadoError(
                    "La reserva seleccionada no pertenece a esta clase."
                );
            }

            if (datosReserva.idEstadoReserva == 2)
            {
                return CrearResultadoError(
                    "No se puede registrar asistencia porque la reserva fue cancelada."
                );
            }

            if (datosReserva.idEstadoReserva != 1 &&
                datosReserva.idEstadoReserva != 3 &&
                datosReserva.idEstadoReserva != 4)
            {
                return CrearResultadoError(
                    "El estado actual de la reserva no permite registrar asistencia."
                );
            }

            if (datosReserva.idEstadoClase != 1)
            {
                return CrearResultadoError(
                    "La clase ya no se encuentra activa."
                );
            }

            DateTime fechaHoraClase =
                datosReserva.fechaClase.Date
                    .Add(datosReserva.horaInicio);

            if (DateTime.Now < fechaHoraClase)
            {
                return CrearResultadoError(
                    "La asistencia solo puede registrarse una vez iniciada la clase."
                );
            }

            int resultado =
                _registrarAsistenciaAD
                    .RegistrarAsistencia(asistencia);

            if (resultado <= 0)
            {
                return CrearResultadoError(
                    "No fue posible registrar la asistencia. Intenta nuevamente."
                );
            }

            return CrearResultadoExitoso(
                asistencia.asistio
                    ? "La asistencia se registró correctamente."
                    : "La ausencia se registró correctamente."
            );
        }

        private ResultadoAsistenciaDto CrearResultadoExitoso(
            string mensaje)
        {
            return new ResultadoAsistenciaDto
            {
                fueExitosa = true,
                mensaje = mensaje
            };
        }

        private ResultadoAsistenciaDto CrearResultadoError(
            string mensaje)
        {
            return new ResultadoAsistenciaDto
            {
                fueExitosa = false,
                mensaje = mensaje
            };
        }
    }
}