using System;

namespace GimnasioJena.Abstracciones.Modelos.Asistencias
{
    public class AsistenciaValidacionDto
    {
        public int idReserva { get; set; }

        public int idClaseProgramada { get; set; }

        public int idEstadoReserva { get; set; }

        public int idEstadoClase { get; set; }

        public DateTime fechaClase { get; set; }

        public TimeSpan horaInicio { get; set; }
    }
}