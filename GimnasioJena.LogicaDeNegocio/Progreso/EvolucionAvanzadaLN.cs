using GimnasioJena.Abstracciones.AccesoADatos.Progreso;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Progreso;
using GimnasioJena.Abstracciones.Modelos.Progreso;
using GimnasioJena.AccesoADatos.Entidades.Progreso;
using GimnasioJena.AccesoADatos.Progreso;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GimnasioJena.LogicaDeNegocio.Progreso
{
    public class EvolucionAvanzadaLN : IEvolucionAvanzadaLN
    {
        private readonly IEntrenamientoAvanzadoAD _entrenamientoAvanzadoAD;
        private readonly IProgresoAvanzadoAD _progresoAvanzadoAD;
        private readonly IMedicionesAD _medicionesAD;
        private readonly IUsuarioActualAD _usuarioActualAD;

        public EvolucionAvanzadaLN()
        {
            _entrenamientoAvanzadoAD = new EntrenamientoAvanzadoAD();
            _progresoAvanzadoAD = new ProgresoAvanzadoAD();
            _medicionesAD = new MedicionesAD();
            _usuarioActualAD = new UsuarioActualAD();
        }

        public async Task GuardarOClonarRutinaLN(GuardarRutinaManualDto dto, bool esClonacion, bool puedeGestionarRutinas)
        {
            if (dto == null)
            {
                throw new ArgumentException("No se recibió la información de la rutina.");
            }

            if (!puedeGestionarRutinas)
            {
                throw new UnauthorizedAccessException("Solo un Administrador o Entrenador puede crear o clonar rutinas.");
            }

            if (string.IsNullOrWhiteSpace(dto.DiaSemana))
            {
                throw new ArgumentException("Debe indicar el día de la semana de la rutina.");
            }

            if (string.IsNullOrWhiteSpace(dto.NombreRutina))
            {
                throw new ArgumentException("Debe indicar el nombre de la rutina.");
            }

            // Lógica de clonación: si se clona y no hay contenido nuevo, se toma el del entrenamiento origen.
            if (esClonacion
                && dto.IdEntrenamientoClonado.HasValue
                && string.IsNullOrWhiteSpace(dto.TextoLibreMarkdown)
                && (dto.Bloques == null || dto.Bloques.Count == 0))
            {
                var origen = await _entrenamientoAvanzadoAD
                    .ObtenerEntrenamientoPorIdAD(dto.IdEntrenamientoClonado.Value);

                if (origen == null)
                {
                    throw new ArgumentException("No se encontró el entrenamiento que se desea clonar.");
                }

                dto.TextoLibreMarkdown = origen.EjerciciosDetalle;
            }

            // Se serializan los bloques o se almacena el markdown libre directamente.
            string detalle;
            if (dto.Bloques != null && dto.Bloques.Count > 0)
            {
                detalle = JsonConvert.SerializeObject(dto.Bloques);
            }
            else
            {
                detalle = dto.TextoLibreMarkdown ?? string.Empty;
            }

            var entidad = new EntrenamientoEntidad
            {
                DiaSemana = dto.DiaSemana,
                NombreRutina = dto.NombreRutina,
                EjerciciosDetalle = detalle,
                EsPlantilla = dto.EsPlantilla
            };

            await _entrenamientoAvanzadoAD.InsertarEntrenamientoFlexibleAD(entidad);
        }

        public async Task<GuardarRutinaManualDto> ObtenerRutinaParaClonarLN(int idEntrenamiento)
        {
            if (idEntrenamiento <= 0)
            {
                throw new ArgumentException("El entrenamiento indicado no es válido.");
            }

            var origen = await _entrenamientoAvanzadoAD
                .ObtenerEntrenamientoPorIdAD(idEntrenamiento);

            if (origen == null)
            {
                throw new ArgumentException("No se encontró el entrenamiento que se desea clonar.");
            }

            return new GuardarRutinaManualDto
            {
                IdEntrenamientoClonado = origen.IdEntrenamiento,
                DiaSemana = origen.DiaSemana,
                NombreRutina = origen.NombreRutina,
                TextoLibreMarkdown = origen.EjerciciosDetalle,
                EsPlantilla = origen.EsPlantilla
            };
        }

        public async Task<List<EntrenamientoDto>> ObtenerPlantillasDisponiblesLN()
        {
            var plantillas = await _entrenamientoAvanzadoAD.ObtenerPlantillasDisponiblesAD();

            return plantillas
                .Select(e => new EntrenamientoDto
                {
                    IdEntrenamiento = e.IdEntrenamiento,
                    DiaSemana = e.DiaSemana,
                    NombreRutina = e.NombreRutina,
                    EjerciciosDetalle = e.EjerciciosDetalle
                })
                .ToList();
        }

        public async Task RegistrarProgresoClienteLN(RegistroProgresoAvanzadoDto dto, string identityUserId, bool esCliente)
        {
            if (dto == null)
            {
                throw new ArgumentException("No se recibió la información del progreso.");
            }

            // Seguridad: el Cliente solo puede registrar contra su propio idUsuario validado.
            if (esCliente)
            {
                if (string.IsNullOrWhiteSpace(identityUserId))
                {
                    throw new ArgumentException("No se pudo identificar al usuario autenticado.");
                }

                var idUsuario = await _usuarioActualAD.ObtenerIdUsuarioPorIdentityAD(identityUserId);

                if (idUsuario == null || idUsuario <= 0)
                {
                    throw new ArgumentException("El usuario autenticado no está registrado en el sistema.");
                }

                dto.idUsuario = idUsuario.Value;
            }

            if (dto.idUsuario <= 0)
            {
                throw new ArgumentException("El usuario indicado no es válido.");
            }

            if (dto.IdEntrenamiento <= 0)
            {
                throw new ArgumentException("El entrenamiento indicado no es válido.");
            }

            if (dto.NivelEnergia.HasValue && (dto.NivelEnergia.Value < 1 || dto.NivelEnergia.Value > 5))
            {
                throw new ArgumentException("El nivel de energía debe estar entre 1 y 5.");
            }

            bool tieneFuerza = dto.PesoAlcanzado.HasValue || dto.Repeticiones.HasValue || dto.Series.HasValue;
            bool tieneCardio = dto.DistanciaKm.HasValue || dto.TiempoMinutos.HasValue;

            if (!tieneFuerza && !tieneCardio)
            {
                throw new ArgumentException("Debe registrar al menos métricas de fuerza o de cardio.");
            }

            var entidad = new ProgresoClienteEntidad
            {
                idUsuario = dto.idUsuario,
                IdEntrenamiento = dto.IdEntrenamiento,
                PesoAlcanzado = dto.PesoAlcanzado,
                Repeticiones = dto.Repeticiones,
                Series = dto.Series,
                RIR = dto.RIR,
                DistanciaKm = dto.DistanciaKm,
                TiempoMinutos = dto.TiempoMinutos,
                Pulsaciones = dto.Pulsaciones,
                NivelEnergia = dto.NivelEnergia,
                Notas = dto.Notas,
                FechaRegistro = DateTime.Now
            };

            await _progresoAvanzadoAD.GuardarProgresoMultimodalAD(entidad);
        }

        public async Task GuardarEntrenamientoClaseLN(CrearEntrenamientoClaseDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException("No se recibió la información del entrenamiento.");
            }

            // Validación: debe indicarse el tipo de asignación coherente con el identificador.
            bool esRecurrente = dto.IdHorarioSemanal.HasValue && dto.IdHorarioSemanal.Value > 0;
            bool esAislada = dto.IdClaseProgramada.HasValue && dto.IdClaseProgramada.Value > 0;

            if (!esRecurrente && !esAislada)
            {
                throw new ArgumentException("Debe indicar un horario semanal (recurrente) o una clase programada (aislada).");
            }

            if (esRecurrente && esAislada)
            {
                throw new ArgumentException("Solo puede asignar la rutina a un horario recurrente o a una clase aislada, no a ambos.");
            }

            if (string.IsNullOrWhiteSpace(dto.NombreRutina))
            {
                throw new ArgumentException("Debe indicar el nombre de la rutina.");
            }

            if (dto.Ejercicios == null || dto.Ejercicios.Count == 0)
            {
                throw new ArgumentException("Debe agregar al menos un ejercicio a la rutina.");
            }

            var detalle = JsonConvert.SerializeObject(dto.Ejercicios);

            var entidad = new EntrenamientoEntidad
            {
                DiaSemana = dto.DiaSemana,
                NombreRutina = dto.NombreRutina,
                EjerciciosDetalle = detalle,
                EsPlantilla = false,
                idHorario = esRecurrente ? dto.IdHorarioSemanal : null,
                idClaseProgramada = esAislada ? dto.IdClaseProgramada : null
            };

            await _entrenamientoAvanzadoAD.GuardarEntrenamientoAD(entidad);
        }

        public async Task<RegistrarProgresoClaseDto> ObtenerHojaProgresoParaClienteLN(int idClaseProgramada, int idUsuario)
        {
            if (idClaseProgramada <= 0)
            {
                throw new ArgumentException("La clase indicada no es válida.");
            }

            if (idUsuario <= 0)
            {
                throw new ArgumentException("El usuario indicado no es válido.");
            }

            var entrenamiento = await _entrenamientoAvanzadoAD.ObtenerEntrenamientoPorClaseAD(idClaseProgramada);

            if (entrenamiento == null)
            {
                throw new ArgumentException("No se encontró un entrenamiento asignado para esta clase.");
            }

            var hoja = new RegistrarProgresoClaseDto
            {
                IdClaseProgramada = idClaseProgramada,
                idUsuario = idUsuario
            };

            var ejercicios = DeserializarEjercicios(entrenamiento.EjerciciosDetalle);

            foreach (var ejercicio in ejercicios)
            {
                hoja.FilasEjercicios.Add(new ProgresoFilaDto
                {
                    NombreEjercicio = ejercicio.Nombre,
                    SeriesPrescritas = ejercicio.SeriesPrescritas ?? 0,
                    RepeticionesPrescritas = ejercicio.RepeticionesPrescritas ?? 0
                });
            }

            return hoja;
        }

        public async Task GuardarProgresoClienteLN(RegistrarProgresoClaseDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException("No se recibió la información del progreso.");
            }

            if (dto.idUsuario <= 0)
            {
                throw new ArgumentException("El usuario indicado no es válido.");
            }

            if (dto.IdClaseProgramada <= 0)
            {
                throw new ArgumentException("La clase indicada no es válida.");
            }

            if (dto.FilasEjercicios == null || dto.FilasEjercicios.Count == 0)
            {
                throw new ArgumentException("No hay ejercicios para registrar.");
            }

            var entrenamiento = await _entrenamientoAvanzadoAD.ObtenerEntrenamientoPorClaseAD(dto.IdClaseProgramada);

            if (entrenamiento == null)
            {
                throw new ArgumentException("No se encontró un entrenamiento asignado para esta clase.");
            }

            var fechaRegistro = DateTime.Now;

            var entidades = dto.FilasEjercicios
                .Select(fila => new ProgresoClienteEntidad
                {
                    idUsuario = dto.idUsuario,
                    IdEntrenamiento = entrenamiento.IdEntrenamiento,
                    idClaseProgramada = dto.IdClaseProgramada,
                    NombreEjercicio = fila.NombreEjercicio,
                    PesoAlcanzado = fila.PesoLogrado,
                    Repeticiones = fila.RepeticionesLogradas,
                    Series = fila.SeriesLogradas,
                    TiempoMinutos = fila.TiempoMinutos,
                    Notas = fila.NotasCliente,
                    FechaRegistro = fechaRegistro
                })
                .ToList();

            await _progresoAvanzadoAD.GuardarProgresoClienteDetalleAD(entidades);
        }

        private static List<EjercicioItemDto> DeserializarEjercicios(string detalle)
        {
            if (string.IsNullOrWhiteSpace(detalle))
            {
                return new List<EjercicioItemDto>();
            }

            try
            {
                var ejercicios = JsonConvert.DeserializeObject<List<EjercicioItemDto>>(detalle);
                return ejercicios ?? new List<EjercicioItemDto>();
            }
            catch (JsonException)
            {
                // Si el detalle es texto libre (markdown) y no JSON, se retorna una lista vacía.
                return new List<EjercicioItemDto>();
            }
        }

        public async Task<MetricasEvolucionAvanzadaDto> CompilarDashboardAvanzadoLN(int idUsuario, string periodo)
        {
            if (idUsuario <= 0)
            {
                throw new ArgumentException("El usuario indicado no es válido.");
            }

            var fechaFin = DateTime.Now;
            var fechaInicio = ObtenerFechaInicioSegunPeriodo(periodo, fechaFin);

            var progresos = await _progresoAvanzadoAD
                .ObtenerProgresoRangoAD(idUsuario, fechaInicio, fechaFin);

            var metricas = new MetricasEvolucionAvanzadaDto();

            // Volumen de fuerza = Peso * Repeticiones * Series.
            metricas.HistoricoVolumen = progresos
                .Where(p => p.PesoAlcanzado.HasValue && p.Repeticiones.HasValue && p.Series.HasValue)
                .Select(p => new PuntoVolumenFuerzaDto
                {
                    Fecha = p.FechaRegistro,
                    EjercicioRutina = p.IdEntrenamiento.ToString(),
                    VolumenTotal = p.PesoAlcanzado.Value * p.Repeticiones.Value * p.Series.Value
                })
                .ToList();

            // Cardio: ritmo medio = minutos por kilómetro.
            metricas.HistoricoCardio = progresos
                .Where(p => p.DistanciaKm.HasValue || p.TiempoMinutos.HasValue)
                .Select(p => new PuntoCardioDto
                {
                    Fecha = p.FechaRegistro,
                    Distancia = p.DistanciaKm,
                    RitmoMedio = (p.TiempoMinutos.HasValue && p.DistanciaKm.HasValue && p.DistanciaKm.Value > 0)
                        ? Math.Round(p.TiempoMinutos.Value / p.DistanciaKm.Value, 2)
                        : (decimal?)null,
                    Pulsaciones = p.Pulsaciones
                })
                .ToList();

            // Heatmap de asistencia anual.
            metricas.AsistenciaHeatmap = await _progresoAvanzadoAD
                .ObtenerMatrizAsistenciaAnualAD(idUsuario, fechaFin.Year);

            // Composición corporal (mediciones dinámicas).
            metricas.HistoricoComposicionCorporal = await _medicionesAD
                .ObtenerMedicionesUsuarioAD(idUsuario);

            return metricas;
        }

        private static DateTime ObtenerFechaInicioSegunPeriodo(string periodo, DateTime fechaFin)
        {
            switch ((periodo ?? string.Empty).Trim().ToUpperInvariant())
            {
                case "3MESES":
                    return fechaFin.AddMonths(-3);
                case "6MESES":
                    return fechaFin.AddMonths(-6);
                case "ANUAL":
                    return fechaFin.AddYears(-1);
                case "1MES":
                default:
                    return fechaFin.AddMonths(-1);
            }
        }
    }
}
