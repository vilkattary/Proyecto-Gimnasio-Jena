using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GimnasioJena.AccesoADatos.Entidades.Catalogos
{
    [Table("EstadoClase")]
    public class EstadoClaseEntidad
    {
        [Key]
        public int idEstadoClase { get; set; }

        [Required]
        [StringLength(30)]
        public string nombreEstado { get; set; }

        public bool estado { get; set; }
    }
}