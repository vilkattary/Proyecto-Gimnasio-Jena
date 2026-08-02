using GimnasioJena.Abstracciones.AccesoADatos
    .HorariosSemanales.CambiarEstadoHorarioSemanal;
using GimnasioJena.AccesoADatos.Entidades.HorariosSemanales;
using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

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

            try
            {
                _contexto.SaveChanges();
            }
            catch (DbEntityValidationException exValidacion)
            {
                string errores = string.Join(
                    " | ",
                    exValidacion.EntityValidationErrors
                        .SelectMany(e => e.ValidationErrors)
                        .Select(e => e.PropertyName + ": " + e.ErrorMessage)
                );

                throw new Exception(
                    "No fue posible guardar el cambio de estado del horario (validación): " +
                    errores,
                    exValidacion
                );
            }
            catch (DbUpdateException ex)
            {
                Exception masProfunda = ex;

                while (masProfunda.InnerException != null)
                {
                    masProfunda = masProfunda.InnerException;
                }

                throw new Exception(
                    "No fue posible guardar el cambio de estado del horario: " +
                    masProfunda.Message,
                    ex
                );
            }

            return horario.estado;
        }
    }
}