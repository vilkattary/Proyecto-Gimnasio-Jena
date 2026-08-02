using GimnasioJena.Abstracciones.Modelos.Membresias;

namespace GimnasioJena.Abstracciones.AccesoADatos.Membresias.ObtenerPlanMembresiaPorId
{
    public interface IObtenerPlanMembresiaPorIdAD
    {
        PlanMembresiaDatosDto ObtenerPlanMembresiaPorId(
            int idPlanMembresia);
    }
}