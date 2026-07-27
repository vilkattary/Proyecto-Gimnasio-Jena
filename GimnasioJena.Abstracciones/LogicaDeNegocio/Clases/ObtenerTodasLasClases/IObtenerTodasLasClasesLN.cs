using GimnasioJena.Abstracciones.Modelos.Clases;
using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Clases.ObtenerTodasLasClases
{
    public interface IObtenerTodasLasClasesLN
    {
        List<ClaseListadoDto> ObtenerTodasLasClases();

        List<ClaseListadoDto> ObtenerProximasClasesParaCliente();
    }
}