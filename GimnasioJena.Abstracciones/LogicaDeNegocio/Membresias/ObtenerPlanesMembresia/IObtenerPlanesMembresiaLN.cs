using GimnasioJena.Abstracciones.Modelos.Membresias;
using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.ObtenerPlanesMembresia
{
    public interface IObtenerPlanesMembresiaLN
    {
        List<PlanMembresiaListadoDto> ObtenerTodosLosPlanes();

        List<PlanMembresiaListadoDto> ObtenerPlanesActivos();
    }
}