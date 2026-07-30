using System.ComponentModel.DataAnnotations;

namespace GimnasioJena.Abstracciones.Modelos.Membresias
{
    public class EditarPrecioPlanDto
    {
        public int idPlanMembresia { get; set; }

        public string nombrePlan { get; set; }

        [Required(
            ErrorMessage = "El precio del plan es obligatorio.")]
        [Range(
            1,
            9999999,
            ErrorMessage = "El precio debe ser mayor que cero.")]
        public decimal precio { get; set; }
    }
}