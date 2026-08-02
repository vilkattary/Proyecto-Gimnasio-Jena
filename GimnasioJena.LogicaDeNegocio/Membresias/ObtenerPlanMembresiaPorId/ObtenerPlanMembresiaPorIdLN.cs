using GimnasioJena.Abstracciones.AccesoADatos.Membresias.ObtenerPlanMembresiaPorId;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.ObtenerPlanMembresiaPorId;
using GimnasioJena.Abstracciones.Modelos.Membresias;
using GimnasioJena.AccesoADatos.Membresias.ObtenerPlanMembresiaPorId;

namespace GimnasioJena.LogicaDeNegocio.Membresias.ObtenerPlanMembresiaPorId
{
    public class ObtenerPlanMembresiaPorIdLN :
        IObtenerPlanMembresiaPorIdLN
    {
        private readonly IObtenerPlanMembresiaPorIdAD
            _obtenerPlanMembresiaPorIdAD;

        public ObtenerPlanMembresiaPorIdLN()
        {
            _obtenerPlanMembresiaPorIdAD =
                new ObtenerPlanMembresiaPorIdAD();
        }

        public PlanMembresiaDatosDto ObtenerPlanMembresiaPorId(
            int idPlanMembresia)
        {
            if (idPlanMembresia <= 0)
                return null;

            var plan =
                _obtenerPlanMembresiaPorIdAD
                    .ObtenerPlanMembresiaPorId(idPlanMembresia);

            if (plan == null)
                return null;

            if (!plan.estado)
                return null;

            return plan;
        }
    }
}