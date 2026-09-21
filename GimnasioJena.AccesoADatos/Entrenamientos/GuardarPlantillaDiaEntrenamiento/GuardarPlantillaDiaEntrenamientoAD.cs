using GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.GuardarPlantillaDiaEntrenamiento;
using GimnasioJena.Abstracciones.Modelos.Entrenamientos;
using GimnasioJena.AccesoADatos.Entidades.Entrenamientos;
using System;
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

                    if (modelo.idPlantillaDia > 0)
                    {
                        plantilla = contexto.PlantillasDiaEntrenamiento
                            .FirstOrDefault(p => p.idPlantillaDia == modelo.idPlantillaDia);

                        if (plantilla == null)
                        {
                            transaccion.Rollback();
                            return CrearError("No existe la plantilla de día indicada.");
                        }

                        var ejerciciosPrevios = contexto.EjerciciosEntrenamiento
                            .Where(e => e.idPlantillaDia == plantilla.idPlantillaDia)
                            .ToList();

                        foreach (var ejercicioPrevio in ejerciciosPrevios)
                        {
                            var progresionesPrevias = contexto.ProgresionesEjercicio
                                .Where(pr => pr.idEjercicio == ejercicioPrevio.idEjercicio)
                                .ToList();

                            contexto.ProgresionesEjercicio.RemoveRange(progresionesPrevias);
                        }

                        contexto.EjerciciosEntrenamiento.RemoveRange(ejerciciosPrevios);
                        contexto.SaveChanges();

                        plantilla.DiaSemana = modelo.DiaSemana;
                        plantilla.AreaEnfoque = modelo.AreaEnfoque?.Trim();
                        plantilla.OrdenIndice = modelo.OrdenIndice;
                    }
                    else
                    {
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
                        EjercicioEntrenamientoEntidad ejercicio =
                            new EjercicioEntrenamientoEntidad
                            {
                                idPlantillaDia = plantilla.idPlantillaDia,
                                NombreEjercicio = ejercicioDto.NombreEjercicio?.Trim(),
                                TipoMetrica = ejercicioDto.TipoMetrica,
                                OrdenIndice = ordenEjercicio++,
                                Notas = ejercicioDto.Notas?.Trim()
                            };

                        contexto.EjerciciosEntrenamiento.Add(ejercicio);
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
                    }

                    transaccion.Commit();

                    return new ResultadoGuardarPlantillaDto
                    {
                        Exito = true,
                        Mensaje = "Rutina guardada correctamente.",
                        idPlantillaDia = plantilla.idPlantillaDia,
                        NombrePlantilla = ConstruirNombre(plantilla.DiaSemana, plantilla.AreaEnfoque)
                    };
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    return CrearError("No se pudo guardar la rutina: " + ex.Message);
                }
            }
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
