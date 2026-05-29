using System;

namespace GimnasioJena.Abstracciones.Modelos.Asistencias
{
    public class AsistenciaListadoDto
    {
        public int idAsistencia { get; set; }
        public string nombreCliente { get; set; }
        public string nombreClase { get; set; }
        public DateTime fechaClase { get; set; }
        public TimeSpan horaInicio { get; set; }
        public bool asistio { get; set; }
        public DateTime fechaRegistro { get; set; }
        public string recepcionista { get; set; }
    }
}