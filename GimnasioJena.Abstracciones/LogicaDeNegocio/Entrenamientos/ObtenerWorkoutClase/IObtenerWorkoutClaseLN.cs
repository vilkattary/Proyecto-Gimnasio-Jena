using GimnasioJena.Abstracciones.Modelos.Entrenamientos;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenamientos.ObtenerWorkoutClase
{
    public interface IObtenerWorkoutClaseLN
    {
        ClaseWorkoutClienteDto ObtenerWorkoutClase(int classId, int userId);
    }
}
