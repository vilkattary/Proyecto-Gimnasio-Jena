using GimnasioJena.Abstracciones.AccesoADatos.Clases.ObtenerTodasLasClases;
using GimnasioJena.Abstracciones.Modelos.Clases;
using GimnasioJena.AccesoADatos.Entidades.Clases;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Clases.ObtenerTodasLasClases
{
    public class ObtenerTodasLasClasesAD : IObtenerTodasLasClasesAD
    {
        private Contexto _elContexto;

        public ObtenerTodasLasClasesAD()
        {
            _elContexto = new Contexto();
        }

        public List<ClaseListadoDto> ObtenerTodasLasClases()
        {
            try
            {
                List<ClaseListadoDto> listaDeClases = (from clase 
                                                       in _elContexto.Clases
                                                       select new ClaseListadoDto
                                                       {
                                                           idClaseProgramada = clase.idClaseProgramada,
                                                           nombreClase = clase.nombreClase,
                                                           TipoClase = clase.tipoClase,
                                                           nombreEntrenador = clase.nombreEntrenador,
                                                           estadoClase = clase.estadoClase,
                                                           fechaClase = clase.fechaClase,
                                                           horaInicio = clase.horaInicio,
                                                           horaFin = clase.horaFin,
                                                           cupoMaximo = clase.cupoMaximo,
                                                           cuposReservados = 0,
                                                           cuposDisponibles = clase.cupoMaximo,
                                                           ubicacion = clase.ubicacion,
                                                           fechaCreacion = clase.fechaCreacion,
                                                           fechaModificacion = clase.fechaModificacion
                                                       }).ToList();

                return listaDeClases;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las clases: " + ex.Message);
            }
        }
    }
}
