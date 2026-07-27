using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GimnasioJena.AccesoADatos.Entidades.HorariosSemanales
{
    [Table("HorarioSemanal")]
    public class HorarioSemanalEntidad
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idHorario { get; set; }

        public int idTipoClase { get; set; }

        public int idUsuarioEntrenador { get; set; }

        public byte diaSemana { get; set; }

        public TimeSpan horaInicio { get; set; }

        public TimeSpan horaFin { get; set; }

        public int cupoMaximo { get; set; }

        [StringLength(100)]
        public string ubicacion { get; set; }

        public bool estado { get; set; }

        public DateTime fechaCreacion { get; set; }

        public DateTime? fechaModificacion { get; set; }
    }
}