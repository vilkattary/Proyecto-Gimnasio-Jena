using GimnasioJena.Abstracciones.Modelos.Entrenamientos;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenamientos.ObtenerPlantillaDiaPorId
{
    public interface IObtenerPlantillaDiaPorIdLN
    {
        PlantillaDiaDto ObtenerPlantillaDiaPorId(int idPlantillaDia);
    }
}
