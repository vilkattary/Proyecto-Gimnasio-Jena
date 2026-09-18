using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.Modelos.Progreso
{
    public class GuardarRutinaManualDto
    {
        public int? IdEntrenamientoClonado { get; set; }
        public string DiaSemana { get; set; }
        public string NombreRutina { get; set; }
        public string TextoLibreMarkdown { get; set; }
        public List<RutinaBloqueDto> Bloques { get; set; } = new List<RutinaBloqueDto>();
        public bool EsPlantilla { get; set; }
    }
}
