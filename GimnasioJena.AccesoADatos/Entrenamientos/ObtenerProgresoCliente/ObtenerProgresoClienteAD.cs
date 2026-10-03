using GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.ObtenerProgresoCliente;
using GimnasioJena.Abstracciones.Modelos.Entrenamientos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Entrenamientos.ObtenerProgresoCliente
{
    public class ObtenerProgresoClienteAD : IObtenerProgresoClienteAD
    {
        public ProgresoClienteDto ObtenerProgreso(int userId, int ultimosDias)
        {
            using (Contexto contexto = new Contexto())
            {
                var resultado = new ProgresoClienteDto();

                DateTime? desde = null;
                if (ultimosDias > 0)
                {
                    desde = DateTime.Today.AddDays(-ultimosDias);
                }

                // Sesiones del usuario dentro del rango (con sus series cargadas en memoria).
                var sesiones = contexto.UserWorkoutLogs
                    .Where(w => w.UserId == userId
                                && (desde == null || w.LoggedDate >= desde))
                    .Select(w => new
                    {
                        w.Id,
                        w.LoggedDate,
                        Series = w.Series.Select(s => new
                        {
                            s.WorkoutExerciseId,
                            s.VolumeLoad,
                            s.Estimated1RM
                        })
                    })
                    .ToList();

                resultado.TotalSesiones = sesiones.Count;
                resultado.UltimaSesion = sesiones.Any()
                    ? sesiones.Max(s => (DateTime?)s.LoggedDate)
                    : null;

                // Volumen total por sesión (suma de VolumeLoad de sus series).
                resultado.VolumenPorSesion = sesiones
                    .OrderBy(s => s.LoggedDate)
                    .Select(s => new PuntoVolumenDto
                    {
                        Fecha = s.LoggedDate,
                        VolumenTotal = s.Series.Sum(x => x.VolumeLoad ?? 0m)
                    })
                    .ToList();

                resultado.VolumenTotalAcumulado =
                    resultado.VolumenPorSesion.Sum(p => p.VolumenTotal);
                resultado.TotalSeriesRegistradas =
                    sesiones.Sum(s => s.Series.Count());

                // Asistencia: número de sesiones entrenadas agrupadas por mes.
                resultado.AsistenciaPorMes = sesiones
                    .GroupBy(s => new { s.LoggedDate.Year, s.LoggedDate.Month })
                    .Select(g => new PuntoAsistenciaDto
                    {
                        Mes = new DateTime(g.Key.Year, g.Key.Month, 1),
                        Sesiones = g.Count()
                    })
                    .OrderBy(p => p.Mes)
                    .ToList();

                // Fechas (una por día) en las que hubo al menos una sesión, para el
                // calendario de asistencia coloreado por día de la semana.
                resultado.FechasAsistencia = sesiones
                    .Select(s => s.LoggedDate.Date)
                    .Distinct()
                    .OrderBy(f => f)
                    .ToList();

                // Evolución del mejor 1RM estimado por ejercicio y por sesión.
                var puntos1RM = sesiones
                    .SelectMany(s => s.Series
                        .Where(x => x.Estimated1RM.HasValue)
                        .Select(x => new
                        {
                            x.WorkoutExerciseId,
                            s.LoggedDate,
                            Valor = x.Estimated1RM.Value
                        }))
                    .ToList();

                var idsEjercicios = puntos1RM
                    .Select(p => p.WorkoutExerciseId)
                    .Distinct()
                    .ToList();

                var nombres = contexto.EjerciciosEntrenamiento
                    .Where(e => idsEjercicios.Contains(e.idEjercicio))
                    .ToDictionary(e => e.idEjercicio, e => e.NombreEjercicio);

                resultado.Evolucion1RM = puntos1RM
                    .GroupBy(p => p.WorkoutExerciseId)
                    .Select(g => new SerieEjercicio1RMDto
                    {
                        WorkoutExerciseId = g.Key,
                        NombreEjercicio = nombres.ContainsKey(g.Key)
                            ? nombres[g.Key]
                            : "Ejercicio " + g.Key,
                        // Mejor 1RM por día (evita ruido de varias series el mismo día).
                        Puntos = g
                            .GroupBy(x => x.LoggedDate.Date)
                            .OrderBy(dg => dg.Key)
                            .Select(dg => new Punto1RMDto
                            {
                                Fecha = dg.Key,
                                Mejor1RM = dg.Max(x => x.Valor)
                            })
                            .ToList()
                    })
                    .OrderBy(s => s.NombreEjercicio)
                    .ToList();

                // Historial de biometría.
                resultado.Biometria = contexto.UserBiometrics
                    .Where(b => b.UserId == userId
                                && (desde == null || b.MeasurementDate >= desde))
                    .OrderBy(b => b.MeasurementDate)
                    .Select(b => new PuntoBiometriaDto
                    {
                        Fecha = b.MeasurementDate,
                        PesoKg = b.WeightKg,
                        GrasaPorcentaje = b.BodyFatPercentage,
                        MusculoPorcentaje = b.MuscleMassPercentage
                    })
                    .ToList();

                return resultado;
            }
        }
    }
}
