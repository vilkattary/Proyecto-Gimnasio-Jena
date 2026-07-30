using GimnasioJena.Abstracciones.Modelos.Membresias;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.EditarPrecioPlan
{
    public interface IEditarPrecioPlanLN
    {
        bool EditarPrecioPlan(EditarPrecioPlanDto modelo);
    }
}