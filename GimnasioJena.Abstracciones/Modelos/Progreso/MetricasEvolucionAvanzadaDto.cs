using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.Modelos.Progreso
{
    public class MetricasEvolucionAvanzadaDto
    {
        public List<PuntoVolumenFuerzaDto> HistoricoVolumen { get; set; } = new List<PuntoVolumenFuerzaDto>();
        public List<PuntoCardioDto> HistoricoCardio { get; set; } = new List<PuntoCardioDto>();
        public Dictionary<string, int> AsistenciaHeatmap { get; set; } = new Dictionary<string, int>();
        public List<MedicionDinamicaDto> HistoricoComposicionCorporal { get; set; } = new List<MedicionDinamicaDto>();
    }
}
