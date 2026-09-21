using System;

namespace GimnasioJena.Abstracciones.Modelos.Progreso
{
    public class MedicionDinamicaDto
    {
        public int IdCampo { get; set; }
        public string NombreCampo { get; set; }
        public decimal Valor { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
