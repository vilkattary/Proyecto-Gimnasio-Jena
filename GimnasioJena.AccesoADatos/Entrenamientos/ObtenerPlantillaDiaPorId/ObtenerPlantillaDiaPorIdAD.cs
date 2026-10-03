using GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.ObtenerPlantillaDiaPorId;
using GimnasioJena.Abstracciones.Modelos.Entrenamientos;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Entrenamientos.ObtenerPlantillaDiaPorId
{
    public class ObtenerPlantillaDiaPorIdAD : IObtenerPlantillaDiaPorIdAD
    {
        public PlantillaDiaDto ObtenerPlantillaDiaPorId(int idPlantillaDia)
        {
            using (Contexto contexto = new Contexto())
            {
                return contexto.PlantillasDiaEntrenamiento
                    .Where(p => p.idPlantillaDia == idPlantillaDia)
                    .Select(p => new PlantillaDiaDto
                    {
                        idPlantillaDia = p.idPlantillaDia,
                        idMesociclo = p.idMesociclo,
                        DiaSemana = p.DiaSemana,
                        AreaEnfoque = p.AreaEnfoque,
                        OrdenIndice = p.OrdenIndice,
                        Ejercicios = p.Ejercicios
                            .OrderBy(e => e.OrdenIndice)
                            .Select(e => new EjercicioDto
                            {
                                idEjercicio = e.idEjercicio,
                                NombreEjercicio = e.NombreEjercicio,
                                TipoMetrica = e.TipoMetrica,
                                OrdenIndice = e.OrdenIndice,
                                Notas = e.Notas,
                                Progresiones = e.Progresiones
                                    .OrderBy(pr => pr.NumeroSemana)
                                    .Select(pr => new ProgresionSemanaDto
                                    {
                                        idProgresion = pr.idProgresion,
                                        NumeroSemana = pr.NumeroSemana,
                                        Series = pr.Series,
                                        RepeticionesObjetivo = pr.RepeticionesObjetivo,
                                        DuracionSegundos = pr.DuracionSegundos,
                                        CargaOrpeSugerido = pr.CargaOrpeSugerido
                                    })
                                    .ToList()
                            })
                            .ToList()
                    })
                    .FirstOrDefault();
            }
        }
    }
}
