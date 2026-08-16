using GimnasioJena.Abstracciones.Modelos.Membresias;

namespace GimnasioJena.Abstracciones.AccesoADatos.Membresias.CambiarEstadoPlanMembresia
{
    public interface ICambiarEstadoPlanMembresiaAD
    {
        bool CambiarEstadoPlanMembresia(CambiarEstadoPlanMembresiaDto modelo);
    }
}
