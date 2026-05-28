using System;

namespace GimnasioJena.Abstracciones.Modelos.Membresias
{
    public class MembresiaCrearDto
    {
        public int idUsuario { get; set; }
        public int idPlanMembresia { get; set; }
        public int idEstadoMembresia { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public int? clasesDisponibles { get; set; }
        public string observaciones { get; set; }
    }
}