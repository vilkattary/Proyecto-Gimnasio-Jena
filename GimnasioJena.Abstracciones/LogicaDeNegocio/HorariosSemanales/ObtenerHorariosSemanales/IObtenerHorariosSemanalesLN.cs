using GimnasioJena.Abstracciones.Modelos.HorariosSemanales;
using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.HorariosSemanales.ObtenerHorariosSemanales
{
    public interface IObtenerHorariosSemanalesLN
    {
        List<HorarioSemanalListadoDto> ObtenerHorariosSemanales();
    }
}