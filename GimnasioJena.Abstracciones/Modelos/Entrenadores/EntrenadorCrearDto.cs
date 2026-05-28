using System;

namespace GimnasioJena.Abstracciones.Modelos.Entrenadores
{
    public class EntrenadorCrearDto
    {
        public int idUsuario { get; set; }
        public string especialidad { get; set; }
        public string descripcion { get; set; }
        public DateTime fechaContratacion { get; set; }
    }
}