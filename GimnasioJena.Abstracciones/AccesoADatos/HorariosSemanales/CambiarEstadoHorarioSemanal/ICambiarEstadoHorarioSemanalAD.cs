namespace GimnasioJena.Abstracciones.AccesoADatos
    .HorariosSemanales.CambiarEstadoHorarioSemanal
{
    public interface ICambiarEstadoHorarioSemanalAD
    {
        bool CambiarEstadoHorarioSemanal(
            int idHorario
        );
    }
}