using System;

namespace GimnasioJena.Abstracciones.Modelos.Reservas
{
    public class ReservaClienteDto
    {
        public int idReserva { get; set; }
        public int idUsuario { get; set; }
        public string nombreClase { get; set; }
        public string nombreEntrenador { get; set; }
        public string estadoReserva { get; set; }
        public DateTime fechaClase { get; set; }
        public TimeSpan horaInicio { get; set; }
        public TimeSpan horaFin { get; set; }
        public string ubicacion { get; set; }
    }
}