using System;

namespace GimnasioJena.Abstracciones.Modelos.Clases
{
    public class HorarioItemDto
    {
        public string fechaClase         { get; set; }
        public string horaInicio         { get; set; }
        public string horaFin            { get; set; }
        public int    idUsuarioEntrenador { get; set; }
    }
}
