using GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.ObtenerMesocicloPorId;
using GimnasioJena.Abstracciones.Modelos.Entrenamientos;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Entrenamientos.ObtenerMesocicloPorId
{
    public class ObtenerMesocicloPorIdAD : IObtenerMesocicloPorIdAD
    {
        public MesocicloDto ObtenerMesocicloPorId(int idMesociclo)
        {
            using (Contexto contexto = new Contexto())
            {
                MesocicloDto mesociclo = contexto.Mesociclos
                    .Where(m => m.idMesociclo == idMesociclo)
                    .Select(m => new MesocicloDto
                    {
                        idMesociclo = m.idMesociclo,
                        Nombre = m.Nombre,
                        FechaInicio = m.FechaInicio,
                        FechaFin = m.FechaFin,
                        EsActivo = m.EsActivo,
                        PlantillasDia = m.PlantillasDia
                            .OrderBy(p => p.OrdenIndice)
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
                            .ToList()
                    })
                    .FirstOrDefault();

                return mesociclo;
            }
        }
    }
}
