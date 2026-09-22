namespace GimnasioJena.Abstracciones.Modelos.Progreso
{
    public class RegistroProgresoAvanzadoDto
    {
        public int IdEntrenamiento { get; set; }
        public int idUsuario { get; set; }
        public decimal? PesoAlcanzado { get; set; }
        public int? Repeticiones { get; set; }
        public int? Series { get; set; }
        public int? RIR { get; set; }
        public decimal? DistanciaKm { get; set; }
        public decimal? TiempoMinutos { get; set; }
        public int? Pulsaciones { get; set; }
        public int? NivelEnergia { get; set; }
        public string Notas { get; set; }
    }
}
