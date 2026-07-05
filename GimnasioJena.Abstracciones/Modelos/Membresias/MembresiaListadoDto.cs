using System;

namespace GimnasioJena.Abstracciones.Modelos.Membresias
{
    public class MembresiaListadoDto
    {
        public int idMembresiaCliente { get; set; }
        public int idUsuario { get; set; }
        public string nombreCliente { get; set; }
        public string nombrePlan { get; set; }
        public string estadoMembresia { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public int? clasesDisponibles { get; set; }
        public decimal precio { get; set; }
    }
}