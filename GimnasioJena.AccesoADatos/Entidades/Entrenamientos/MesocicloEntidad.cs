using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GimnasioJena.AccesoADatos.Entidades.Entrenamientos
{
    [Table("Mesociclo")]
    public class MesocicloEntidad
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idMesociclo { get; set; }

        [Required]
        [StringLength(150)]
        public string Nombre { get; set; }

        [Column(TypeName = "date")]
        public DateTime FechaInicio { get; set; }

        [Column(TypeName = "date")]
        public DateTime FechaFin { get; set; }

        public bool EsActivo { get; set; }

        public virtual ICollection<PlantillaDiaEntrenamientoEntidad> PlantillasDia { get; set; }
            = new List<PlantillaDiaEntrenamientoEntidad>();
    }
}
