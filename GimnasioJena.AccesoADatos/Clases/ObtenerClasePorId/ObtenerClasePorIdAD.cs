using GimnasioJena.Abstracciones.AccesoADatos.Clases.ObtenerClasePorId;
using GimnasioJena.Abstracciones.Modelos.Clases;
using System;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Clases.ObtenerClasePorId
{
    public class ObtenerClasePorIdAD : IObtenerClasePorIdAD
    {
        private Contexto _elContexto;

        public ObtenerClasePorIdAD()
        {
            _elContexto = new Contexto();
        }

        public ClaseListadoDto ObtenerClasePorId(int id)
        {
            var laClaseEnBaseDeDatos =
                (from clase in _elContexto.Clases
                 join tipo in _elContexto.TiposClase
                    on clase.idTipoClase equals tipo.idTipoClase
                 join entrenadorUsuario in _elContexto.Usuarios
                    on clase.idUsuarioEntrenador equals entrenadorUsuario.idUsuario
                 join estado in _elContexto.EstadoClases
                    on clase.idEstadoClase equals estado.idEstadoClase
                 where clase.idClaseProgramada == id
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
                 }).FirstOrDefault();

            return laClaseEnBaseDeDatos;
        }
    }
}