using GimnasioJena.Abstracciones.AccesoADatos
    .HorariosSemanales.ObtenerHorarioSemanalPorId;
using GimnasioJena.Abstracciones.Modelos.HorariosSemanales;
using System.Linq;

namespace GimnasioJena.AccesoADatos.HorariosSemanales
    .ObtenerHorarioSemanalPorId
{
    public class ObtenerHorarioSemanalPorIdAD
        : IObtenerHorarioSemanalPorIdAD
    {
        private readonly Contexto _contexto;

        public ObtenerHorarioSemanalPorIdAD()
        {
            _contexto = new Contexto();
        }

        public HorarioSemanalEditarDto ObtenerHorarioSemanalPorId(
            int idHorario
        )
        {
            return _contexto.HorariosSemanales
                .Where(h =>
                    h.idHorario == idHorario
                )
                .Select(h =>
                    new HorarioSemanalEditarDto
                    {
                        idHorario =
                            h.idHorario,

                        idTipoClase =
                            h.idTipoClase,

                        idUsuarioEntrenador =
                            h.idUsuarioEntrenador,

                        diaSemana =
                            h.diaSemana,

                        horaInicio =
                            h.horaInicio,

                        horaFin =
                            h.horaFin,

                        cupoMaximo =
                            h.cupoMaximo,

                        ubicacion =
                            h.ubicacion,

                        estado =
                            h.estado,

                        fechaModificacion =
                            h.fechaModificacion
                    }
                )
                .FirstOrDefault();
        }
    }
}
