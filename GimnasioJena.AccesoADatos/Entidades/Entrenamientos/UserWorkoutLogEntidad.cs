using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GimnasioJena.AccesoADatos.Entidades.Entrenamientos
{
    [Table("UserWorkoutLog")]
    public class UserWorkoutLogEntidad
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ClassInstanceId { get; set; }

        public int WorkoutDayTemplateId { get; set; }

        public DateTime LoggedDate { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        public virtual ICollection<UserSetLogEntidad> Series { get; set; }
    }
}
