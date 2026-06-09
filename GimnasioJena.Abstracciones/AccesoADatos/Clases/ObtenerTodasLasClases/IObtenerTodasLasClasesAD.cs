using GimnasioJena.Abstracciones.Modelos.Clases;
using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.AccesoADatos.Clases.ObtenerTodasLasClases
{
    public interface IObtenerTodasLasClasesAD
    {
        List<ClaseListadoDto> ObtenerTodasLasClases();
    }
}
