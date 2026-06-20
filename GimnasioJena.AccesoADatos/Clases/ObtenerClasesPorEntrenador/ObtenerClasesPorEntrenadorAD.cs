using GimnasioJena.Abstracciones.AccesoADatos.Clases.ObtenerClasesPorEntrenador;
using GimnasioJena.Abstracciones.Modelos.Clases;
using System.Collections.Generic;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Clases.ObtenerClasesPorEntrenador
{
    public class ObtenerClasesPorEntrenadorAD : IObtenerClasesPorEntrenadorAD
    {
        private readonly Contexto _elContexto;

        public ObtenerClasesPorEntrenadorAD()
        {
            _elContexto = new Contexto();
        }

        public List<ClaseEntrenadorDto> ObtenerClasesPorEntrenador(string identityUserId)
        {
            var clases =
                (from clase in _elContexto.Clases
                 join usuario in _elContexto.Usuarios
                    on clase.idUsuarioEntrenador equals usuario.idUsuario
                 join tipoClase in _elContexto.TiposClase
                    on clase.idTipoClase equals tipoClase.idTipoClase
                 where usuario.identityUserId == identityUserId
                 select new ClaseEntrenadorDto
                 {
                     idClaseProgramada = clase.idClaseProgramada,
                     idUsuarioEntrenador = clase.idUsuarioEntrenador,

                     nombreClase = tipoClase.nombreClase,

                     fechaClase = clase.fechaClase,
                     horaInicio = clase.horaInicio,
                     horaFin = clase.horaFin,

                     cupoMaximo = clase.cupoMaximo,

                     inscritos =
                        _elContexto.Reservas.Count(r =>
                            r.idClaseProgramada == clase.idClaseProgramada),

                     ubicacion = clase.ubicacion
                 })
                 .OrderBy(c => c.fechaClase)
                 .ThenBy(c => c.horaInicio)
                 .ToList();

            return clases;
        }
    }
}