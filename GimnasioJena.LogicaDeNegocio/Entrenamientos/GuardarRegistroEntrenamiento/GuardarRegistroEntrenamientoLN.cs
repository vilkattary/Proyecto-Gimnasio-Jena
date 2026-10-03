using GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.GuardarRegistroEntrenamiento;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenamientos.GuardarRegistroEntrenamiento;
using GimnasioJena.Abstracciones.Modelos.Entrenamientos;
using GimnasioJena.AccesoADatos.Entrenamientos.GuardarRegistroEntrenamiento;
using System;
using System.Linq;

namespace GimnasioJena.LogicaDeNegocio.Entrenamientos.GuardarRegistroEntrenamiento
{
    public class GuardarRegistroEntrenamientoLN : IGuardarRegistroEntrenamientoLN
    {
        private readonly IGuardarRegistroEntrenamientoAD _guardarRegistroEntrenamientoAD;

        public GuardarRegistroEntrenamientoLN()
        {
            _guardarRegistroEntrenamientoAD = new GuardarRegistroEntrenamientoAD();
        }

        public ResultadoRegistroEntrenamientoDto GuardarRegistroEntrenamiento(
            RegistrarEntrenamientoClienteDto modelo)
        {
            if (modelo == null)
            {
                return Error("No se recibieron datos del entrenamiento.");
            }

            if (modelo.UserId <= 0)
            {
                return Error("Usuario no válido.");
            }

            if (modelo.ClassInstanceId <= 0 || modelo.WorkoutDayTemplateId <= 0)
            {
                return Error("La clase o la rutina asociada no son válidas.");
            }

            if (modelo.Series == null || !modelo.Series.Any())
            {
                return Error("Debes registrar al menos una serie.");
            }

            // Calcula VolumeLoad y Estimated1RM (Epley) por serie antes de persistir.
            decimal mejor1RMActual = 0m;

            foreach (var serie in modelo.Series)
            {
                if (serie.SetNumber <= 0)
                {
                    return Error("El número de serie debe ser mayor que cero.");
                }

                decimal reps = serie.RepsCompleted ?? 0;
                decimal peso = serie.WeightUsedKg ?? 0m;

                serie.VolumeLoad = reps > 0 && peso > 0
                    ? Math.Round(reps * peso, 2)
                    : (decimal?)null;

                // Epley: 1RM = peso * (1 + reps/30)
                serie.Estimated1RM = reps > 0 && peso > 0
                    ? Math.Round(peso * (1m + (reps / 30m)), 2)
                    : (decimal?)null;

                if (serie.Estimated1RM.HasValue && serie.Estimated1RM.Value > mejor1RMActual)
                {
                    mejor1RMActual = serie.Estimated1RM.Value;
                }
            }

            var resultado = _guardarRegistroEntrenamientoAD
                .GuardarRegistroEntrenamiento(modelo);

            if (!resultado.Exito)
            {
                return resultado;
            }

            // Detección de récord personal: compara el mejor 1RM de esta sesión
            // contra el histórico previo del usuario (excluyendo la sesión recién guardada).
            bool nuevoRecord = false;

            if (mejor1RMActual > 0m)
            {
                foreach (var exerciseId in modelo.Series
                             .Select(s => s.WorkoutExerciseId).Distinct())
                {
                    decimal? historico = _guardarRegistroEntrenamientoAD
                        .ObtenerMejor1RMHistorico(modelo.UserId, exerciseId, resultado.idWorkoutLog);

                    decimal mejorSesionEjercicio = modelo.Series
                        .Where(s => s.WorkoutExerciseId == exerciseId && s.Estimated1RM.HasValue)
                        .Select(s => s.Estimated1RM.Value)
                        .DefaultIfEmpty(0m)
                        .Max();

                    if (mejorSesionEjercicio > 0m
                        && (!historico.HasValue || mejorSesionEjercicio > historico.Value))
                    {
                        nuevoRecord = true;
                        break;
                    }
                }
            }

            resultado.NuevoRecord = nuevoRecord;
            resultado.Mensaje = nuevoRecord
                ? "¡Entrenamiento registrado! Rompiste un récord personal."
                : "Entrenamiento registrado correctamente.";

            return resultado;
        }

        private static ResultadoRegistroEntrenamientoDto Error(string mensaje)
        {
            return new ResultadoRegistroEntrenamientoDto
            {
                Exito = false,
                Mensaje = mensaje
            };
        }
    }
}
