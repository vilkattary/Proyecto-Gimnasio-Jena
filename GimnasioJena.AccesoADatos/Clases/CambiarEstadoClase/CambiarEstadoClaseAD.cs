using GimnasioJena.Abstracciones.AccesoADatos.Clases.CambiarEstadoClase;
using GimnasioJena.AccesoADatos.Entidades.Clases;
using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Clases.CambiarEstadoClase
{
    public class CambiarEstadoClaseAD : ICambiarEstadoClaseAD
    {
        private readonly Contexto _contexto;

        public CambiarEstadoClaseAD()
        {
            _contexto = new Contexto();
        }

        public bool CambiarEstadoClase(int idClaseProgramada)
        {
            ClaseEntidad clase =
                _contexto.Clases.Find(idClaseProgramada);

            if (clase == null)
            {
                throw new Exception(
                    "No se encontró la clase."
                );
            }

            int idEstadoActivo =
                _contexto.EstadoClases
                    .Where(e => e.nombreEstado == "Activo")
                    .Select(e => e.idEstadoClase)
                    .FirstOrDefault();

            int idEstadoCancelado =
                _contexto.EstadoClases
                    .Where(e => e.nombreEstado == "Cancelado")
                    .Select(e => e.idEstadoClase)
                    .FirstOrDefault();

            if (idEstadoActivo == 0 || idEstadoCancelado == 0)
            {
                throw new Exception(
                    "No se encontraron los estados 'Activo' y 'Cancelado' en el catálogo."
                );
            }

            bool estabaActiva =
                clase.idEstadoClase == idEstadoActivo;

            clase.idEstadoClase =
                estabaActiva ? idEstadoCancelado : idEstadoActivo;

            clase.fechaModificacion = DateTime.Now;

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
                    "No fue posible guardar el cambio de estado de la clase (validación): " +
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
                    "No fue posible guardar el cambio de estado de la clase: " +
                    masProfunda.Message,
                    ex
                );
            }

            return !estabaActiva;
        }
    }
}
