using GimnasioJena.Abstracciones.AccesoADatos
    .HorariosSemanales.EditarHorarioSemanal;
using GimnasioJena.Abstracciones.AccesoADatos
    .HorariosSemanales.ObtenerHorariosSemanales;
using GimnasioJena.Abstracciones.LogicaDeNegocio
    .HorariosSemanales.EditarHorarioSemanal;
using GimnasioJena.Abstracciones.Modelos.HorariosSemanales;
using GimnasioJena.AccesoADatos.HorariosSemanales
    .EditarHorarioSemanal;
using GimnasioJena.AccesoADatos.HorariosSemanales
    .ObtenerHorariosSemanales;
using System;
using System.Linq;

namespace GimnasioJena.LogicaDeNegocio.HorariosSemanales
    .EditarHorarioSemanal
{
    public class EditarHorarioSemanalLN
        : IEditarHorarioSemanalLN
    {
        private readonly IEditarHorarioSemanalAD
            _editarHorarioSemanalAD;

        private readonly IObtenerHorariosSemanalesAD
            _obtenerHorariosSemanalesAD;

        public EditarHorarioSemanalLN()
        {
            _editarHorarioSemanalAD =
                new EditarHorarioSemanalAD();

            _obtenerHorariosSemanalesAD =
                new ObtenerHorariosSemanalesAD();
        }

        public void EditarHorarioSemanal(
            HorarioSemanalEditarDto modelo
        )
        {
            if (modelo == null)
            {
                throw new ArgumentException(
                    "No se recibieron los datos del horario semanal."
                );
            }

            if (modelo.horaFin <= modelo.horaInicio)
            {
                throw new ArgumentException(
                    "La hora final debe ser mayor que la hora inicial."
                );
            }

            // Solo validamos traslapes si el horario estará activo.
            if (modelo.estado)
            {
                ValidarTraslapes(modelo);
            }

            _editarHorarioSemanalAD
                .EditarHorarioSemanal(modelo);
        }

        private void ValidarTraslapes(
            HorarioSemanalEditarDto modelo
        )
        {
            var horariosExistentes =
                _obtenerHorariosSemanalesAD
                    .ObtenerHorariosSemanales()
                    .Where(h =>
                        h.estado &&
                        h.idHorario != modelo.idHorario &&
                        h.diaSemana == modelo.diaSemana &&
                        h.idUsuarioEntrenador ==
                            modelo.idUsuarioEntrenador)
                    .ToList();

            foreach (var existente in horariosExistentes)
            {
                bool seTraslapan =
                    modelo.horaInicio < existente.horaFin &&
                    existente.horaInicio < modelo.horaFin;

                if (seTraslapan)
                {
                    throw new ArgumentException(
                        "El entrenador ya tiene otro horario " +
                        "recurrente que coincide con el horario " +
                        "seleccionado. Debe elegir una hora diferente."
                    );
                }
            }
        }
    }
}