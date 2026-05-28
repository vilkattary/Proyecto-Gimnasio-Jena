using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GimnasioJena.AccesoADatos.Entidades.Catalogos
{
    [Table("TipoClase")]
    public class TipoClaseEntidad
    {
        [Key]
        public int idTipoClase { get; set; }

        [Required]
        [StringLength(100)]
        public string nombreClase { get; set; }

        [StringLength(500)]
        public string descripcion { get; set; }

        public int duracionMinutos { get; set; }

        public bool estado { get; set; }
    }
}