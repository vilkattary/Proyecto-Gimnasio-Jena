using System;
using System.ComponentModel.DataAnnotations;

namespace GimnasioJena.Abstracciones.Modelos.HorariosSemanales
{
    public class HorarioSemanalDetalleCrearDto
    {
        [Required(ErrorMessage = "Debe indicar la hora de inicio.")]
        [Display(Name = "Hora de inicio")]
        public TimeSpan horaInicio { get; set; }

        [Required(ErrorMessage = "Debe indicar la hora de finalización.")]
        [Display(Name = "Hora de finalización")]
        public TimeSpan horaFin { get; set; }
    }
}