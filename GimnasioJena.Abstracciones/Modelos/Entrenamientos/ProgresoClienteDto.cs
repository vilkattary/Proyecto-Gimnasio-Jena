using System;
using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.Modelos.Entrenamientos
{
    // Agregados de progreso del cliente para alimentar las gráficas (Etapa 3).
    public class ProgresoClienteDto
    {
        // Indicadores resumen.
        public int TotalSesiones { get; set; }
        public decimal VolumenTotalAcumulado { get; set; }
        public int TotalSeriesRegistradas { get; set; }
        public DateTime? UltimaSesion { get; set; }

        // Serie temporal de volumen por sesión (carga total = reps x kg).
        public List<PuntoVolumenDto> VolumenPorSesion { get; set; }
            = new List<PuntoVolumenDto>();

        // Evolución del 1RM estimado agrupada por ejercicio.
        public List<SerieEjercicio1RMDto> Evolucion1RM { get; set; }
            = new List<SerieEjercicio1RMDto>();

        // Historial de biometría (peso corporal y composición).
        public List<PuntoBiometriaDto> Biometria { get; set; }
            = new List<PuntoBiometriaDto>();

        // Asistencia (número de sesiones entrenadas agrupadas por mes).
        public List<PuntoAsistenciaDto> AsistenciaPorMes { get; set; }
            = new List<PuntoAsistenciaDto>();

        // Fechas individuales en las que hubo al menos una sesión (para el
        // calendario de asistencia: fila = mes, columna = día, color = día de semana).
        public List<DateTime> FechasAsistencia { get; set; }
            = new List<DateTime>();
    }

    public class PuntoVolumenDto
    {
        public DateTime Fecha { get; set; }
        public decimal VolumenTotal { get; set; }
    }

    public class SerieEjercicio1RMDto
    {
        public int WorkoutExerciseId { get; set; }
        public string NombreEjercicio { get; set; }
        public List<Punto1RMDto> Puntos { get; set; }
            = new List<Punto1RMDto>();
    }

    public class Punto1RMDto
    {
        public DateTime Fecha { get; set; }
        public decimal Mejor1RM { get; set; }
    }

    public class PuntoBiometriaDto
    {
        public DateTime Fecha { get; set; }
        public decimal PesoKg { get; set; }
        public decimal? GrasaPorcentaje { get; set; }
        public decimal? MusculoPorcentaje { get; set; }
    }

    public class PuntoAsistenciaDto
    {
        // Primer día del mes representado (para eje temporal).
        public DateTime Mes { get; set; }
        // Número de sesiones entrenadas en ese mes.
        public int Sesiones { get; set; }
    }
}
