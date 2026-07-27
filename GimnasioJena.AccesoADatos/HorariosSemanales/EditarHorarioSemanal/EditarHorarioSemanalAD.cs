using GimnasioJena.Abstracciones.AccesoADatos
    .HorariosSemanales.EditarHorarioSemanal;
using GimnasioJena.Abstracciones.Modelos.HorariosSemanales;
using GimnasioJena.AccesoADatos.Entidades.HorariosSemanales;
using System;

namespace GimnasioJena.AccesoADatos.HorariosSemanales
    .EditarHorarioSemanal
{
    public class EditarHorarioSemanalAD
        : IEditarHorarioSemanalAD
    {
        private readonly Contexto _contexto;

        public EditarHorarioSemanalAD()
        {
            _contexto = new Contexto();
        }

        public void EditarHorarioSemanal(
            HorarioSemanalEditarDto modelo
        )
        {
            HorarioSemanalEntidad horario =
                _contexto.HorariosSemanales
                    .Find(modelo.idHorario);

            if (horario == null)
            {
                throw new Exception(
                    "No se encontró el horario semanal."
                );
            }

            horario.idTipoClase =
                modelo.idTipoClase;

            horario.idUsuarioEntrenador =
                modelo.idUsuarioEntrenador;

            horario.diaSemana =
                modelo.diaSemana;

            horario.horaInicio =
                modelo.horaInicio;

            horario.horaFin =
                modelo.horaFin;

            horario.cupoMaximo =
                modelo.cupoMaximo;

            horario.ubicacion =
                "Salón principal";

            horario.estado =
                modelo.estado;

            horario.fechaModificacion =
                DateTime.Now;

            _contexto.SaveChanges();
        }
    }
}