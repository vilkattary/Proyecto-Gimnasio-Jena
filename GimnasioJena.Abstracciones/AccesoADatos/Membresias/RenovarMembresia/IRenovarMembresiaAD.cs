using GimnasioJena.Abstracciones.Modelos.Membresias;

namespace GimnasioJena.Abstracciones.AccesoADatos.Membresias.RenovarMembresia
{
    public interface IRenovarMembresiaAD
    {
        bool RenovarMembresia(MembresiaRenovarDto modelo);
    }
}