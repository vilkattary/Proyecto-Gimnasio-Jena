using GimnasioJena.Abstracciones.Modelos.Membresias;
using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.AccesoADatos.Membresias.ObtenerPlanesMembresia
{
    public interface IObtenerPlanesMembresiaAD
    {
        List<PlanMembresiaListadoDto> ObtenerPlanesMembresia();
    }
}