namespace GimnasioJena.Abstracciones.Modelos.Membresias
{
    public class PlanMembresiaDatosDto
    {
        public int idPlanMembresia { get; set; }

        public int? cantidadClases { get; set; }

        public int duracionDias { get; set; }

        public decimal precio { get; set; }

        public bool estado { get; set; }
    }
}