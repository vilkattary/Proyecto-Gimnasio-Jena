using GimnasioJena.Abstracciones.AccesoADatos.Clases.ObtenerClasesPorEntrenador;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Clases.ObtenerClasesPorEntrenador;
using GimnasioJena.Abstracciones.Modelos.Clases;
using GimnasioJena.AccesoADatos.Clases.ObtenerClasesPorEntrenador;
using System.Collections.Generic;

namespace GimnasioJena.LogicaDeNegocio.Clases.ObtenerClasesPorEntrenador
{
    public class ObtenerClasesPorEntrenadorLN : IObtenerClasesPorEntrenadorLN
    {
        private readonly IObtenerClasesPorEntrenadorAD _obtenerClasesPorEntrenadorAD;

        public ObtenerClasesPorEntrenadorLN()
        {
            _obtenerClasesPorEntrenadorAD = new ObtenerClasesPorEntrenadorAD();
        }

        public List<ClaseEntrenadorDto> ObtenerClasesPorEntrenador(string identityUserId)
        {
            if (string.IsNullOrWhiteSpace(identityUserId))
                return new List<ClaseEntrenadorDto>();

            return _obtenerClasesPorEntrenadorAD
                .ObtenerClasesPorEntrenador(identityUserId);
        }
    }
}