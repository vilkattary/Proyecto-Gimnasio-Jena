using GimnasioJena.Abstracciones.Modelos.Entrenamientos;
using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.ObtenerPlantillasDia
{
    public interface IObtenerPlantillasDiaAD
    {
        List<PlantillaDiaDto> ObtenerPlantillasDia(int? idMesociclo);
    }
}
