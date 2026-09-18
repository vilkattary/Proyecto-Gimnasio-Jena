using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GimnasioJena.Abstracciones.Modelos.Entrenamientos
{
    public class MesocicloDto
    {
        public int idMesociclo { get; set; }

        [Required(ErrorMessage = "Debe indicar el nombre del mesociclo.")]
        [StringLength(150)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debe indicar la fecha de inicio.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de inicio")]
        public DateTime FechaInicio { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de fin")]
        public DateTime FechaFin { get; set; }

        [Display(Name = "Activo")]
        public bool EsActivo { get; set; }

        public List<PlantillaDiaDto> PlantillasDia { get; set; }
            = new List<PlantillaDiaDto>();
    }
}
