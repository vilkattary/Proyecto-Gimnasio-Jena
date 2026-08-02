using GimnasioJena.Abstracciones.AccesoADatos.Membresias.ObtenerPlanesMembresia;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.ObtenerPlanesMembresia;
using GimnasioJena.Abstracciones.Modelos.Membresias;
using GimnasioJena.AccesoADatos.Membresias.ObtenerPlanesMembresia;
using System.Collections.Generic;
using System.Linq;

namespace GimnasioJena.LogicaDeNegocio.Membresias.ObtenerPlanesMembresia
{
    public class ObtenerPlanesMembresiaLN : IObtenerPlanesMembresiaLN
    {
        private readonly IObtenerPlanesMembresiaAD
            _obtenerPlanesMembresiaAD;

        public ObtenerPlanesMembresiaLN()
        {
            _obtenerPlanesMembresiaAD =
                new ObtenerPlanesMembresiaAD();
        }

        public List<PlanMembresiaListadoDto> ObtenerTodosLosPlanes()
        {
            return _obtenerPlanesMembresiaAD
                .ObtenerPlanesMembresia();
        }

        public List<PlanMembresiaListadoDto> ObtenerPlanesActivos()
        {
            return _obtenerPlanesMembresiaAD
                .ObtenerPlanesMembresia()
                .Where(p => p.estado)
                .ToList();
        }
    }
}