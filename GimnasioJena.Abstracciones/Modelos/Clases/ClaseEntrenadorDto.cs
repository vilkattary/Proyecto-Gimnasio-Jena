using System;

namespace GimnasioJena.Abstracciones.Modelos.Clases
{
    public class ClaseEntrenadorDto
    {
        public int idClaseProgramada { get; set; }
        public int idUsuarioEntrenador { get; set; }
        public string nombreClase { get; set; }
        public DateTime fechaClase { get; set; }
        public TimeSpan horaInicio { get; set; }
        public TimeSpan horaFin { get; set; }
        public int cupoMaximo { get; set; }
        public int inscritos { get; set; }
        public string ubicacion { get; set; }
    }
}