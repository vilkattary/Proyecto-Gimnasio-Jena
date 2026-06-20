using System;

namespace GimnasioJena.Abstracciones.Modelos.Reservas
{
    public class ReservaListadoDto
    {
        public int idReserva { get; set; }
        public int idClaseProgramada { get; set; }        
        public string nombreCliente { get; set; }
        public string nombreClase { get; set; }
        public string nombreEntrenador { get; set; }     
        public int idEstadoReserva { get; set; }        
        public string estadoReserva { get; set; }
        public DateTime fechaClase { get; set; }
        public TimeSpan horaInicio { get; set; }
        public TimeSpan horaFin { get; set; }
        public DateTime fechaReserva { get; set; }
        public string ubicacion { get; set; }
    }
}