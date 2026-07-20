using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GimnasioJena.AccesoADatos.Entidades.Clases
{
    [Table("ClaseProgramada")]
    public class ClaseEntidad
    {
        [Key]
        public int idClaseProgramada { get; set; }

        public int? idHorarioClase     { get; set; }
        public int  idTipoClase        { get; set; }
        public int  idUsuarioEntrenador { get; set; }
        public int  idEstadoClase      { get; set; }

        public DateTime fechaClase { get; set; }

        public TimeSpan horaInicio { get; set; }
        public TimeSpan horaFin { get; set; }

        public int cupoMaximo { get; set; }

        [StringLength(100)]
        public string ubicacion { get; set; }

        [StringLength(500)]
        public string observaciones { get; set; }

        public DateTime fechaCreacion { get; set; }

        public DateTime? fechaModificacion { get; set; }
    }
}