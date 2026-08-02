namespace GimnasioJena.Abstracciones.Modelos.Membresias
{
    public class PlanMembresiaListadoDto
    {
        public int idPlanMembresia { get; set; }

        public string nombrePlan { get; set; }

        public int? cantidadClases { get; set; }

        public int duracionDias { get; set; }

        public decimal precio { get; set; }

        public bool estado { get; set; }
    }
}