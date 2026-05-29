using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GimnasioJena.AccesoADatos.Entidades.Catalogos
{
    [Table("MetodoPago")]
    public class MetodoPagoEntidad
    {
        [Key]
        public int idMetodoPago { get; set; }

        [Required]
        [StringLength(50)]
        public string nombreMetodo { get; set; }

        public bool estado { get; set; }
    }
}