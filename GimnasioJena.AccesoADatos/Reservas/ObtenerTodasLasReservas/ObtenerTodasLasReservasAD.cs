using GimnasioJena.Abstracciones.AccesoADatos.Reservas.ObtenerTodasLasReservas;
using GimnasioJena.Abstracciones.Modelos.Reservas;
using System.Collections.Generic;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Reservas.ObtenerTodasLasReservas
{
    public class ObtenerTodasLasReservasAD : IObtenerTodasLasReservasAD
    {
        private readonly Contexto _contexto;

        public ObtenerTodasLasReservasAD()
        {
            _contexto = new Contexto();
        }

        public List<ReservaListadoDto> ObtenerTodasLasReservas()
        {
            var reservas =
                (from r in _contexto.Reservas
                 join u in _contexto.Usuarios
                    on r.idUsuario equals u.idUsuario
                 join c in _contexto.Clases
                    on r.idClaseProgramada equals c.idClaseProgramada
                 join tc in _contexto.TiposClase
                    on c.idTipoClase equals tc.idTipoClase
                 join er in _contexto.EstadoReservas
                    on r.idEstadoReserva equals er.idEstadoReserva
                 join entrenador in _contexto.Usuarios
                    on c.idUsuarioEntrenador equals entrenador.idUsuario
                 select new ReservaListadoDto
                 {
                     idReserva = r.idReserva,
                     idUsuario = r.idUsuario,
                     idClaseProgramada = r.idClaseProgramada,
                     nombreCliente = u.nombre + " " + u.apellido1 + " " + u.apellido2,
                     nombreClase = tc.nombreClase,
                     nombreEntrenador = entrenador.nombre + " " + entrenador.apellido1 + " " + entrenador.apellido2,
                     idEstadoReserva = r.idEstadoReserva,
                     estadoReserva = er.nombreEstado,
                     fechaClase = c.fechaClase,
                     horaInicio = c.horaInicio,
                     horaFin = c.horaFin,
                     fechaReserva = r.fechaReserva,
                     ubicacion = c.ubicacion
                 })
                .OrderByDescending(r => r.fechaReserva)
                .ToList();

            return reservas;
        }
    }
}