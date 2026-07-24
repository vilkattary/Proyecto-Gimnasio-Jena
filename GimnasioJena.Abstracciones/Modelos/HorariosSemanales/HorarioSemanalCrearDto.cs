using System;
using System.ComponentModel.DataAnnotations;

namespace GimnasioJena.Abstracciones.Modelos.HorariosSemanales
{
    public class HorarioSemanalCrearDto
    {
        [Required(ErrorMessage = "Debe seleccionar el tipo de clase.")]
        [Display(Name = "Tipo de clase")]
        public int idTipoClase { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un entrenador.")]
        [Display(Name = "Entrenador")]
        public int idUsuarioEntrenador { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el día de la semana.")]
        [Range(1, 7, ErrorMessage = "El día seleccionado no es válido.")]
        [Display(Name = "Día de la semana")]
        public byte diaSemana { get; set; }

        [Required(ErrorMessage = "Debe indicar la hora de inicio.")]
        [Display(Name = "Hora de inicio")]
        public TimeSpan horaInicio { get; set; }

        [Required(ErrorMessage = "Debe indicar la hora de finalización.")]
        [Display(Name = "Hora de finalización")]
        public TimeSpan horaFin { get; set; }

        [Required(ErrorMessage = "Debe indicar el cupo máximo.")]
        [Range(
    1,
    30,
    ErrorMessage = "El cupo máximo debe estar entre 1 y 30."
)]
        [Display(Name = "Cupo máximo")]
        public int cupoMaximo { get; set; }

        public string ubicacion { get; set; }

        public bool estado { get; set; }

        public DateTime fechaCreacion { get; set; }
    }
}