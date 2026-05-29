using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GimnasioJena.AccesoADatos.Entidades.Catalogos
{
    [Table("EstadoMembresia")]
    public class EstadoMembresiaEntidad
    {
        [Key]
        public int idEstadoMembresia { get; set; }

        [Required]
        [StringLength(30)]
        public string nombreEstado { get; set; }

        public bool estado { get; set; }
    }
}