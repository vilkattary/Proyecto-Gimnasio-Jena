using System;
using System.ComponentModel.DataAnnotations;

namespace GimnasioJena.Abstracciones.Modelos.HorariosSemanales
{
    public class GenerarClasesProgramadasDto
    {
        [Required(
            ErrorMessage = "Debe indicar la fecha inicial."
        )]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha inicial")]
        public DateTime fechaInicio { get; set; }

        [Required(
            ErrorMessage = "Debe indicar la fecha final."
        )]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha final")]
        public DateTime fechaFin { get; set; }
    }
}
