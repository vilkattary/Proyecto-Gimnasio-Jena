using System;

namespace GimnasioJena.Abstracciones.Modelos.HorariosSemanales
{
    public class ResultadoGeneracionClasesDto
    {
        public bool fueExitosa { get; set; }

        public string mensaje { get; set; }

        public int clasesGeneradas { get; set; }

        public int clasesOmitidas { get; set; }

        public int horariosProcesados { get; set; }
        public DateTime fechaInicioGenerada { get; set; }

        public DateTime fechaFinGenerada { get; set; }
        public int diasProcesados { get; set; }
    }
}