using GimnasioJena.Abstracciones.Modelos.HorariosSemanales;

namespace GimnasioJena.Abstracciones.AccesoADatos
    .HorariosSemanales.ObtenerHorarioSemanalPorId
{
    public interface IObtenerHorarioSemanalPorIdAD
    {
        HorarioSemanalEditarDto ObtenerHorarioSemanalPorId(
            int idHorario
        );
    }
}