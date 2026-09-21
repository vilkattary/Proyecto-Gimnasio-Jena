using GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.GuardarRegistroEntrenamiento;
using GimnasioJena.Abstracciones.Modelos.Entrenamientos;
using GimnasioJena.AccesoADatos.Entidades.Entrenamientos;
using System;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Entrenamientos.GuardarRegistroEntrenamiento
{
    public class GuardarRegistroEntrenamientoAD : IGuardarRegistroEntrenamientoAD
    {
        public ResultadoRegistroEntrenamientoDto GuardarRegistroEntrenamiento(
            RegistrarEntrenamientoClienteDto modelo)
        {
            using (Contexto contexto = new Contexto())
            using (var transaccion = contexto.Database.BeginTransaction())
            {
                try
                {
                    var log = new UserWorkoutLogEntidad
                    {
                        UserId = modelo.UserId,
                        ClassInstanceId = modelo.ClassInstanceId,
                        WorkoutDayTemplateId = modelo.WorkoutDayTemplateId,
                        LoggedDate = DateTime.UtcNow,
                        Notes = modelo.Notes?.Trim()
                    };

                    contexto.UserWorkoutLogs.Add(log);
                    contexto.SaveChanges();

                    foreach (var serie in modelo.Series)
                    {
                        var setLog = new UserSetLogEntidad
                        {
                            UserWorkoutLogId = log.Id,
                            WorkoutExerciseId = serie.WorkoutExerciseId,
                            SetNumber = serie.SetNumber,
                            RepsCompleted = serie.RepsCompleted,
                            WeightUsedKg = serie.WeightUsedKg,
                            DurationSeconds = serie.DurationSeconds,
                            DistanceMeters = serie.DistanceMeters,
                            Estimated1RM = serie.Estimated1RM,
                            VolumeLoad = serie.VolumeLoad
                        };

                        contexto.UserSetLogs.Add(setLog);
                    }

                    contexto.SaveChanges();
                    transaccion.Commit();

                    return new ResultadoRegistroEntrenamientoDto
                    {
                        Exito = true,
                        Mensaje = "Entrenamiento registrado correctamente.",
                        idWorkoutLog = log.Id
                    };
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    return new ResultadoRegistroEntrenamientoDto
                    {
                        Exito = false,
                        Mensaje = "No se pudo registrar el entrenamiento: " + ex.Message
                    };
                }
            }
        }

        public decimal? ObtenerMejor1RMHistorico(
            int userId, int workoutExerciseId, int excluirWorkoutLogId)
        {
            using (Contexto contexto = new Contexto())
            {
                return contexto.UserSetLogs
                    .Where(s => s.WorkoutExerciseId == workoutExerciseId
                                && s.WorkoutLog.UserId == userId
                                && s.UserWorkoutLogId != excluirWorkoutLogId
                                && s.Estimated1RM != null)
                    .Select(s => s.Estimated1RM)
                    .OrderByDescending(v => v)
                    .FirstOrDefault();
            }
        }
    }
}
