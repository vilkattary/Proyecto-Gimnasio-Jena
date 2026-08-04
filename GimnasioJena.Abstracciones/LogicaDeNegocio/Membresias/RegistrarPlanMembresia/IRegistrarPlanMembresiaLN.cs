using GimnasioJena.Abstracciones.Modelos.Membresias;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.RegistrarPlanMembresia
{
    public interface IRegistrarPlanMembresiaLN
    {
        int LimiteMaximoPlanes { get; }

        bool SePuedeRegistrarNuevoPlan();

        bool RegistrarPlanMembresia(RegistrarPlanMembresiaDto modelo);
    }
}
