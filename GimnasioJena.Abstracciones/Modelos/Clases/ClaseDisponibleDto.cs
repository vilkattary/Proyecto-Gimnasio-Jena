using System;

namespace GimnasioJena.Abstracciones.Modelos.Clases
{
    public class ClaseDisponibleDto
    {
        public int idClaseProgramada { get; set; }
        public string nombreClase { get; set; }
        public string nombreEntrenador { get; set; }
        public DateTime fechaClase { get; set; }
        public TimeSpan horaInicio { get; set; }
        public TimeSpan horaFin { get; set; }
        public int cuposDisponibles { get; set; }
        public string ubicacion { get; set; }
    }
}