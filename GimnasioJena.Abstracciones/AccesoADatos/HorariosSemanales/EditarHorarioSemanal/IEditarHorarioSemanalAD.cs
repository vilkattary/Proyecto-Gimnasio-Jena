using GimnasioJena.Abstracciones.Modelos.HorariosSemanales;

namespace GimnasioJena.Abstracciones.AccesoADatos
    .HorariosSemanales.EditarHorarioSemanal
{
    public interface IEditarHorarioSemanalAD
    {
        void EditarHorarioSemanal(
            HorarioSemanalEditarDto modelo
        );
    }
}
