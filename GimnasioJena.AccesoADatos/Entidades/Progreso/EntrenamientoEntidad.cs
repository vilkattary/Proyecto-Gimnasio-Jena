using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GimnasioJena.AccesoADatos.Entidades.Clases;
using GimnasioJena.AccesoADatos.Entidades.HorariosSemanales;

namespace GimnasioJena.AccesoADatos.Entidades.Progreso
{
    [Table("Entrenamiento")]
    public class EntrenamientoEntidad
    {
        [Key]
        public int IdEntrenamiento { get; set; }

        [Required]
        [StringLength(20)]
        public string DiaSemana { get; set; }

        [Required]
        [StringLength(100)]
        public string NombreRutina { get; set; }

        public string EjerciciosDetalle { get; set; }

        public bool EsPlantilla { get; set; }

        [ForeignKey("ClaseProgramada")]
        public int? idClaseProgramada { get; set; }
        public virtual ClaseEntidad ClaseProgramada { get; set; }

        [ForeignKey("HorarioSemanal")]
        public int? idHorario { get; set; }
        public virtual HorarioSemanalEntidad HorarioSemanal { get; set; }
    }
}
