using GimnasioJena.Abstracciones.Modelos.Membresias;

namespace GimnasioJena.Abstracciones.AccesoADatos.Membresias.RegistrarPlanMembresia
{
    public interface IRegistrarPlanMembresiaAD
    {
        int ContarPlanes();

        bool RegistrarPlanMembresia(RegistrarPlanMembresiaDto modelo);
    }
}
