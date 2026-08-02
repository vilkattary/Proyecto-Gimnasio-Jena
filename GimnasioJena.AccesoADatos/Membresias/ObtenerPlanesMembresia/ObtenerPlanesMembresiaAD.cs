using GimnasioJena.Abstracciones.AccesoADatos.Membresias.ObtenerPlanesMembresia;
using GimnasioJena.Abstracciones.Modelos.Membresias;
using System.Collections.Generic;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Membresias.ObtenerPlanesMembresia
{
    public class ObtenerPlanesMembresiaAD : IObtenerPlanesMembresiaAD
    {
        private readonly Contexto _contexto;

        public ObtenerPlanesMembresiaAD()
        {
            _contexto = new Contexto();
        }

        public List<PlanMembresiaListadoDto> ObtenerPlanesMembresia()
        {
            return _contexto.PlanesMembresia
                .OrderBy(p => p.nombrePlan)
                .Select(p => new PlanMembresiaListadoDto
                {
                    idPlanMembresia =
                        p.idPlanMembresia,

                    nombrePlan =
                        p.nombrePlan,

                    cantidadClases =
                        p.cantidadClases,

                    duracionDias =
                        p.duracionDias,

                    precio =
                        p.precio,

                    estado =
                        p.estado
                })
                .ToList();
        }
    }
}