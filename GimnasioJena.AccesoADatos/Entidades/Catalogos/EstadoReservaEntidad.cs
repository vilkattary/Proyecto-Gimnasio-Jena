using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GimnasioJena.AccesoADatos.Entidades.Catalogos
{
    [Table("EstadoReserva")]
    public class EstadoReservaEntidad
    {
        [Key]
        public int idEstadoReserva { get; set; }

        [Required]
        [StringLength(30)]
        public string nombreEstado { get; set; }

        public bool estado { get; set; }
    }
}