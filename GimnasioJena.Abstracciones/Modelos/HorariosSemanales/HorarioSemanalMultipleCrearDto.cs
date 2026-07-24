using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GimnasioJena.Abstracciones.Modelos.HorariosSemanales
{
    public class HorarioSemanalMultipleCrearDto
    {
        public HorarioSemanalMultipleCrearDto()
        {
            horarios = new List<HorarioSemanalDetalleCrearDto>();
        }

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

        [Required(ErrorMessage = "Debe indicar el cupo máximo.")]
        [Range(1, 30, ErrorMessage = "El cupo debe encontrarse entre 1 y 30.")]
        [Display(Name = "Cupo máximo")]
        public int cupoMaximo { get; set; }

        [Required(ErrorMessage = "Debe indicar la ubicación.")]
        [StringLength(
            100,
            ErrorMessage = "La ubicación no puede superar los 100 caracteres."
        )]
        [Display(Name = "Ubicación")]
        public string ubicacion { get; set; }

        public List<HorarioSemanalDetalleCrearDto> horarios { get; set; }
    }
}