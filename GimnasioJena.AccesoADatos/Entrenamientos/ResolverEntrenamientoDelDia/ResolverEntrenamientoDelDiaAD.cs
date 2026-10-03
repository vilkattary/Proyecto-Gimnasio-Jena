using GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.ResolverEntrenamientoDelDia;
using GimnasioJena.Abstracciones.Modelos.Entrenamientos;
using GimnasioJena.AccesoADatos.Entidades.Entrenamientos;
using System;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Entrenamientos.ResolverEntrenamientoDelDia
{
    public class ResolverEntrenamientoDelDiaAD : IResolverEntrenamientoDelDiaAD
    {
        public PlantillaDiaDto ResolverEntrenamientoDelDia(
            DateTime fechaSesion,
            byte diaSemana,
            int? idPlantillaDiaForzada
        )
        {
            using (Contexto contexto = new Contexto())
            {
                DateTime fecha = fechaSesion.Date;

                MesocicloEntidad mesociclo = contexto.Mesociclos
                    .Where(m => m.EsActivo
                        && fecha >= m.FechaInicio
                        && fecha <= m.FechaFin)
                    .OrderByDescending(m => m.FechaInicio)
                    .FirstOrDefault();

                int numeroSemana = 1;

                if (mesociclo != null)
                {
                    int dias = (fecha - mesociclo.FechaInicio.Date).Days;
                    numeroSemana = (dias / 7) + 1;

                    if (numeroSemana < 1)
                    {
                        numeroSemana = 1;
                    }
                    else if (numeroSemana > 4)
                    {
                        numeroSemana = 4;
                    }
                }

                PlantillaDiaEntrenamientoEntidad plantilla;

                if (idPlantillaDiaForzada.HasValue && idPlantillaDiaForzada.Value > 0)
                {
                    plantilla = contexto.PlantillasDiaEntrenamiento
                        .FirstOrDefault(p => p.idPlantillaDia == idPlantillaDiaForzada.Value);
                }
                else if (mesociclo != null)
                {
                    plantilla = contexto.PlantillasDiaEntrenamiento
                        .Where(p => p.idMesociclo == mesociclo.idMesociclo
                            && p.DiaSemana == diaSemana)
                        .OrderBy(p => p.OrdenIndice)
                        .FirstOrDefault();
                }
                else
                {
                    plantilla = null;
                }

                if (plantilla == null)
                {
                    return null;
                }

                return contexto.PlantillasDiaEntrenamiento
                    .Where(p => p.idPlantillaDia == plantilla.idPlantillaDia)
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
                                    .Where(pr => pr.NumeroSemana == numeroSemana)
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
