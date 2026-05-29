using System;

namespace GimnasioJena.Abstracciones.Modelos.Clases
{
    public class ClaseEditarDto
    {
        public int idClaseProgramada { get; set; }
        public int idTipoClase { get; set; }
        public int idUsuarioEntrenador { get; set; }
        public int idEstadoClase { get; set; }
        public DateTime fechaClase { get; set; }
        public TimeSpan horaInicio { get; set; }
        public TimeSpan horaFin { get; set; }
        public int cupoMaximo { get; set; }
        public string ubicacion { get; set; }
        public string observaciones { get; set; }
    }
}