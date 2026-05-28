using System;

namespace GimnasioJena.Abstracciones.Modelos.Reportes
{
    public class ReporteReservaDto
    {
        public string nombreCliente { get; set; }
        public string nombreClase { get; set; }
        public string estadoReserva { get; set; }
        public DateTime fechaClase { get; set; }
        public TimeSpan horaInicio { get; set; }
        public DateTime fechaReserva { get; set; }
    }
}