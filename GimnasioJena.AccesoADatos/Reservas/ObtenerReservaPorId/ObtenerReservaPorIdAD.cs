using GimnasioJena.Abstracciones.AccesoADatos.Reservas.ObtenerReservaPorId;
using GimnasioJena.Abstracciones.Modelos.Reservas;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Reservas.ObtenerReservaPorId
{
    public class ObtenerReservaPorIdAD : IObtenerReservaPorIdAD
    {
        private readonly Contexto _contexto;

        public ObtenerReservaPorIdAD()
        {
            _contexto = new Contexto();
        }

        public ReservaListadoDto ObtenerReservaPorId(int idReserva)
        {
            return (
                from r in _contexto.Reservas
                join u in _contexto.Usuarios
                    on r.idUsuario equals u.idUsuario
                join cp in _contexto.Clases
                    on r.idClaseProgramada equals cp.idClaseProgramada
                join tc in _contexto.TiposClase
                    on cp.idTipoClase equals tc.idTipoClase
                join er in _contexto.EstadoReservas
                    on r.idEstadoReserva equals er.idEstadoReserva
                join ue in _contexto.Usuarios
                    on cp.idUsuarioEntrenador equals ue.idUsuario
                where r.idReserva == idReserva
                select new ReservaListadoDto
                {
                    idReserva = r.idReserva,
                    idUsuario = r.idUsuario,
                    idClaseProgramada = cp.idClaseProgramada,
                    nombreCliente = u.nombre + " " + u.apellido1 + " " + u.apellido2,
                    nombreClase = tc.nombreClase,
                    nombreEntrenador = ue.nombre + " " + ue.apellido1 + " " + ue.apellido2,
                    idEstadoReserva = er.idEstadoReserva,
                    estadoReserva = er.nombreEstado,
                    fechaClase = cp.fechaClase,
                    horaInicio = cp.horaInicio,
                    horaFin = cp.horaFin,
                    fechaReserva = r.fechaReserva,
                    ubicacion = cp.ubicacion
                }
            ).FirstOrDefault();
        }
    }
}