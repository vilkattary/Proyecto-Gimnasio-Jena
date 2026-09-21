using GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.GuardarPlantillaDiaEntrenamiento;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenamientos.GuardarPlantillaDiaEntrenamiento;
using GimnasioJena.Abstracciones.Modelos.Entrenamientos;
using GimnasioJena.AccesoADatos.Entrenamientos.GuardarPlantillaDiaEntrenamiento;
using System.Collections.Generic;
using System.Linq;

namespace GimnasioJena.LogicaDeNegocio.Entrenamientos.GuardarPlantillaDiaEntrenamiento
{
    public class GuardarPlantillaDiaEntrenamientoLN
        : IGuardarPlantillaDiaEntrenamientoLN
    {
        private const int MetricaRepeticiones = 1;
        private const int MetricaTiempo = 2;
        private const int MetricaDistancia = 3;

        private readonly IGuardarPlantillaDiaEntrenamientoAD
            _guardarPlantillaDiaEntrenamientoAD;

        public GuardarPlantillaDiaEntrenamientoLN()
        {
            _guardarPlantillaDiaEntrenamientoAD =
                new GuardarPlantillaDiaEntrenamientoAD();
        }

        public ResultadoGuardarPlantillaDto GuardarPlantillaDiaEntrenamiento(
            GuardarPlantillaDiaDto modelo
        )
        {
            if (modelo == null)
            {
                return Error("Debe indicar los datos de la rutina del día.");
            }

            if (modelo.idMesociclo <= 0 && modelo.idPlantillaDia <= 0)
            {
                return Error("Debe indicar el mesociclo al que pertenece la rutina.");
            }

            if (modelo.DiaSemana < 1 || modelo.DiaSemana > 7)
            {
                return Error("El día de la semana no es válido.");
            }

            if (modelo.Ejercicios == null || !modelo.Ejercicios.Any())
            {
                return Error("Debe agregar al menos un ejercicio al día.");
            }

            modelo.AreaEnfoque = modelo.AreaEnfoque?.Trim();

            foreach (EjercicioDto ejercicio in modelo.Ejercicios)
            {
                if (string.IsNullOrWhiteSpace(ejercicio.NombreEjercicio))
                {
                    return Error("Todos los ejercicios deben tener un nombre.");
                }

                ejercicio.NombreEjercicio = ejercicio.NombreEjercicio.Trim();
                ejercicio.Notas = ejercicio.Notas?.Trim();

                NormalizarProgresiones(ejercicio);
                AplicarAutoCascade(ejercicio);

                string errorValidacion = ValidarProgresiones(ejercicio);
                if (errorValidacion != null)
                {
                    return Error(errorValidacion);
                }
            }

            return _guardarPlantillaDiaEntrenamientoAD
                .GuardarPlantillaDiaEntrenamiento(modelo);
        }

        // Garantiza que existan las 4 semanas (1..4) por ejercicio.
        private static void NormalizarProgresiones(EjercicioDto ejercicio)
        {
            if (ejercicio.Progresiones == null)
            {
                ejercicio.Progresiones = new List<ProgresionSemanaDto>();
            }

            for (int semana = 1; semana <= 4; semana++)
            {
                if (!ejercicio.Progresiones.Any(p => p.NumeroSemana == semana))
                {
                    ejercicio.Progresiones.Add(new ProgresionSemanaDto
                    {
                        NumeroSemana = semana
                    });
                }
            }

            ejercicio.Progresiones = ejercicio.Progresiones
                .Where(p => p.NumeroSemana >= 1 && p.NumeroSemana <= 4)
                .OrderBy(p => p.NumeroSemana)
                .ToList();
        }

        // Regla 1: si las semanas 2,3,4 están vacías, clonar los valores de la semana 1.
        private static void AplicarAutoCascade(EjercicioDto ejercicio)
        {
            ProgresionSemanaDto semana1 =
                ejercicio.Progresiones.First(p => p.NumeroSemana == 1);

            foreach (ProgresionSemanaDto progresion in ejercicio.Progresiones
                .Where(p => p.NumeroSemana > 1))
            {
                if (EstaVacia(progresion))
                {
                    progresion.Series = semana1.Series;
                    progresion.RepeticionesObjetivo = semana1.RepeticionesObjetivo;
                    progresion.DuracionSegundos = semana1.DuracionSegundos;
                    progresion.CargaOrpeSugerido = semana1.CargaOrpeSugerido;
                }
            }
        }

        private static bool EstaVacia(ProgresionSemanaDto progresion)
        {
            return !progresion.Series.HasValue
                && string.IsNullOrWhiteSpace(progresion.RepeticionesObjetivo)
                && !progresion.DuracionSegundos.HasValue
                && string.IsNullOrWhiteSpace(progresion.CargaOrpeSugerido);
        }

        // Regla 2 + validación: valores positivos según la métrica del ejercicio.
        private static string ValidarProgresiones(EjercicioDto ejercicio)
        {
            foreach (ProgresionSemanaDto progresion in ejercicio.Progresiones)
            {
                if (progresion.Series.HasValue && progresion.Series.Value <= 0)
                {
                    return $"El ejercicio '{ejercicio.NombreEjercicio}' tiene series inválidas en la semana {progresion.NumeroSemana}.";
                }

                if (ejercicio.TipoMetrica == MetricaTiempo)
                {
                    progresion.RepeticionesObjetivo = null;

                    if (progresion.DuracionSegundos.HasValue
                        && progresion.DuracionSegundos.Value <= 0)
                    {
                        return $"El ejercicio '{ejercicio.NombreEjercicio}' tiene una duración inválida en la semana {progresion.NumeroSemana}.";
                    }
                }
                else
                {
                    // Repeticiones o Distancia: no se usa duración.
                    if (ejercicio.TipoMetrica == MetricaRepeticiones)
                    {
                        progresion.DuracionSegundos = null;
                    }

                    if (!string.IsNullOrWhiteSpace(progresion.RepeticionesObjetivo))
                    {
                        int repeticiones;
                        if (int.TryParse(progresion.RepeticionesObjetivo.Trim(), out repeticiones)
                            && repeticiones <= 0)
                        {
                            return $"El ejercicio '{ejercicio.NombreEjercicio}' tiene repeticiones inválidas en la semana {progresion.NumeroSemana}.";
                        }
                    }
                }
            }

            return null;
        }

        private static ResultadoGuardarPlantillaDto Error(string mensaje)
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
