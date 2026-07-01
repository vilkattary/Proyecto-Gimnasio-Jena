using GimnasioJena.Abstracciones.Modelos.Membresias;

namespace GimnasioJena.Abstracciones.AccesoADatos.Membresias.EditarMembresia
{
    public interface IEditarMembresiaAD
    {
        int EditarMembresia(MembresiaEditarDto membresia);
    }
}