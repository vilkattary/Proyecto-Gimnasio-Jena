using GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.ObtenerPlantillaDiaPorId;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenamientos.ObtenerPlantillaDiaPorId;
using GimnasioJena.Abstracciones.Modelos.Entrenamientos;
using GimnasioJena.AccesoADatos.Entrenamientos.ObtenerPlantillaDiaPorId;

namespace GimnasioJena.LogicaDeNegocio.Entrenamientos.ObtenerPlantillaDiaPorId
{
    public class ObtenerPlantillaDiaPorIdLN : IObtenerPlantillaDiaPorIdLN
    {
        private readonly IObtenerPlantillaDiaPorIdAD _obtenerPlantillaDiaPorIdAD;

        public ObtenerPlantillaDiaPorIdLN()
        {
            _obtenerPlantillaDiaPorIdAD = new ObtenerPlantillaDiaPorIdAD();
        }

        public PlantillaDiaDto ObtenerPlantillaDiaPorId(int idPlantillaDia)
        {
            return _obtenerPlantillaDiaPorIdAD.ObtenerPlantillaDiaPorId(idPlantillaDia);
        }
    }
}
