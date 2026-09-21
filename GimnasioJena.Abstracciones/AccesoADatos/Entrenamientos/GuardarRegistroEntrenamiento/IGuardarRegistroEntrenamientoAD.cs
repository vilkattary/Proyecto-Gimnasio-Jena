using GimnasioJena.Abstracciones.Modelos.Entrenamientos;

namespace GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.GuardarRegistroEntrenamiento
{
    public interface IGuardarRegistroEntrenamientoAD
    {
        // Persiste el UserWorkoutLog y sus UserSetLog en una transacción.
        ResultadoRegistroEntrenamientoDto GuardarRegistroEntrenamiento(
            RegistrarEntrenamientoClienteDto modelo);

        // Obtiene el mejor Estimated1RM histórico del usuario para un ejercicio
        // (excluyendo el log recién insertado). Sirve para detectar récord personal.
        decimal? ObtenerMejor1RMHistorico(int userId, int workoutExerciseId, int excluirWorkoutLogId);
    }
}
