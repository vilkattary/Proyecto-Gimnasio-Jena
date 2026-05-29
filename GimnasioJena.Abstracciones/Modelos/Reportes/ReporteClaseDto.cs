using System;

namespace GimnasioJena.Abstracciones.Modelos.Reportes
{
    public class ReporteClaseDto
    {
        public string nombreClase { get; set; }
        public string nombreEntrenador { get; set; }
        public DateTime fechaClase { get; set; }
        public TimeSpan horaInicio { get; set; }
        public int cupoMaximo { get; set; }
        public int cuposReservados { get; set; }
        public int cuposDisponibles { get; set; }
    }
}