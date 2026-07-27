using GimnasioJena.Abstracciones.AccesoADatos.HorariosSemanales.ObtenerHorariosSemanales;
using GimnasioJena.Abstracciones.AccesoADatos.HorariosSemanales.RegistrarHorariosSemanales;
using GimnasioJena.Abstracciones.LogicaDeNegocio.HorariosSemanales.RegistrarHorariosSemanales;
using GimnasioJena.Abstracciones.Modelos.HorariosSemanales;
using GimnasioJena.AccesoADatos.HorariosSemanales.ObtenerHorariosSemanales;
using GimnasioJena.AccesoADatos.HorariosSemanales.RegistrarHorariosSemanales;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GimnasioJena.LogicaDeNegocio.HorariosSemanales.RegistrarHorariosSemanales
{
    public class RegistrarHorariosSemanalesLN
        : IRegistrarHorariosSemanalesLN
    {
        private readonly IRegistrarHorariosSemanalesAD
            _registrarHorariosSemanalesAD;

        private readonly IObtenerHorariosSemanalesAD
            _obtenerHorariosSemanalesAD;

        public RegistrarHorariosSemanalesLN()
        {
            _registrarHorariosSemanalesAD =
                new RegistrarHorariosSemanalesAD();

            _obtenerHorariosSemanalesAD =
                new ObtenerHorariosSemanalesAD();
        }

        public ResultadoRegistroHorariosDto
            RegistrarHorariosSemanales(
                HorarioSemanalMultipleCrearDto modelo
            )
        {
            ResultadoRegistroHorariosDto resultado =
                new ResultadoRegistroHorariosDto();

            try
            {
                if (modelo != null)
                {
                    modelo.ubicacion = "Sala Principal";
                }

                ValidarModeloGeneral(modelo);

                List<HorarioSemanalDetalleCrearDto> horariosOrdenados =
                    modelo.horarios
                        .OrderBy(h => h.horaInicio)
                        .ToList();

                ValidarRangosIndividuales(horariosOrdenados);

                ValidarTraslapesDentroDelFormulario(
                    horariosOrdenados
                );

                ValidarTraslapesConHorariosExistentes(
                    modelo,
                    horariosOrdenados
                );

                modelo.horarios = horariosOrdenados;

                int cantidadRegistrada =
                    _registrarHorariosSemanalesAD
                        .RegistrarHorariosSemanales(modelo);

                resultado.fueExitoso = true;
                resultado.cantidadRegistrada =
                    cantidadRegistrada;

                resultado.mensaje =
                    cantidadRegistrada == 1
                        ? "El horario semanal se registró correctamente."
                        : $"Se registraron {cantidadRegistrada} horarios semanales correctamente.";

                return resultado;
            }
            catch (Exception ex)
            {
                resultado.fueExitoso = false;
                resultado.cantidadRegistrada = 0;
                resultado.mensaje = ex.Message;

                return resultado;
            }
        }

        private void ValidarModeloGeneral(
            HorarioSemanalMultipleCrearDto modelo
        )
        {
            if (modelo == null)
            {
                throw new Exception(
                    "No se recibieron los datos del horario semanal."
                );
            }

            if (modelo.idTipoClase <= 0)
            {
                throw new Exception(
                    "Debe seleccionar un tipo de clase."
                );
            }

            if (modelo.idUsuarioEntrenador <= 0)
            {
                throw new Exception(
                    "Debe seleccionar un entrenador."
                );
            }

            if (modelo.diaSemana < 1 ||
                modelo.diaSemana > 7)
            {
                throw new Exception(
                    "Debe seleccionar un día válido."
                );
            }

            if (modelo.cupoMaximo <= 0)
            {
                throw new Exception(
                    "El cupo máximo debe ser mayor que cero."
                );
            }

            if (modelo.cupoMaximo > 30)
            {
                throw new Exception(
                    "El cupo máximo no puede superar los 30 espacios."
                );
            }

            if (modelo.horarios == null ||
                !modelo.horarios.Any())
            {
                throw new Exception(
                    "Debe agregar al menos un rango horario."
                );
            }
        }

        private void ValidarRangosIndividuales(
            List<HorarioSemanalDetalleCrearDto> horarios
        )
        {
            foreach (var horario in horarios)
            {
                if (horario.horaFin <= horario.horaInicio)
                {
                    throw new Exception(
                        $"La hora final debe ser mayor que la hora inicial " +
                        $"en el rango {FormatearHora(horario.horaInicio)} - " +
                        $"{FormatearHora(horario.horaFin)}."
                    );
                }

                TimeSpan duracion =
                    horario.horaFin - horario.horaInicio;

                if (duracion.TotalMinutes < 15)
                {
                    throw new Exception(
                        $"El rango {FormatearHora(horario.horaInicio)} - " +
                        $"{FormatearHora(horario.horaFin)} debe tener una " +
                        "duración mínima de 15 minutos."
                    );
                }
            }
        }

        private void ValidarTraslapesDentroDelFormulario(
            List<HorarioSemanalDetalleCrearDto> horarios
        )
        {
            for (int i = 0; i < horarios.Count; i++)
            {
                for (int j = i + 1; j < horarios.Count; j++)
                {
                    bool seTraslapan =
                        horarios[i].horaInicio <
                            horarios[j].horaFin
                        &&
                        horarios[j].horaInicio <
                            horarios[i].horaFin;

                    if (seTraslapan)
                    {
                        throw new Exception(
                            "Los horarios " +
                            $"{FormatearHora(horarios[i].horaInicio)} - " +
                            $"{FormatearHora(horarios[i].horaFin)} y " +
                            $"{FormatearHora(horarios[j].horaInicio)} - " +
                            $"{FormatearHora(horarios[j].horaFin)} " +
                            "se traslapan."
                        );
                    }
                }
            }
        }

        private void ValidarTraslapesConHorariosExistentes(
            HorarioSemanalMultipleCrearDto modelo,
            List<HorarioSemanalDetalleCrearDto> nuevosHorarios
        )
        {
            var horariosExistentes =
                _obtenerHorariosSemanalesAD
                    .ObtenerHorariosSemanales()
                    .Where(h =>
                        h.estado &&
                        h.diaSemana == modelo.diaSemana &&
                        h.idUsuarioEntrenador ==
                            modelo.idUsuarioEntrenador)
                    .ToList();

            foreach (var nuevo in nuevosHorarios)
            {
                foreach (var existente in horariosExistentes)
                {
                    bool seTraslapan =
                        nuevo.horaInicio < existente.horaFin
                        &&
                        existente.horaInicio < nuevo.horaFin;

                    if (seTraslapan)
                    {
                        throw new Exception(
                            "El entrenador ya posee un horario que se " +
                            "traslapa con el rango " +
                            $"{FormatearHora(nuevo.horaInicio)} - " +
                            $"{FormatearHora(nuevo.horaFin)}. " +
                            "Horario existente: " +
                            $"{FormatearHora(existente.horaInicio)} - " +
                            $"{FormatearHora(existente.horaFin)}."
                        );
                    }
                }
            }
        }

        private string FormatearHora(TimeSpan hora)
        {
            DateTime fechaTemporal =
                DateTime.Today.Add(hora);

            return fechaTemporal.ToString("hh:mm tt");
        }
    }
}