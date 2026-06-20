using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GimnasioJena.AccesoADatos.Entidades.Home
{
    [Table("ContenidoWeb")]
    public class ContenidoWeb
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Pagina { get; set; }

        [Required]
        [StringLength(50)]
        public string Seccion { get; set; }

        [Required]
        [StringLength(50)]
        public string Clave { get; set; }

        public string TextoPrincipal { get; set; }

        public string TextoSecundario { get; set; }

        [StringLength(2048)]
        public string UrlImagen { get; set; }

        public int Orden { get; set; }

        public DateTime? FechaModificacion { get; set; }
    }
}
