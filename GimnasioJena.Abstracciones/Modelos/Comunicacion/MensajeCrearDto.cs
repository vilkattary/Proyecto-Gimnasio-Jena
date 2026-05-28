namespace GimnasioJena.Abstracciones.Modelos.Comunicacion
{
    public class MensajeCrearDto
    {
        public int? idUsuario { get; set; }
        public string nombre { get; set; }
        public string correo { get; set; }
        public string telefono { get; set; }
        public string asunto { get; set; }
        public string mensaje { get; set; }
    }
}