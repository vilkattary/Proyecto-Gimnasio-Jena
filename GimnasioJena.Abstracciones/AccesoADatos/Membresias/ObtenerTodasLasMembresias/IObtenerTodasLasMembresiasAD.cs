using GimnasioJena.Abstracciones.Modelos.Membresias;
using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.AccesoADatos.Membresias.ObtenerTodasLasMembresias
{
    public interface IObtenerTodasLasMembresiasAD
    {
        List<MembresiaListadoDto> ObtenerTodasLasMembresias();
    }
}