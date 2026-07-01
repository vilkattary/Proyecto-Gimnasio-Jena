using GimnasioJena.Abstracciones.Modelos.Membresias;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.RegistrarMembresia
{
    public interface IRegistrarMembresiaLN
    {
        bool RegistrarMembresia(MembresiaCrearDto membresia);
    }
}