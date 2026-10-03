using GimnasioJena.Abstracciones.Modelos.Entrenamientos;

namespace GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.ObtenerPlantillaDiaPorId
{
    public interface IObtenerPlantillaDiaPorIdAD
    {
        PlantillaDiaDto ObtenerPlantillaDiaPorId(int idPlantillaDia);
    }
}
