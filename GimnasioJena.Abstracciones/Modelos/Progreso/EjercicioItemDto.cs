namespace GimnasioJena.Abstracciones.Modelos.Progreso
{
    public class EjercicioItemDto
    {
        public int Orden { get; set; }
        public string Nombre { get; set; }
        public int? SeriesPrescritas { get; set; }
        public int? RepeticionesPrescritas { get; set; }
        public string Prescripcion { get; set; }
        public string NotasCoach { get; set; }
    }
}
