using System;

namespace GimnasioJena.Abstracciones.Modelos.Reportes
{
    public class ReporteAsistenciaDto
    {
        public string nombreCliente { get; set; }
        public string nombreClase { get; set; }
        public string nombreEntrenador { get; set; }
        public DateTime fechaClase { get; set; }
        public bool asistio { get; set; }
    }
}