using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GimnasioJena.AccesoADatos.Entidades.Pagos
{
    [Table("Pago")]
    public class PagoEntidad
    {
        [Key]
        public int idPago { get; set; }

        public int idMembresiaCliente { get; set; }

        public int idMetodoPago { get; set; }

        public int idEstadoPago { get; set; }

        public decimal monto { get; set; }

        public DateTime fechaPago { get; set; }

        [StringLength(100)]
        public string referenciaPago { get; set; }

        [StringLength(300)]
        public string observaciones { get; set; }
    }
}