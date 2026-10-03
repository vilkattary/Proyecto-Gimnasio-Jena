using GimnasioJena.Abstracciones.Modelos.Entrenamientos;
using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenamientos.ObtenerPlantillasDia
{
    public interface IObtenerPlantillasDiaLN
    {
        List<PlantillaDiaDto> ObtenerPlantillasDia(int? idMesociclo);
    }
}
