using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GimnasioJena.AccesoADatos.Entidades.Usuarios
{
    [Table("Usuario")]
    public class UsuarioEntidad
    {
        [Key]
        public int idUsuario { get; set; }

        [StringLength(128)]
        public string identityUserId { get; set; }

        [Required]
        [StringLength(50)]
        public string nombre { get; set; }

        [Required]
        [StringLength(50)]
        public string apellido1 { get; set; }

        [StringLength(50)]
        public string apellido2 { get; set; }

        [Required]
        [StringLength(50)]
        public string identificacion { get; set; }

        [Required]
        [StringLength(100)]
        public string correo { get; set; }

        [StringLength(20)]
        public string telefono { get; set; }

        public DateTime fechaRegistro { get; set; }

        public DateTime? fechaModificacion { get; set; }

        public bool estado { get; set; }
    }
}
