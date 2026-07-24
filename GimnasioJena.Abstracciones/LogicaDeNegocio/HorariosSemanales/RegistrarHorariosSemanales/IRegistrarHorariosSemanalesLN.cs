using GimnasioJena.Abstracciones.Modelos.HorariosSemanales;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.HorariosSemanales.RegistrarHorariosSemanales
{
    public interface IRegistrarHorariosSemanalesLN
    {
        ResultadoRegistroHorariosDto RegistrarHorariosSemanales(
            HorarioSemanalMultipleCrearDto horarios
        );
    }
}