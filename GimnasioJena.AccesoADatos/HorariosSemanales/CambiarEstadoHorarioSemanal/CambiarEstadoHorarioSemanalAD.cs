using GimnasioJena.Abstracciones.AccesoADatos
    .HorariosSemanales.CambiarEstadoHorarioSemanal;
using GimnasioJena.AccesoADatos.Entidades.HorariosSemanales;
using System;

namespace GimnasioJena.AccesoADatos.HorariosSemanales
    .CambiarEstadoHorarioSemanal
{
    public class CambiarEstadoHorarioSemanalAD
        : ICambiarEstadoHorarioSemanalAD
    {
        private readonly Contexto _contexto;

        public CambiarEstadoHorarioSemanalAD()
        {
            _contexto = new Contexto();
        }

        public bool CambiarEstadoHorarioSemanal(
            int idHorario
        )
        {
            HorarioSemanalEntidad horario =
                _contexto.HorariosSemanales
                    .Find(idHorario);

            if (horario == null)
            {
                throw new Exception(
                    "No se encontró el horario semanal."
                );
            }

            horario.estado =
                !horario.estado;

            horario.fechaModificacion =
                DateTime.Now;

            _contexto.SaveChanges();

            return horario.estado;
        }
    }
}