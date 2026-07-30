using System;

namespace GimnasioJena.Abstracciones.Modelos.Membresias
{
    public class OperacionMembresiaDto
    {
        public int idUsuario { get; set; }

        public int idPlan { get; set; }

        public string nombrePlan { get; set; }

        public decimal precio { get; set; }

        public int duracionDias { get; set; }

        public string tipoOperacion { get; set; }

        public DateTime fechaInicioPropuesta { get; set; }

        public DateTime fechaFinPropuesta { get; set; }

        public int? clasesAsignadas { get; set; }

        public string descripcionPlan { get; set; }
    }
}