using System;

namespace GimnasioJena.Abstracciones.Modelos.Clases
{
    public class ClaseListadoDto
    {
        public int idClaseProgramada { get; set; }
        public string nombreClase { get; set; }
        public string nombreEntrenador { get; set; }
        public string estadoClase { get; set; }
        public DateTime fechaClase { get; set; }
        public TimeSpan horaInicio { get; set; }
        public TimeSpan horaFin { get; set; }
        public int cupoMaximo { get; set; }
        public int cuposReservados { get; set; }
        public int cuposDisponibles { get; set; }
        public string ubicacion { get; set; }
    }
}