using System;

namespace GimnasioJena.Abstracciones.Modelos.Asistencias
{
    public class AsistenciaDto
    {
        public int idAsistencia { get; set; }
        public int idReserva { get; set; }
        public int? idUsuarioRecepcionista { get; set; }
        public DateTime fechaRegistro { get; set; }
        public bool asistio { get; set; }
        public string observaciones { get; set; }
    }
}