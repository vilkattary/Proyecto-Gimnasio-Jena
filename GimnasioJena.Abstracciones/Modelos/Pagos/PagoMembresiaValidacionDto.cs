using System;

namespace GimnasioJena.Abstracciones.Modelos.Pagos
{
    public class PagoMembresiaValidacionDto
    {
        public int idMembresiaCliente { get; set; }
        public int idPlanMembresia { get; set; }
        public int idEstadoMembresia { get; set; }

        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }

        public int? cantidadClasesPlan { get; set; }
        public int duracionDiasPlan { get; set; }
        public decimal precioPlan { get; set; }
    }
}