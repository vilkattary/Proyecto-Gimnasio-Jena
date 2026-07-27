using GimnasioJena.Abstracciones.Modelos.HorariosSemanales;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio
    .HorariosSemanales.ObtenerHorarioSemanalPorId
{
    public interface IObtenerHorarioSemanalPorIdLN
    {
        HorarioSemanalEditarDto ObtenerHorarioSemanalPorId(
            int idHorario
        );
    }
}
