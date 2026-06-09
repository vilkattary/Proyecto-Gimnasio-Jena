using GimnasioJena.Abstracciones.AccesoADatos.Clases.ObtenerTodasLasClases;
using GimnasioJena.Abstracciones.Modelos.Clases;
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
                var listaDeClases =
                    (from clase in _elContexto.Clases
                     join tipo in _elContexto.TiposClase
                        on clase.idTipoClase equals tipo.idTipoClase
                     join entrenadorUsuario in _elContexto.Usuarios
                        on clase.idUsuarioEntrenador equals entrenadorUsuario.idUsuario
                     join estado in _elContexto.EstadoClases
                        on clase.idEstadoClase equals estado.idEstadoClase
                     select new ClaseListadoDto
                     {
                         idClaseProgramada = clase.idClaseProgramada,
                         nombreClase = tipo.nombreClase,
                         nombreEntrenador = entrenadorUsuario.nombre + " " + entrenadorUsuario.apellido1 + " " + entrenadorUsuario.apellido2,
                         estadoClase = estado.nombreEstado,
                         fechaClase = clase.fechaClase,
                         horaInicio = clase.horaInicio,
                         horaFin = clase.horaFin,
                         cupoMaximo = clase.cupoMaximo,
                         cuposReservados = _elContexto.Reservas.Count(r => r.idClaseProgramada == clase.idClaseProgramada),
                         cuposDisponibles = clase.cupoMaximo - _elContexto.Reservas.Count(r => r.idClaseProgramada == clase.idClaseProgramada),
                         ubicacion = clase.ubicacion
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