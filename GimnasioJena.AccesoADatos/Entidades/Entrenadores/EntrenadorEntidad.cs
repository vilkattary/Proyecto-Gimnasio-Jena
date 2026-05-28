using GimnasioJena.AccesoADatos.Entidades.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GimnasioJena.AccesoADatos.Entidades.Entrenadores
{
    [Table("Entrenador")]
    public class EntrenadorEntidad
    {
        [Key]
        public int idEntrenador { get; set; }

        public int idUsuario { get; set; }

        [StringLength(100)]
        public string especialidad { get; set; }

        [StringLength(500)]
        public string descripcion { get; set; }

        public DateTime fechaContratacion { get; set; }

        public bool estado { get; set; }

        [ForeignKey("idUsuario")]
        public virtual UsuarioEntidad Usuario { get; set; }
    }
}