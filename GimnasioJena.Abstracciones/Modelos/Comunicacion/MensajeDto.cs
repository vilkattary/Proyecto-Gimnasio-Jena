using System;

namespace GimnasioJena.Abstracciones.Modelos.Comunicacion
{
    public class MensajeDto
    {
        public int idMensaje { get; set; }
        public int? idUsuario { get; set; }
        public string nombre { get; set; }
        public string correo { get; set; }
        public string telefono { get; set; }
        public string asunto { get; set; }
        public string mensaje { get; set; }
        public string respuesta { get; set; }
        public DateTime fechaEnvio { get; set; }
        public DateTime? fechaRespuesta { get; set; }
        public string estado { get; set; }
    }
}