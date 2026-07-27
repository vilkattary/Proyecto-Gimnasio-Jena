using System;

namespace GimnasioJena.Abstracciones.Modelos.Pagos
{
    public class PagoDetalleDto
    {
        public int idPago { get; set; }
        public int idMembresiaCliente { get; set; }
        public string nombreCliente { get; set; }
        public string nombrePlan { get; set; }
        public string metodoPago { get; set; }
        public string estadoPago { get; set; }
        public decimal monto { get; set; }
        public DateTime fechaPago { get; set; }
        public string referenciaPago { get; set; }
        public string observaciones { get; set; }
    }
}