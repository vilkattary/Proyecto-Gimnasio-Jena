namespace GimnasioJena.Abstracciones.Modelos.Asistencias
{
    public class AsistenciaCrearDto
    {
        public int idReserva { get; set; }
        public int? idUsuarioRecepcionista { get; set; }
        public bool asistio { get; set; }
        public string observaciones { get; set; }
    }
}