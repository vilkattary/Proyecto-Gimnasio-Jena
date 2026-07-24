namespace GimnasioJena.Abstracciones.LogicaDeNegocio
    .HorariosSemanales.CambiarEstadoHorarioSemanal
{
    public interface ICambiarEstadoHorarioSemanalLN
    {
        bool CambiarEstadoHorarioSemanal(
            int idHorario
        );
    }
}
