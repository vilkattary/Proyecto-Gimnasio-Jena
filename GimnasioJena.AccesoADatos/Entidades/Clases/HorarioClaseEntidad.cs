using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GimnasioJena.AccesoADatos.Entidades.Clases
{
    [Table("HorarioClase")]
    public class HorarioClaseEntidad
    {
        [Key]
        public int idHorarioClase { get; set; }

        public int    idTipoClase   { get; set; }
        public int    idEstadoClase { get; set; }
        public int    cupoMaximo    { get; set; }

        [StringLength(500)]
        public string observaciones { get; set; }

        public DateTime fechaCreacion { get; set; }
    }
}
