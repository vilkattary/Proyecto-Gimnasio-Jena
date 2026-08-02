using GimnasioJena.Abstracciones.Modelos.Membresias;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.ObtenerPlanMembresiaPorId
{
    public interface IObtenerPlanMembresiaPorIdLN
    {
        PlanMembresiaDatosDto ObtenerPlanMembresiaPorId(
            int idPlanMembresia);
    }
}