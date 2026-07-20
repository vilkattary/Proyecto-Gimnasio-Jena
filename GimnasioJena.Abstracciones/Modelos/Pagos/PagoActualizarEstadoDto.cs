using System.ComponentModel.DataAnnotations;

namespace GimnasioJena.Abstracciones.Modelos.Pagos
{
    public class PagoActualizarEstadoDto
    {
        public int idPago { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un estado.")]
        public int idEstadoPago { get; set; }

        [StringLength(300)]
        public string observaciones { get; set; }
    }
}