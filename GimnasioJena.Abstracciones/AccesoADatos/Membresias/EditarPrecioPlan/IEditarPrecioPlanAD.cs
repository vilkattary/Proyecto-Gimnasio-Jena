using GimnasioJena.Abstracciones.Modelos.Membresias;

namespace GimnasioJena.Abstracciones.AccesoADatos.Membresias.EditarPrecioPlan
{
    public interface IEditarPrecioPlanAD
    {
        bool EditarPrecioPlan(EditarPrecioPlanDto modelo);
    }
}