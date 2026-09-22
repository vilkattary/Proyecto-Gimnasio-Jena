using GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.ObtenerPlantillasDia;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenamientos.ObtenerPlantillasDia;
using GimnasioJena.Abstracciones.Modelos.Entrenamientos;
using GimnasioJena.AccesoADatos.Entrenamientos.ObtenerPlantillasDia;
using System.Collections.Generic;

namespace GimnasioJena.LogicaDeNegocio.Entrenamientos.ObtenerPlantillasDia
{
    public class ObtenerPlantillasDiaLN : IObtenerPlantillasDiaLN
    {
        private readonly IObtenerPlantillasDiaAD _obtenerPlantillasDiaAD;

        public ObtenerPlantillasDiaLN()
        {
            _obtenerPlantillasDiaAD = new ObtenerPlantillasDiaAD();
        }

        public List<PlantillaDiaDto> ObtenerPlantillasDia(int? idMesociclo)
        {
            return _obtenerPlantillasDiaAD.ObtenerPlantillasDia(idMesociclo);
        }
    }
}
