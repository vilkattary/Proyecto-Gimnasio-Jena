namespace GimnasioJena.Abstracciones.Modelos.Entrenamientos
{
    // Detalle de una serie completada enviada por el cliente.
    public class SerieRegistradaDto
    {
        public int WorkoutExerciseId { get; set; }
        public int SetNumber { get; set; }
        public int? RepsCompleted { get; set; }
        public decimal? WeightUsedKg { get; set; }
        public int? DurationSeconds { get; set; }
        public decimal? DistanceMeters { get; set; }

        // Calculados en la capa LN antes de persistir.
        public decimal? Estimated1RM { get; set; }
        public decimal? VolumeLoad { get; set; }
    }
}
