using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.Modelos.Entrenamientos
{
    // Payload que envía el cliente al registrar sus resultados reales de una clase.
    public class RegistrarEntrenamientoClienteDto
    {
        public int UserId { get; set; }
        public int ClassInstanceId { get; set; }
        public int WorkoutDayTemplateId { get; set; }
        public string Notes { get; set; }

        public List<SerieRegistradaDto> Series { get; set; }
            = new List<SerieRegistradaDto>();
    }
}
