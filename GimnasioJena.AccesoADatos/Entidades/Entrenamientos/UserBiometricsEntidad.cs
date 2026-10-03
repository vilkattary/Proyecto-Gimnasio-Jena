using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GimnasioJena.AccesoADatos.Entidades.Entrenamientos
{
    [Table("UserBiometrics")]
    public class UserBiometricsEntidad
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }

        [Column(TypeName = "date")]
        public DateTime MeasurementDate { get; set; }

        [Column(TypeName = "decimal")]
        public decimal WeightKg { get; set; }

        [Column(TypeName = "decimal")]
        public decimal? BodyFatPercentage { get; set; }

        [Column(TypeName = "decimal")]
        public decimal? MuscleMassPercentage { get; set; }
    }
}
