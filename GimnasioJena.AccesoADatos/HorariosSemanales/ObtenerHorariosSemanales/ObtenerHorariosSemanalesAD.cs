using GimnasioJena.Abstracciones.AccesoADatos.HorariosSemanales.ObtenerHorariosSemanales;
using GimnasioJena.Abstracciones.Modelos.HorariosSemanales;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GimnasioJena.AccesoADatos.HorariosSemanales.ObtenerHorariosSemanales
{
    public class ObtenerHorariosSemanalesAD : IObtenerHorariosSemanalesAD
    {
        private readonly Contexto _elContexto;

        public ObtenerHorariosSemanalesAD()
        {
            _elContexto = new Contexto();
        }

        public List<HorarioSemanalListadoDto> ObtenerHorariosSemanales()
        {
            try
            {
                var horarios =
                    (from horario in _elContexto.HorariosSemanales

                     join tipoClase in _elContexto.TiposClase
                         on horario.idTipoClase equals tipoClase.idTipoClase

                     join entrenador in _elContexto.Usuarios
                         on horario.idUsuarioEntrenador equals entrenador.idUsuario

                     select new HorarioSemanalListadoDto
                     {
                         idHorario = horario.idHorario,

                         idTipoClase = horario.idTipoClase,

                         idUsuarioEntrenador =
                             horario.idUsuarioEntrenador,

                         diaSemana = horario.diaSemana,

                         nombreClase = tipoClase.nombreClase,

                         nombreEntrenador =
                             entrenador.nombre + " " +
                             entrenador.apellido1 + " " +
                             entrenador.apellido2,

                         horaInicio = horario.horaInicio,

                         horaFin = horario.horaFin,

                         cupoMaximo = horario.cupoMaximo,

                         ubicacion = horario.ubicacion,

                         estado = horario.estado,

                         fechaCreacion = horario.fechaCreacion,

                         fechaModificacion =
                             horario.fechaModificacion
                     })
                    .ToList();

                return horarios;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Error al obtener los horarios semanales: " +
                    ex.Message
                );
            }
        }
    }
}