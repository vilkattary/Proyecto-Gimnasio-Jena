namespace GimnasioJena.Abstracciones.Modelos.Reservas
{
    public class ReservaClaseDto
    {
        public int idReserva { get; set; }
        public int idClaseProgramada { get; set; }
        public string nombreCliente { get; set; }
        public string correoCliente { get; set; }
        public string telefonoCliente { get; set; }
        public int idEstadoReserva { get; set; } 
        public string estadoReserva { get; set; }
    }
}