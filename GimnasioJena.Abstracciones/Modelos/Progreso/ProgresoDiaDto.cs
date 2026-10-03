using System;

namespace GimnasioJena.Abstracciones.Modelos.Progreso
{
    public class ProgresoDiaDto
    {
        public int idUsuario { get; set; }
        public int IdEntrenamiento { get; set; }
        public string NombreRutina { get; set; }
        public decimal PesoAlcanzado { get; set; }
        public int Repeticiones { get; set; }
        public string Notas { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
