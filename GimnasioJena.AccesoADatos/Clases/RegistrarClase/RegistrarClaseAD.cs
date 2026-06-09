using GimnasioJena.Abstracciones.AccesoADatos.Clases.RegistrarClase;
using GimnasioJena.Abstracciones.Modelos.Clases;
using GimnasioJena.AccesoADatos.Entidades.Clases;

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
            return _elContexto.SaveChanges();
        }

        private ClaseEntidad ConvertirObjetoEntidad(ClaseCrearDto laClase)
        {
            return new ClaseEntidad
            {
                idTipoClase = laClase.idTipoClase,
                idUsuarioEntrenador = laClase.idUsuarioEntrenador,
                idEstadoClase = laClase.idEstadoClase,
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