using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.Modelos.Entrenamientos
{
    // ViewModel que alimenta la vista parcial del modal de registro del cliente.
    // Combina la rutina prescrita de la clase con la referencia histórica del usuario.
    public class ClaseWorkoutClienteDto
    {
        public int ClassInstanceId { get; set; }
        public int WorkoutDayTemplateId { get; set; }
        public string NombreClase { get; set; }
        public string AreaEnfoque { get; set; }

        // Semana vigente del mesociclo para mostrar la meta correcta (1-4).
        public int NumeroSemana { get; set; }

        public List<EjercicioClienteDto> Ejercicios { get; set; }
            = new List<EjercicioClienteDto>();
    }

    // Ejercicio prescrito con su meta de la semana vigente y la marca previa del cliente.
    public class EjercicioClienteDto
    {
        public int WorkoutExerciseId { get; set; }
        public string NombreEjercicio { get; set; }
        public int TipoMetrica { get; set; }

        // Meta de la semana vigente.
        public int? SeriesObjetivo { get; set; }
        public string RepeticionesObjetivo { get; set; }
        public int? DuracionSegundosObjetivo { get; set; }
        public string CargaSugerida { get; set; }

        // Referencia histórica: último peso registrado por el cliente en este ejercicio.
        public decimal? UltimoPesoKg { get; set; }
    }
}
