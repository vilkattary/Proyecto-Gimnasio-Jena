using GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.ObtenerWorkoutClase;
using GimnasioJena.Abstracciones.Modelos.Entrenamientos;
using System;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Entrenamientos.ObtenerWorkoutClase
{
    public class ObtenerWorkoutClaseAD : IObtenerWorkoutClaseAD
    {
        public ClaseWorkoutClienteDto ObtenerWorkoutClase(int classId, int userId)
        {
            using (Contexto contexto = new Contexto())
            {
                var clase = contexto.Clases
                    .FirstOrDefault(c => c.idClaseProgramada == classId);

                if (clase == null)
                {
                    return null;
                }

                // La plantilla puede estar asignada directamente en la clase o, si esta
                // se generó desde un horario recurrente, heredarse del HorarioSemanal.
                int? idPlantillaResuelta = (clase.idPlantillaDia != null && clase.idPlantillaDia > 0)
                    ? clase.idPlantillaDia
                    : null;

                if (idPlantillaResuelta == null && clase.idHorario != null)
                {
                    idPlantillaResuelta = contexto.HorariosSemanales
                        .Where(h => h.idHorario == clase.idHorario)
                        .Select(h => h.idPlantillaDia)
                        .FirstOrDefault();
                }

                if (idPlantillaResuelta == null || idPlantillaResuelta <= 0)
                {
                    return null;
                }

                int idPlantilla = idPlantillaResuelta.Value;

                var plantilla = contexto.PlantillasDiaEntrenamiento
                    .FirstOrDefault(p => p.idPlantillaDia == idPlantilla);

                if (plantilla == null)
                {
                    return null;
                }

                int numeroSemana = CalcularSemanaVigente(contexto, plantilla.idMesociclo);

                var resultado = new ClaseWorkoutClienteDto
                {
                    ClassInstanceId = classId,
                    WorkoutDayTemplateId = idPlantilla,
                    AreaEnfoque = plantilla.AreaEnfoque,
                    NombreClase = plantilla.AreaEnfoque,
                    NumeroSemana = numeroSemana
                };

                var ejercicios = contexto.EjerciciosEntrenamiento
                    .Where(e => e.idPlantillaDia == idPlantilla)
                    .OrderBy(e => e.OrdenIndice)
                    .ToList();

                foreach (var ejercicio in ejercicios)
                {
                    var progresion = contexto.ProgresionesEjercicio
                        .FirstOrDefault(pr => pr.idEjercicio == ejercicio.idEjercicio
                                              && pr.NumeroSemana == numeroSemana)
                        ?? contexto.ProgresionesEjercicio
                            .Where(pr => pr.idEjercicio == ejercicio.idEjercicio)
                            .OrderBy(pr => pr.NumeroSemana)
                            .FirstOrDefault();

                    // Último peso registrado por el cliente en este ejercicio.
                    decimal? ultimoPeso = contexto.UserSetLogs
                        .Where(s => s.WorkoutExerciseId == ejercicio.idEjercicio
                                    && s.WorkoutLog.UserId == userId
                                    && s.WeightUsedKg != null)
                        .OrderByDescending(s => s.WorkoutLog.LoggedDate)
                        .ThenByDescending(s => s.Id)
                        .Select(s => s.WeightUsedKg)
                        .FirstOrDefault();

                    resultado.Ejercicios.Add(new EjercicioClienteDto
                    {
                        WorkoutExerciseId = ejercicio.idEjercicio,
                        NombreEjercicio = ejercicio.NombreEjercicio,
                        TipoMetrica = ejercicio.TipoMetrica,
                        SeriesObjetivo = progresion?.Series,
                        RepeticionesObjetivo = progresion?.RepeticionesObjetivo,
                        DuracionSegundosObjetivo = progresion?.DuracionSegundos,
                        CargaSugerida = progresion?.CargaOrpeSugerido,
                        UltimoPesoKg = ultimoPeso
                    });
                }

                return resultado;
            }
        }

        // Determina la semana vigente (1-4) del mesociclo según la fecha actual.
        private static int CalcularSemanaVigente(Contexto contexto, int idMesociclo)
        {
            var mesociclo = contexto.Mesociclos
                .FirstOrDefault(m => m.idMesociclo == idMesociclo);

            if (mesociclo == null)
            {
                return 1;
            }

            int dias = (DateTime.Today - mesociclo.FechaInicio.Date).Days;
            if (dias < 0)
            {
                return 1;
            }

            int semana = (dias / 7) + 1;
            if (semana < 1) semana = 1;
            if (semana > 4) semana = 4;
            return semana;
        }
    }
}
