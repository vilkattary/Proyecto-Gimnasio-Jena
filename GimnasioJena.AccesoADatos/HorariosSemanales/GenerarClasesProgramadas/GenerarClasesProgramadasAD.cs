using GimnasioJena.Abstracciones.AccesoADatos.HorariosSemanales.GenerarClasesProgramadas;
using GimnasioJena.Abstracciones.Modelos.HorariosSemanales;
using GimnasioJena.AccesoADatos.Entidades.Clases;
using GimnasioJena.AccesoADatos.Entidades.HorariosSemanales;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GimnasioJena.AccesoADatos.HorariosSemanales.GenerarClasesProgramadas
{
    public class GenerarClasesProgramadasAD :
        IGenerarClasesProgramadasAD
    {
        public ResultadoGeneracionClasesDto
            GenerarClasesProgramadas(
                GenerarClasesProgramadasDto modelo
            )
        {
            DateTime fechaInicio =
                modelo.fechaInicio.Date;

            DateTime fechaFin =
                modelo.fechaFin.Date;

            int diasProcesados =
                (fechaFin - fechaInicio).Days + 1;

            using (Contexto contexto = new Contexto())
            using (
                var transaccion =
                    contexto.Database.BeginTransaction()
            )
            {
                try
                {
                    int? idEstadoClaseActiva =
                        contexto.EstadoClases
                            .Where(e =>
                                e.estado &&
                                e.nombreEstado == "Activa"
                            )
                            .Select(e =>
                                (int?)e.idEstadoClase
                            )
                            .FirstOrDefault();

                    if (!idEstadoClaseActiva.HasValue)
                    {
                        transaccion.Rollback();

                        return CrearResultadoError(
                            "No existe un estado de clase activo con el nombre 'Activa'.",
                            fechaInicio,
                            fechaFin,
                            diasProcesados
                        );
                    }

                    List<HorarioSemanalEntidad>
                        horariosActivos =
                            contexto.HorariosSemanales
                                .Where(h => h.estado)
                                .OrderBy(h => h.diaSemana)
                                .ThenBy(h => h.horaInicio)
                                .ToList();

                    if (!horariosActivos.Any())
                    {
                        transaccion.Rollback();

                        return new ResultadoGeneracionClasesDto
                        {
                            fueExitosa = true,

                            mensaje =
                                "No se generaron clases porque no existen horarios semanales activos.",

                            clasesGeneradas = 0,
                            clasesOmitidas = 0,
                            horariosProcesados = 0,
                            diasProcesados = diasProcesados,
                            fechaInicioGenerada = fechaInicio,
                            fechaFinGenerada = fechaFin
                        };
                    }

                    /*
                     * Se cargan únicamente las clases del periodo
                     * que fueron creadas desde un horario semanal.
                     *
                     * Con esta información se construye un HashSet
                     * para detectar duplicados en memoria sin ejecutar
                     * una consulta SQL por cada posible clase.
                     */
                    var clasesExistentes =
    contexto.Clases
        .Where(c =>
            c.idHorario.HasValue &&
            c.fechaClase >= fechaInicio &&
            c.fechaClase <= fechaFin
        )
        .Select(c =>
            new
            {
                idHorario = c.idHorario.Value,
                fechaClase = c.fechaClase,
                horaInicio = c.horaInicio,
                horaFin = c.horaFin
            }
        )
        .ToList();

                    HashSet<string> clavesExistentes =
                        new HashSet<string>(
                            clasesExistentes.Select(c =>
                                ConstruirClaveClase(
                                    c.idHorario,
                                    c.fechaClase,
                                    c.horaInicio,
                                    c.horaFin
                                )
                            )
                        );

                    List<ClaseEntidad> clasesNuevas =
                        new List<ClaseEntidad>();

                    int clasesOmitidas = 0;

                    DateTime fechaCreacion =
                        DateTime.Now;

                    for (
                        DateTime fechaActual = fechaInicio;
                        fechaActual <= fechaFin;
                        fechaActual =
                            fechaActual.AddDays(1)
                    )
                    {
                        byte numeroDiaSemana =
                            ConvertirDiaSemana(
                                fechaActual.DayOfWeek
                            );

                        IEnumerable<HorarioSemanalEntidad>
                            horariosDelDia =
                                horariosActivos
                                    .Where(h =>
                                        h.diaSemana ==
                                        numeroDiaSemana
                                    );

                        foreach (
                            HorarioSemanalEntidad horario
                            in horariosDelDia
                        )
                        {
                            string claveClase =
                                ConstruirClaveClase(
                                    horario.idHorario,
                                    fechaActual,
                                    horario.horaInicio,
                                    horario.horaFin
                                );

                            if (
                                clavesExistentes.Contains(
                                    claveClase
                                )
                            )
                            {
                                clasesOmitidas++;

                                continue;
                            }

                            ClaseEntidad nuevaClase =
                                CrearClaseProgramada(
                                    horario,
                                    fechaActual,
                                    idEstadoClaseActiva.Value,
                                    fechaCreacion
                                );

                            clasesNuevas.Add(nuevaClase);

                            /*
                             * Se agrega inmediatamente al HashSet
                             * para impedir duplicados dentro de esta
                             * misma operación de generación.
                             */
                            clavesExistentes.Add(
                                claveClase
                            );
                        }
                    }

                    if (clasesNuevas.Any())
                    {
                        contexto.Clases.AddRange(
                            clasesNuevas
                        );

                        contexto.SaveChanges();
                    }

                    transaccion.Commit();

                    return CrearResultadoExitoso(
                        fechaInicio,
                        fechaFin,
                        diasProcesados,
                        horariosActivos.Count,
                        clasesNuevas.Count,
                        clasesOmitidas
                    );
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();

                    Exception excepcionReal =
                        ObtenerExcepcionReal(ex);

                    return CrearResultadoError(
                        "Ocurrió un error al generar las clases programadas. " +
                        "Detalle: " +
                        excepcionReal.Message,
                        fechaInicio,
                        fechaFin,
                        diasProcesados
                    );
                }
            }
        }

        private ClaseEntidad CrearClaseProgramada(
            HorarioSemanalEntidad horario,
            DateTime fechaClase,
            int idEstadoClase,
            DateTime fechaCreacion
        )
        {
            return new ClaseEntidad
            {
                idHorario =
                    horario.idHorario,

                idTipoClase =
                    horario.idTipoClase,

                idUsuarioEntrenador =
                    horario.idUsuarioEntrenador,

                idEstadoClase =
                    idEstadoClase,

                fechaClase =
                    fechaClase.Date,

                horaInicio =
                    horario.horaInicio,

                horaFin =
                    horario.horaFin,

                cupoMaximo =
                    horario.cupoMaximo,

                ubicacion =
                    horario.ubicacion,

                observaciones =
                    "Clase generada automáticamente desde la programación semanal.",

                fechaCreacion =
                    fechaCreacion,

                fechaModificacion =
                    null
            };
        }

        private byte ConvertirDiaSemana(
            DayOfWeek dia
        )
        {
            /*
             * DayOfWeek utiliza:
             *
             * Domingo = 0
             * Lunes = 1
             * ...
             * Sábado = 6
             *
             * HorarioSemanal utiliza:
             *
             * Lunes = 1
             * ...
             * Domingo = 7
             */
            return dia == DayOfWeek.Sunday
                ? (byte)7
                : (byte)dia;
        }

        private string ConstruirClaveClase(
            int idHorario,
            DateTime fechaClase,
            TimeSpan horaInicio,
            TimeSpan horaFin
        )
        {
            return
                idHorario
                + "|"
                + fechaClase.Date.ToString("yyyyMMdd")
                + "|"
                + horaInicio.Ticks
                + "|"
                + horaFin.Ticks;
        }

        private ResultadoGeneracionClasesDto
            CrearResultadoExitoso(
                DateTime fechaInicio,
                DateTime fechaFin,
                int diasProcesados,
                int horariosProcesados,
                int clasesGeneradas,
                int clasesOmitidas
            )
        {
            string mensaje;

            if (
                clasesGeneradas == 0 &&
                clasesOmitidas > 0
            )
            {
                mensaje =
                    "No se generaron clases nuevas porque todas las clases del periodo ya existían.";
            }
            else if (
                clasesGeneradas == 0 &&
                clasesOmitidas == 0
            )
            {
                mensaje =
                    "No había clases por generar dentro del periodo seleccionado.";
            }
            else
            {
                mensaje =
                    $"Se generaron correctamente {clasesGeneradas} " +
                    $"{(clasesGeneradas == 1 ? "clase" : "clases")}.";

                if (clasesOmitidas > 0)
                {
                    mensaje +=
                        $" Se omitieron {clasesOmitidas} " +
                        $"{(clasesOmitidas == 1 ? "clase" : "clases")} " +
                        "porque ya existían.";
                }
            }

            return new ResultadoGeneracionClasesDto
            {
                fueExitosa = true,
                mensaje = mensaje,
                clasesGeneradas = clasesGeneradas,
                clasesOmitidas = clasesOmitidas,
                horariosProcesados = horariosProcesados,
                diasProcesados = diasProcesados,
                fechaInicioGenerada = fechaInicio,
                fechaFinGenerada = fechaFin
            };
        }

        private ResultadoGeneracionClasesDto
            CrearResultadoError(
                string mensaje,
                DateTime fechaInicio,
                DateTime fechaFin,
                int diasProcesados
            )
        {
            return new ResultadoGeneracionClasesDto
            {
                fueExitosa = false,
                mensaje = mensaje,
                clasesGeneradas = 0,
                clasesOmitidas = 0,
                horariosProcesados = 0,
                diasProcesados = diasProcesados,
                fechaInicioGenerada = fechaInicio,
                fechaFinGenerada = fechaFin
            };
        }

        private Exception ObtenerExcepcionReal(
            Exception excepcion
        )
        {
            Exception excepcionReal =
                excepcion;

            while (
                excepcionReal.InnerException != null
            )
            {
                excepcionReal =
                    excepcionReal.InnerException;
            }

            return excepcionReal;
        }
    }
}