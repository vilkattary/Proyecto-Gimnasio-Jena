using GimnasioJena.Abstracciones.AccesoADatos.Asistencias.RegistrarAsistencia;
using GimnasioJena.Abstracciones.Modelos.Asistencias;
using GimnasioJena.AccesoADatos.Entidades.Asistencias;
using System;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Asistencias.RegistrarAsistencia
{
    public class RegistrarAsistenciaAD : IRegistrarAsistenciaAD
    {
        private readonly Contexto _elContexto;

        public RegistrarAsistenciaAD()
        {
            _elContexto = new Contexto();
        }

        public int RegistrarAsistencia(AsistenciaCrearDto asistencia)
        {
            var reserva = _elContexto.Reservas
                .FirstOrDefault(r => r.idReserva == asistencia.idReserva);

            if (reserva == null)
                return 0;

            if (reserva.idEstadoReserva != 1 &&
                reserva.idEstadoReserva != 3 &&
                reserva.idEstadoReserva != 4)
                return 0;

            var asistenciaExistente = _elContexto.Asistencias
                .FirstOrDefault(a => a.idReserva == asistencia.idReserva);

            if (asistenciaExistente != null)
            {
                asistenciaExistente.asistio = asistencia.asistio;
                asistenciaExistente.observaciones = asistencia.observaciones;
                asistenciaExistente.fechaRegistro = DateTime.Now;
                asistenciaExistente.idUsuarioRecepcionista = asistencia.idUsuarioRecepcionista;
            }
            else
            {
                var asistenciaNueva = new AsistenciaEntidad
                {
                    idReserva = asistencia.idReserva,
                    idUsuarioRecepcionista = asistencia.idUsuarioRecepcionista,
                    fechaRegistro = DateTime.Now,
                    asistio = asistencia.asistio,
                    observaciones = asistencia.observaciones
                };

                _elContexto.Asistencias.Add(asistenciaNueva);
            }

            reserva.idEstadoReserva = asistencia.asistio ? 3 : 4;

            return _elContexto.SaveChanges();
        }
    }
}