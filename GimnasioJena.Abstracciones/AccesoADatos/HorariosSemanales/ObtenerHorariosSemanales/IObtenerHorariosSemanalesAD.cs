using GimnasioJena.Abstracciones.Modelos.HorariosSemanales;
using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.AccesoADatos.HorariosSemanales.ObtenerHorariosSemanales
{
    public interface IObtenerHorariosSemanalesAD
    {
        List<HorarioSemanalListadoDto> ObtenerHorariosSemanales();
    }
}