using GimnasioJena.Abstracciones.Modelos.HorariosSemanales;

namespace GimnasioJena.Abstracciones.AccesoADatos.HorariosSemanales.RegistrarHorariosSemanales
{
    public interface IRegistrarHorariosSemanalesAD
    {
        int RegistrarHorariosSemanales(
            HorarioSemanalMultipleCrearDto horarios
        );
    }
}