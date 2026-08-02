using GimnasioJena.Abstracciones.AccesoADatos.Membresias.ObtenerPlanMembresiaPorId;
using GimnasioJena.Abstracciones.Modelos.Membresias;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Membresias.ObtenerPlanMembresiaPorId
{
    public class ObtenerPlanMembresiaPorIdAD :
        IObtenerPlanMembresiaPorIdAD
    {
        private readonly Contexto _contexto;

        public ObtenerPlanMembresiaPorIdAD()
        {
            _contexto = new Contexto();
        }

        public PlanMembresiaDatosDto ObtenerPlanMembresiaPorId(
            int idPlanMembresia)
        {
            return _contexto.PlanesMembresia
                .Where(p =>
                    p.idPlanMembresia == idPlanMembresia)
                .Select(p => new PlanMembresiaDatosDto
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
                .FirstOrDefault();
        }
    }
}