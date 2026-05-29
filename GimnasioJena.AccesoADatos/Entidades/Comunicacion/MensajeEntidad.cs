using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GimnasioJena.AccesoADatos.Entidades.Comunicacion
{
    [Table("Mensaje")]
    public class MensajeEntidad
    {
        [Key]
        public int idMensaje { get; set; }

        public int? idUsuario { get; set; }

        [Required]
        [StringLength(100)]
        public string nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string correo { get; set; }

        [StringLength(20)]
        public string telefono { get; set; }

        [Required]
        [StringLength(150)]
        public string asunto { get; set; }

        [Required]
        [StringLength(1000)]
        public string mensaje { get; set; }

        [StringLength(1000)]
        public string respuesta { get; set; }

        public DateTime fechaEnvio { get; set; }

        public DateTime? fechaRespuesta { get; set; }

        [Required]
        [StringLength(30)]
        public string estado { get; set; }
    }
}