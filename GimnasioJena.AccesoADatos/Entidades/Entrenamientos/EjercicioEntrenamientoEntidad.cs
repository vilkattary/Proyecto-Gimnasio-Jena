using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GimnasioJena.AccesoADatos.Entidades.Entrenamientos
{
    [Table("EjercicioEntrenamiento")]
    public class EjercicioEntrenamientoEntidad
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idEjercicio { get; set; }

        [ForeignKey("PlantillaDia")]
        public int idPlantillaDia { get; set; }
        public virtual PlantillaDiaEntrenamientoEntidad PlantillaDia { get; set; }

        [Required]
        [StringLength(150)]
        public string NombreEjercicio { get; set; }

        public int TipoMetrica { get; set; }

        public int OrdenIndice { get; set; }

        [StringLength(500)]
        public string Notas { get; set; }

        public virtual ICollection<ProgresionEjercicioEntidad> Progresiones { get; set; }
            = new List<ProgresionEjercicioEntidad>();
    }
}
