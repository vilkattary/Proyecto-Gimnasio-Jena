using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.Modelos.Progreso
{
    public class ResumenEvolucionDto
    {
        public int AsistenciaUltimos30Dias { get; set; }
        public List<ProgresoDiaDto> HistorialProgreso { get; set; } = new List<ProgresoDiaDto>();
        public List<MedicionDinamicaDto> HistorialMediciones { get; set; } = new List<MedicionDinamicaDto>();
        public MetricasEvolucionAvanzadaDto MetricasAvanzadas { get; set; } = new MetricasEvolucionAvanzadaDto();
    }
}
