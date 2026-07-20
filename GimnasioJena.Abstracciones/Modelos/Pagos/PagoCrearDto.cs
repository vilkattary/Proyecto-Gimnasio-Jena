using System;
using System.ComponentModel.DataAnnotations;

namespace GimnasioJena.Abstracciones.Modelos.Pagos
{
    public class PagoCrearDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una membresía.")]
        public int idMembresiaCliente { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un método de pago.")]
        public int idMetodoPago { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un estado.")]
        public int idEstadoPago { get; set; }

        [Required]
        [Range(typeof(decimal), "0.01", "999999999.99",
            ErrorMessage = "El monto debe ser mayor que cero.")]
        public decimal monto { get; set; }

        public DateTime fechaPago { get; set; }

        [StringLength(100)]
        public string referenciaPago { get; set; }

        [StringLength(300)]
        public string observaciones { get; set; }
    }
}