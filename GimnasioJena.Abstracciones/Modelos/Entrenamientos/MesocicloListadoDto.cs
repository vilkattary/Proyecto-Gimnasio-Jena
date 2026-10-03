using System;

namespace GimnasioJena.Abstracciones.Modelos.Entrenamientos
{
    public class MesocicloListadoDto
    {
        public int idMesociclo { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool EsActivo { get; set; }
        public int CantidadDias { get; set; }
    }
}
