using System;

namespace GimnasioJena.Abstracciones.Modelos.Progreso
{
    public class PuntoCardioDto
    {
        public DateTime Fecha { get; set; }
        public decimal? Distancia { get; set; }
        public decimal? RitmoMedio { get; set; }
        public int? Pulsaciones { get; set; }
    }
}
