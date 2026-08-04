using GimnasioJena.Abstracciones.Modelos.Membresias;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.CambiarEstadoPlanMembresia
{
    public interface ICambiarEstadoPlanMembresiaLN
    {
        bool CambiarEstadoPlanMembresia(CambiarEstadoPlanMembresiaDto modelo);
    }
}
