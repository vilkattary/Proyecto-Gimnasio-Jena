using System;

namespace GimnasioJena.Abstracciones.Modelos.Asistencias
{
    public class AsistenciaClienteDto
    {
        public int idAsistencia { get; set; }
        public string nombreClase { get; set; }
        public DateTime fechaClase { get; set; }
        public bool asistio { get; set; }
        public string observaciones { get; set; }
    }
}