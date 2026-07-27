using GimnasioJena.Abstracciones.Modelos.HorariosSemanales;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.HorariosSemanales.GenerarClasesProgramadas
{
    public interface IGenerarClasesProgramadasLN
    {
        ResultadoGeneracionClasesDto GenerarClasesProgramadas(
            GenerarClasesProgramadasDto modelo
        );
    }
}