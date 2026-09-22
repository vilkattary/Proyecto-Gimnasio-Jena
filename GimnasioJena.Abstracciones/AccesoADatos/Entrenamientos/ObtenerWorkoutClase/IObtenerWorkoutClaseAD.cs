using GimnasioJena.Abstracciones.Modelos.Entrenamientos;

namespace GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.ObtenerWorkoutClase
{
    public interface IObtenerWorkoutClaseAD
    {
        // Resuelve la rutina prescrita de una clase y adjunta la marca histórica del usuario.
        ClaseWorkoutClienteDto ObtenerWorkoutClase(int classId, int userId);
    }
}
