using System.ComponentModel.DataAnnotations;

namespace GimnasioJena.Abstracciones.Modelos.Membresias
{
    public class RegistrarPlanMembresiaDto
    {
        [Required(
            ErrorMessage = "El nombre del plan es obligatorio.")]
        [StringLength(
            100,
            ErrorMessage = "El nombre del plan no puede exceder los 100 caracteres.")]
        public string nombrePlan { get; set; }

        [Required(
            ErrorMessage = "El precio del plan es obligatorio.")]
        [Range(
            1,
            9999999,
            ErrorMessage = "El precio debe ser mayor que cero.")]
        public decimal precio { get; set; }

        [Required(
            ErrorMessage = "La duración en días es obligatoria.")]
        [Range(
            1,
            3650,
            ErrorMessage = "La duración debe ser mayor que cero.")]
        public int duracionDias { get; set; }

        [Range(
            0,
            9999,
            ErrorMessage = "Las clases incluidas no pueden ser negativas.")]
        public int? cantidadClases { get; set; }
    }
}
