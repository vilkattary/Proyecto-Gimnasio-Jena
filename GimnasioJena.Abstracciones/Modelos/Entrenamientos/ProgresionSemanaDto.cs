namespace GimnasioJena.Abstracciones.Modelos.Entrenamientos
{
    public class ProgresionSemanaDto
    {
        public int idProgresion { get; set; }
        public int NumeroSemana { get; set; }
        public int? Series { get; set; }
        public string RepeticionesObjetivo { get; set; }
        public int? DuracionSegundos { get; set; }
        public string CargaOrpeSugerido { get; set; }
    }
}
