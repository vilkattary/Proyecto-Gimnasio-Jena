using System;

namespace GimnasioJena.Abstracciones.Modelos.Membresias
{
    public class MembresiaRenovarDto
    {
        public int idMembresiaCliente { get; set; }
        public DateTime nuevaFechaInicio { get; set; }
        public DateTime nuevaFechaFin { get; set; }
        public int? nuevasClasesDisponibles { get; set; }
        public string observaciones { get; set; }
    }
}