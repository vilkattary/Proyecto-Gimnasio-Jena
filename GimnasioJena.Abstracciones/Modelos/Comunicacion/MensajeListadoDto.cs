using System;

namespace GimnasioJena.Abstracciones.Modelos.Comunicacion
{
    public class MensajeListadoDto
    {
        public int idMensaje { get; set; }
        public string nombre { get; set; }
        public string correo { get; set; }
        public string asunto { get; set; }
        public DateTime fechaEnvio { get; set; }
        public string estado { get; set; }
    }
}