namespace GimnasioJena.Abstracciones.Modelos.Reservas
{
    public class ReservaCrearDto
    {
        public int idUsuario { get; set; }
        public int idClaseProgramada { get; set; }
        public int idEstadoReserva { get; set; }
        public string observaciones { get; set; }
    }
}