using System.Collections.Generic;
using System;

namespace GimnasioJena.Abstracciones.Modelos.Reservas
{
    public class ReservaClaseValidacionDto
    {
        public int idClaseProgramada { get; set; }
        public int idEstadoClase { get; set; }
        public DateTime fechaClase { get; set; }
        public TimeSpan horaInicio { get; set; }
        public int cupoMaximo { get; set; }
    }
}