using GimnasioJena.Abstracciones.Modelos.Entrenamientos;
using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.ObtenerMesociclos
{
    public interface IObtenerMesociclosAD
    {
        List<MesocicloListadoDto> ObtenerMesociclos();
    }
}
