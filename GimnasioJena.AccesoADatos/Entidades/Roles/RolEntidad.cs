using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace GimnasioJena.AccesoADatos.Entidades.Roles
{
        [Table("Rol")]
        public class RolEntidad
        {
            [Key]
            public int idRol { get; set; }

            [Required]
            [StringLength(50)]
            public string nombreRol { get; set; }

            [StringLength(200)]
            public string descripcion { get; set; }

            public bool estado { get; set; }

            public DateTime fechaCreacion { get; set; }
        }
    }