namespace GimnasioJena.Abstracciones.Modelos.Progreso
{
    public class ProgresoFilaDto
    {
        public string NombreEjercicio { get; set; }
        public int SeriesPrescritas { get; set; }
        public int RepeticionesPrescritas { get; set; }
        public decimal? PesoLogrado { get; set; }
        public int? RepeticionesLogradas { get; set; }
        public int? SeriesLogradas { get; set; }
        public decimal? TiempoMinutos { get; set; }
        public string NotasCliente { get; set; }
    }
}
