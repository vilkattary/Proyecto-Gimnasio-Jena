using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.Modelos.Progreso
{
    public class CrearEntrenamientoClaseDto
    {
        public int? IdHorarioSemanal { get; set; }
        public int? IdClaseProgramada { get; set; }
        public string TipoAsignacion { get; set; }
        public string NombreRutina { get; set; }
        public string DiaSemana { get; set; }
        public List<EjercicioItemDto> Ejercicios { get; set; } = new List<EjercicioItemDto>();
    }
}
