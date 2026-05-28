using System;

namespace GimnasioJena.Abstracciones.Modelos.Reservas
{
    public class ReservaDto
    {
        public int idReserva { get; set; }
        public int idUsuario { get; set; }
        public int idClaseProgramada { get; set; }
        public int idEstadoReserva { get; set; }
        public DateTime fechaReserva { get; set; }
        public string observaciones { get; set; }
    }
}