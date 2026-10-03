using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GimnasioJena.AccesoADatos.Entidades.Entrenamientos
{
    [Table("ProgresionEjercicio")]
    public class ProgresionEjercicioEntidad
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idProgresion { get; set; }

        [ForeignKey("Ejercicio")]
        public int idEjercicio { get; set; }
        public virtual EjercicioEntrenamientoEntidad Ejercicio { get; set; }

        public int NumeroSemana { get; set; }

        public int? Series { get; set; }

        [StringLength(50)]
        public string RepeticionesObjetivo { get; set; }

        public int? DuracionSegundos { get; set; }

        [StringLength(100)]
        public string CargaOrpeSugerido { get; set; }
    }
}
