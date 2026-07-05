using GimnasioJena.Abstracciones.Modelos.Membresias;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.RenovarMembresia
{
    public interface IRenovarMembresiaLN
    {
        bool RenovarMembresia(MembresiaRenovarDto modelo);
    }
}