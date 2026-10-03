using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GimnasioJena.AccesoADatos.Entidades.Progreso
{
    [Table("CampoMedicion")]
    public class CampoMedicionEntidad
    {
        [Key]
        public int IdCampo { get; set; }

        [Required]
        [StringLength(50)]
        public string NombreCampo { get; set; }

        public bool Activo { get; set; }
    }
}
