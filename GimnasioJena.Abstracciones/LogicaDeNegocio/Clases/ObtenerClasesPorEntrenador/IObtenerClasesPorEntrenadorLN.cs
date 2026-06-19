using GimnasioJena.Abstracciones.Modelos.Clases;
using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Clases.ObtenerClasesPorEntrenador
{
    public interface IObtenerClasesPorEntrenadorLN
    {
        List<ClaseEntrenadorDto> ObtenerClasesPorEntrenador(string identityUserId);
    }
}