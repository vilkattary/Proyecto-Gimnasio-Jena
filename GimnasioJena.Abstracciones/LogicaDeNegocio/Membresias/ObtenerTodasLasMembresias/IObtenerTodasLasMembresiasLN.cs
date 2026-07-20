using GimnasioJena.Abstracciones.Modelos.Membresias;
using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.ObtenerTodasLasMembresias
{
    public interface IObtenerTodasLasMembresiasLN
    {
        List<MembresiaListadoDto> ObtenerTodasLasMembresias();
    }
}