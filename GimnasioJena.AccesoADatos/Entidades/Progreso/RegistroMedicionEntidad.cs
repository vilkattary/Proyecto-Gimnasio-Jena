using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GimnasioJena.AccesoADatos.Entidades.Progreso
{
    [Table("RegistroMedicion")]
    public class RegistroMedicionEntidad
    {
        [Key]
        public int IdRegistroMedicion { get; set; }

        public int idUsuario { get; set; }

        public int IdCampo { get; set; }

        [Column(TypeName = "decimal")]
        public decimal Valor { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime FechaRegistro { get; set; }
    }
}
