using GimnasioJena.Abstracciones.AccesoADatos.Clases.RegistrarClase;
using GimnasioJena.Abstracciones.Modelos.Clases;
using GimnasioJena.AccesoADatos.Entidades.Clases;
using System;

namespace GimnasioJena.AccesoADatos.Clases.RegistrarClase
{
    public class RegistrarClaseAD : IRegistrarClaseAD
    {
        private Contexto _elContexto;

        public RegistrarClaseAD()
        {
            _elContexto = new Contexto();
        }

        public int RegistrarClase(ClaseCrearDto laClase)
        {
            ClaseEntidad laClaseAGuardar = ConvertirObjetoEntidad(laClase);
            _elContexto.Clases.Add(laClaseAGuardar);
            int cantidadDeRegistrosAlmacenados = _elContexto.SaveChanges();
            return cantidadDeRegistrosAlmacenados;
        }

        private ClaseEntidad ConvertirObjetoEntidad(ClaseCrearDto laClase)
        {
            return new ClaseEntidad
            {
               
                nombreClase = laClase.nombreClase,
                tipoClase = laClase.tipoClase,
                nombreEntrenador = laClase.nombreEntrenador,
                estadoClase = laClase.estadoClase,
                fechaClase = laClase.fechaClase,
                horaInicio = laClase.horaInicio,
                horaFin = laClase.horaFin,
                cupoMaximo = laClase.cupoMaximo,
                ubicacion = laClase.ubicacion,
                observaciones = laClase.observaciones,
                fechaCreacion = laClase.fechaCreacion
            };
        }
    }
}
