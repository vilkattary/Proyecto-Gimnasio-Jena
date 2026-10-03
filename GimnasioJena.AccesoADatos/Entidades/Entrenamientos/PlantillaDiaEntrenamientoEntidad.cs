using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GimnasioJena.AccesoADatos.Entidades.Entrenamientos
{
    [Table("PlantillaDiaEntrenamiento")]
    public class PlantillaDiaEntrenamientoEntidad
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idPlantillaDia { get; set; }

        [ForeignKey("Mesociclo")]
        public int idMesociclo { get; set; }
        public virtual MesocicloEntidad Mesociclo { get; set; }

        public byte DiaSemana { get; set; }

        [StringLength(100)]
        public string AreaEnfoque { get; set; }

        public int OrdenIndice { get; set; }

        public virtual ICollection<EjercicioEntrenamientoEntidad> Ejercicios { get; set; }
            = new List<EjercicioEntrenamientoEntidad>();
    }
}
