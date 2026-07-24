using GimnasioJena.Abstracciones.Modelos.HorariosSemanales;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio
    .HorariosSemanales.EditarHorarioSemanal
{
    public interface IEditarHorarioSemanalLN
    {
        void EditarHorarioSemanal(
            HorarioSemanalEditarDto modelo
        );
    }
}