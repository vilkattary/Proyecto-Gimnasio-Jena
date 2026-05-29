using System;

namespace GimnasioJena.Abstracciones.Modelos.Reportes
{
    public class ReporteIngresosDto
    {
        public DateTime fechaPago { get; set; }
        public string nombreCliente { get; set; }
        public string nombrePlan { get; set; }
        public string metodoPago { get; set; }
        public string estadoPago { get; set; }
        public decimal monto { get; set; }
    }
}