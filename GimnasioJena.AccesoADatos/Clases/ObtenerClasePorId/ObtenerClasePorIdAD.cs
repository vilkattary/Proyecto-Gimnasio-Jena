using GimnasioJena.Abstracciones.AccesoADatos.Clases.ObtenerClasePorId;
using GimnasioJena.Abstracciones.Modelos.Clases;
using GimnasioJena.AccesoADatos.Entidades.Clases;
using System;
using System.Collections.Generic;
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
            ClaseListadoDto laClaseEnBaseDeDatos = (from laClase in _elContexto.Clases
                                                    where laClase.idClaseProgramada == id
                                                    select new ClaseListadoDto
                                                    {
                                                        idClaseProgramada = laClase.idClaseProgramada,
                                                        nombreClase = laClase.nombreClase,
                                                        TipoClase = laClase.tipoClase,
                                                        nombreEntrenador = laClase.nombreEntrenador,
                                                        estadoClase = laClase.estadoClase,
                                                        fechaClase = laClase.fechaClase,
                                                        horaInicio = laClase.horaInicio,
                                                        horaFin = laClase.horaFin,
                                                        cupoMaximo = laClase.cupoMaximo,
                                                        cuposReservados = 0, 
                                                        cuposDisponibles = laClase.cupoMaximo, 
                                                        ubicacion = laClase.ubicacion,
                                                        fechaCreacion = laClase.fechaCreacion,
                                                        fechaModificacion = laClase.fechaModificacion
                                                    }).FirstOrDefault();

            return laClaseEnBaseDeDatos;
        }
    }
}
