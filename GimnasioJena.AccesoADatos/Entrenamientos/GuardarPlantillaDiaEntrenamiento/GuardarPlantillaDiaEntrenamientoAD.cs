using GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.GuardarPlantillaDiaEntrenamiento;
using GimnasioJena.Abstracciones.Modelos.Entrenamientos;
using GimnasioJena.AccesoADatos.Entidades.Entrenamientos;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Entrenamientos.GuardarPlantillaDiaEntrenamiento
{
    public class GuardarPlantillaDiaEntrenamientoAD
        : IGuardarPlantillaDiaEntrenamientoAD
    {
        private static readonly string[] NombresDia =
        {
            "", "Lunes", "Martes", "Miércoles", "Jueves",
            "Viernes", "Sábado", "Domingo"
        };

        public ResultadoGuardarPlantillaDto GuardarPlantillaDiaEntrenamiento(
            GuardarPlantillaDiaDto modelo
        )
        {
            using (Contexto contexto = new Contexto())
            using (var transaccion = contexto.Database.BeginTransaction())
            {
                try
                {
                    PlantillaDiaEntrenamientoEntidad plantilla;
                    List<EjercicioEntrenamientoEntidad> ejerciciosPrevios =
                        new List<EjercicioEntrenamientoEntidad>();

                    if (modelo.idPlantillaDia > 0)
                    {
                        plantilla = contexto.PlantillasDiaEntrenamiento
                            .FirstOrDefault(p => p.idPlantillaDia == modelo.idPlantillaDia);

                        if (plantilla == null)
                        {
                            transaccion.Rollback();
                            return CrearError("No existe la plantilla de día indicada.");
                        }

                        ejerciciosPrevios = contexto.EjerciciosEntrenamiento
                            .Where(e => e.idPlantillaDia == plantilla.idPlantillaDia)
                            .OrderBy(e => e.OrdenIndice)
                            .ThenBy(e => e.idEjercicio)
                            .ToList();

                        plantilla.DiaSemana = modelo.DiaSemana;
                        plantilla.AreaEnfoque = modelo.AreaEnfoque?.Trim();
                        plantilla.OrdenIndice = modelo.OrdenIndice;
                    }
                    else
                    {
                        if (!contexto.Mesociclos.Any(m => m.idMesociclo == modelo.idMesociclo))
                        {
                            transaccion.Rollback();
                            return CrearError(
                                "No existe un mesociclo válido al cual asociar la rutina. "
                                + "Cree un mesociclo antes de configurar la rutina.");
                        }

                        plantilla = new PlantillaDiaEntrenamientoEntidad
                        {
                            idMesociclo = modelo.idMesociclo,
                            DiaSemana = modelo.DiaSemana,
                            AreaEnfoque = modelo.AreaEnfoque?.Trim(),
                            OrdenIndice = modelo.OrdenIndice
                        };

                        contexto.PlantillasDiaEntrenamiento.Add(plantilla);
                        contexto.SaveChanges();
                    }

                    int ordenEjercicio = 0;

                    foreach (var ejercicioDto in modelo.Ejercicios)
                    {
                        // Se reutilizan los ejercicios existentes para no romper las
                        // referencias históricas (UserSetLog) que apuntan a idEjercicio.
                        EjercicioEntrenamientoEntidad ejercicio =
                            ordenEjercicio < ejerciciosPrevios.Count
                                ? ejerciciosPrevios[ordenEjercicio]
                                : null;

                        if (ejercicio == null)
                        {
                            ejercicio = new EjercicioEntrenamientoEntidad
                            {
                                idPlantillaDia = plantilla.idPlantillaDia
                            };

                            contexto.EjerciciosEntrenamiento.Add(ejercicio);
                        }
                        else
                        {
                            var progresionesPrevias = contexto.ProgresionesEjercicio
                                .Where(pr => pr.idEjercicio == ejercicio.idEjercicio)
                                .ToList();

                            contexto.ProgresionesEjercicio.RemoveRange(progresionesPrevias);
                        }

                        ejercicio.NombreEjercicio = ejercicioDto.NombreEjercicio?.Trim();
                        ejercicio.TipoMetrica = ejercicioDto.TipoMetrica;
                        ejercicio.OrdenIndice = ordenEjercicio;
                        ejercicio.Notas = ejercicioDto.Notas?.Trim();

                        contexto.SaveChanges();

                        foreach (var progresionDto in ejercicioDto.Progresiones)
                        {
                            ProgresionEjercicioEntidad progresion =
                                new ProgresionEjercicioEntidad
                                {
                                    idEjercicio = ejercicio.idEjercicio,
                                    NumeroSemana = progresionDto.NumeroSemana,
                                    Series = progresionDto.Series,
                                    RepeticionesObjetivo = progresionDto.RepeticionesObjetivo?.Trim(),
                                    DuracionSegundos = progresionDto.DuracionSegundos,
                                    CargaOrpeSugerido = progresionDto.CargaOrpeSugerido?.Trim()
                                };

                            contexto.ProgresionesEjercicio.Add(progresion);
                        }

                        contexto.SaveChanges();
                        ordenEjercicio++;
                    }

                    // Ejercicios sobrantes de la versión anterior de la rutina.
                    for (int i = ordenEjercicio; i < ejerciciosPrevios.Count; i++)
                    {
                        EjercicioEntrenamientoEntidad sobrante = ejerciciosPrevios[i];

                        if (contexto.UserSetLogs.Any(l => l.WorkoutExerciseId == sobrante.idEjercicio))
                        {
                            transaccion.Rollback();
                            return CrearError(
                                $"No se puede eliminar el ejercicio '{sobrante.NombreEjercicio}' "
                                + "porque ya tiene registros de entrenamiento asociados.");
                        }

                        var progresionesSobrantes = contexto.ProgresionesEjercicio
                            .Where(pr => pr.idEjercicio == sobrante.idEjercicio)
                            .ToList();

                        contexto.ProgresionesEjercicio.RemoveRange(progresionesSobrantes);
                        contexto.EjerciciosEntrenamiento.Remove(sobrante);
                    }

                    contexto.SaveChanges();
                    transaccion.Commit();

                    return new ResultadoGuardarPlantillaDto
                    {
                        Exito = true,
                        Mensaje = "Rutina guardada correctamente.",
                        idPlantillaDia = plantilla.idPlantillaDia,
                        NombrePlantilla = ConstruirNombre(plantilla.DiaSemana, plantilla.AreaEnfoque)
                    };
                }
                catch (DbEntityValidationException ex)
                {
                    transaccion.Rollback();

                    string detalle = string.Join(" ", ex.EntityValidationErrors
                        .SelectMany(e => e.ValidationErrors)
                        .Select(e => e.PropertyName + ": " + e.ErrorMessage));

                    return CrearError("No se pudo guardar la rutina. " + detalle);
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    return CrearError("No se pudo guardar la rutina: " + ObtenerMensajeDetallado(ex));
                }
            }
        }

        private static string ObtenerMensajeDetallado(Exception ex)
        {
            Exception actual = ex;

            while (actual.InnerException != null)
            {
                actual = actual.InnerException;
            }

            return actual.Message;
        }

        private static string ConstruirNombre(byte diaSemana, string areaEnfoque)
        {
            string dia = diaSemana >= 1 && diaSemana <= 7
                ? NombresDia[diaSemana]
                : "Día " + diaSemana;

            return string.IsNullOrWhiteSpace(areaEnfoque)
                ? dia
                : dia + " - " + areaEnfoque;
        }

        private static ResultadoGuardarPlantillaDto CrearError(string mensaje)
        {
            return new ResultadoGuardarPlantillaDto
            {
                Exito = false,
                Mensaje = mensaje,
                idPlantillaDia = 0,
                NombrePlantilla = null
            };
        }
    }
}
