using System;

namespace GimnasioJena.Abstracciones.Modelos.Entrenadores
{
    public class EntrenadorListadoDto
    {
        public int idEntrenador { get; set; }
        public int idUsuario { get; set; }
        public string nombreCompleto { get; set; }
        public string correo { get; set; }
        public string especialidad { get; set; }
        public DateTime fechaContratacion { get; set; }
        public bool estado { get; set; }
    }
}