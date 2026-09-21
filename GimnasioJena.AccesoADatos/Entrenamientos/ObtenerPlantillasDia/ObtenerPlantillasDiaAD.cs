using GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.ObtenerPlantillasDia;
using GimnasioJena.Abstracciones.Modelos.Entrenamientos;
using System.Collections.Generic;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Entrenamientos.ObtenerPlantillasDia
{
    public class ObtenerPlantillasDiaAD : IObtenerPlantillasDiaAD
    {
        public List<PlantillaDiaDto> ObtenerPlantillasDia(int? idMesociclo)
        {
            using (Contexto contexto = new Contexto())
            {
                var consulta = contexto.PlantillasDiaEntrenamiento.AsQueryable();

                if (idMesociclo.HasValue)
                {
                    consulta = consulta.Where(p => p.idMesociclo == idMesociclo.Value);
                }

                return consulta
                    .OrderBy(p => p.idMesociclo)
                    .ThenBy(p => p.OrdenIndice)
                    .Select(p => new PlantillaDiaDto
                    {
                        idPlantillaDia = p.idPlantillaDia,
                        idMesociclo = p.idMesociclo,
                        DiaSemana = p.DiaSemana,
                        AreaEnfoque = p.AreaEnfoque,
                        OrdenIndice = p.OrdenIndice
                    })
                    .ToList();
            }
        }
    }
}
