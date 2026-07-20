using GimnasioJena.Abstracciones.Modelos.Membresias;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.EditarMembresia
{
    public interface IEditarMembresiaLN
    {
        bool EditarMembresia(MembresiaEditarDto membresia);
    }
}