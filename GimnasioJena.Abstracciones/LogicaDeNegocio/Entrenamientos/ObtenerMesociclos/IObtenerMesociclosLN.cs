using GimnasioJena.Abstracciones.Modelos.Entrenamientos;
using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenamientos.ObtenerMesociclos
{
    public interface IObtenerMesociclosLN
    {
        List<MesocicloListadoDto> ObtenerMesociclos();
    }
}
