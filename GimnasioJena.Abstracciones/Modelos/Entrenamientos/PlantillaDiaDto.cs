using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.Modelos.Entrenamientos
{
    public class PlantillaDiaDto
    {
        public int idPlantillaDia { get; set; }
        public int idMesociclo { get; set; }
        public byte DiaSemana { get; set; }
        public string AreaEnfoque { get; set; }
        public int OrdenIndice { get; set; }

        public List<EjercicioDto> Ejercicios { get; set; }
            = new List<EjercicioDto>();
    }
}
