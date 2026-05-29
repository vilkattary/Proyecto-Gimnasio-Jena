using System;

namespace GimnasioJena.Abstracciones.Modelos.Comunicacion
{
    public class MensajeUsuarioDto
    {
        public int idMensaje { get; set; }
        public string asunto { get; set; }
        public string mensaje { get; set; }
        public string respuesta { get; set; }
        public DateTime fechaEnvio { get; set; }
        public DateTime? fechaRespuesta { get; set; }
        public string estado { get; set; }
    }
}