using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GimnasioJena.AccesoADatos.Entidades.Catalogos
{
    [Table("EstadoPago")]
    public class EstadoPagoEntidad
    {
        [Key]
        public int idEstadoPago { get; set; }

        [Required]
        [StringLength(30)]
        public string nombreEstado { get; set; }

        public bool estado { get; set; }
    }
}