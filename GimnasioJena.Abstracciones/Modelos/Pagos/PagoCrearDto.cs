using System;
using System.ComponentModel.DataAnnotations;

namespace GimnasioJena.Abstracciones.Modelos.Pagos
{
    public class PagoCrearDto
    {
        // "existente" o "nuevo"
        public string tipoPago { get; set; } = "existente";

        // Membresía existente.
        // Para clientes nuevos será 0 hasta crear la membresía.
        [Range(0, int.MaxValue)]
        public int idMembresiaCliente { get; set; }

        // Datos necesarios únicamente para el primer pago.
        [Range(0, int.MaxValue)]
        public int idUsuarioNuevo { get; set; }

        [Range(0, int.MaxValue)]
        public int idPlanNuevo { get; set; }

        [Required]
        [Range(1, int.MaxValue,
            ErrorMessage = "Debe seleccionar un método de pago.")]
        public int idMetodoPago { get; set; }

        [Required]
        [Range(1, int.MaxValue,
            ErrorMessage = "Debe seleccionar un estado.")]
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