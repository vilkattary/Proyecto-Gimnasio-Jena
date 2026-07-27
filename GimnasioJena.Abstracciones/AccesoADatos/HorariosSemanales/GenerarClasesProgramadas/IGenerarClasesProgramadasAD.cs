using GimnasioJena.Abstracciones.Modelos.HorariosSemanales;

namespace GimnasioJena.Abstracciones.AccesoADatos.HorariosSemanales.GenerarClasesProgramadas
{
    public interface IGenerarClasesProgramadasAD
    {
        ResultadoGeneracionClasesDto GenerarClasesProgramadas(
            GenerarClasesProgramadasDto modelo
        );
    }
}