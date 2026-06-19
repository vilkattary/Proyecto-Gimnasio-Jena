using GimnasioJena.Abstracciones.Modelos.Clases;
using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.AccesoADatos.Clases.ObtenerClasesPorEntrenador
{
    public interface IObtenerClasesPorEntrenadorAD
    {
        List<ClaseEntrenadorDto> ObtenerClasesPorEntrenador(string identityUserId);
    }
}