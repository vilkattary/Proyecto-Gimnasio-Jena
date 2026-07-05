using GimnasioJena.Abstracciones.AccesoADatos.Reservas.ObtenerReservasPorUsuario;
using GimnasioJena.Abstracciones.Modelos.Reservas;
using System.Collections.Generic;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Reservas.ObtenerReservasPorUsuario
{
    public class ObtenerReservasPorUsuarioAD : IObtenerReservasPorUsuarioAD
    {
        private readonly Contexto _contexto;

        public ObtenerReservasPorUsuarioAD()
        {
            _contexto = new Contexto();
        }

        public List<ReservaListadoDto> ObtenerReservasPorUsuario(int idUsuario)
        {
            var q =
                from reserva in _contexto.Reservas
                join clase in _contexto.Clases on reserva.idClaseProgramada equals clase.idClaseProgramada
                join tipo in _contexto.TiposClase on clase.idTipoClase equals tipo.idTipoClase
                join entrenador in _contexto.Usuarios on clase.idUsuarioEntrenador equals entrenador.idUsuario
                join estado in _contexto.EstadoReservas on reserva.idEstadoReserva equals estado.idEstadoReserva
                where reserva.idUsuario == idUsuario
                select new ReservaListadoDto
                {
                    idReserva = reserva.idReserva,
                    idUsuario = reserva.idUsuario,
                    idClaseProgramada = reserva.idClaseProgramada,
                    nombreCliente = string.Empty,
                    nombreClase = tipo.nombreClase,
                    nombreEntrenador = entrenador.nombre + " " + entrenador.apellido1,
                    idEstadoReserva = reserva.idEstadoReserva,
                    estadoReserva = estado.nombreEstado,
                    fechaClase = clase.fechaClase,
                    horaInicio = clase.horaInicio,
                    horaFin = clase.horaFin,
                    fechaReserva = reserva.fechaReserva,
                    ubicacion = clase.ubicacion
                };

            return q
                .OrderBy(r => r.fechaClase)
                .ThenBy(r => r.horaInicio)
                .ToList();
        }
    }
}