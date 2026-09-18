using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.Modelos.Progreso
{
    public class RegistrarProgresoClaseDto
    {
        public int IdClaseProgramada { get; set; }
        public int idUsuario { get; set; }
        public List<ProgresoFilaDto> FilasEjercicios { get; set; } = new List<ProgresoFilaDto>();
    }
}
