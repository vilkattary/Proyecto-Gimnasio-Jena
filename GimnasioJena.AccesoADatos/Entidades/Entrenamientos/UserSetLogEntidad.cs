using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GimnasioJena.AccesoADatos.Entidades.Entrenamientos
{
    [Table("UserSetLog")]
    public class UserSetLogEntidad
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("WorkoutLog")]
        public int UserWorkoutLogId { get; set; }
        public virtual UserWorkoutLogEntidad WorkoutLog { get; set; }

        [ForeignKey("Ejercicio")]
        public int WorkoutExerciseId { get; set; }
        public virtual EjercicioEntrenamientoEntidad Ejercicio { get; set; }

        public int SetNumber { get; set; }

        public int? RepsCompleted { get; set; }

        [Column(TypeName = "decimal")]
        public decimal? WeightUsedKg { get; set; }

        public int? DurationSeconds { get; set; }

        [Column(TypeName = "decimal")]
        public decimal? DistanceMeters { get; set; }

        [Column(TypeName = "decimal")]
        public decimal? Estimated1RM { get; set; }

        [Column(TypeName = "decimal")]
        public decimal? VolumeLoad { get; set; }
    }
}
