using System;

namespace GimnasioJena.Abstracciones.Modelos.Reportes
{
    public class ReporteMembresiaDto
    {
        public string nombreCliente { get; set; }
        public string nombrePlan { get; set; }
        public string estadoMembresia { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public int? clasesDisponibles { get; set; }
    }
}