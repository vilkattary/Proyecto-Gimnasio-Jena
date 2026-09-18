using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.Modelos.Entrenamientos
{
    public class EjercicioDto
    {
        public int idEjercicio { get; set; }
        public string NombreEjercicio { get; set; }
        public int TipoMetrica { get; set; }
        public int OrdenIndice { get; set; }
        public string Notas { get; set; }

        public List<ProgresionSemanaDto> Progresiones { get; set; }
            = new List<ProgresionSemanaDto>();
    }
}
