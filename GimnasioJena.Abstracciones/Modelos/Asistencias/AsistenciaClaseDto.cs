namespace GimnasioJena.Abstracciones.Modelos.Asistencias
{
    public class AsistenciaClaseDto
    {
        public int idReserva { get; set; }
        public int? idAsistencia { get; set; }
        public string nombreCliente { get; set; }
        public string correoCliente { get; set; }
        public string telefonoCliente { get; set; }
        public bool? asistio { get; set; }
        public string observaciones { get; set; }
    }
}