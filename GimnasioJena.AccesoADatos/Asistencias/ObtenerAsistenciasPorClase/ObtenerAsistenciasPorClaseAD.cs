using GimnasioJena.Abstracciones.AccesoADatos.Asistencias.ObtenerAsistenciasPorClase;
using GimnasioJena.Abstracciones.Modelos.Asistencias;
using System.Collections.Generic;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Asistencias.ObtenerAsistenciasPorClase
{
    public class ObtenerAsistenciasPorClaseAD : IObtenerAsistenciasPorClaseAD
    {
        private readonly Contexto _elContexto;

        public ObtenerAsistenciasPorClaseAD()
        {
            _elContexto = new Contexto();
        }

        public List<AsistenciaClaseDto> ObtenerAsistenciasPorClase(int idClaseProgramada)
        {
            var asistencias =
                (from reserva in _elContexto.Reservas
                 join usuario in _elContexto.Usuarios
                    on reserva.idUsuario equals usuario.idUsuario
                 join asistencia in _elContexto.Asistencias
                    on reserva.idReserva equals asistencia.idReserva into asistenciaJoin
                 from asistencia in asistenciaJoin.DefaultIfEmpty()
                 where reserva.idClaseProgramada == idClaseProgramada
                 select new AsistenciaClaseDto
                 {
                     idReserva = reserva.idReserva,
                     idAsistencia = asistencia != null ? (int?)asistencia.idAsistencia : null,
                     nombreCliente = usuario.nombre + " " + usuario.apellido1 + " " + usuario.apellido2,
                     correoCliente = usuario.correo,
                     telefonoCliente = usuario.telefono,
                     asistio = asistencia != null ? (bool?)asistencia.asistio : null,
                     observaciones = asistencia != null ? asistencia.observaciones : null
                 })
                .OrderBy(a => a.nombreCliente)
                .ToList();

            return asistencias;
        }
    }
}