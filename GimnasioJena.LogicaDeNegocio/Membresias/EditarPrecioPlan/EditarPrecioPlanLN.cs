using GimnasioJena.Abstracciones.AccesoADatos.Membresias.EditarPrecioPlan;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.EditarPrecioPlan;
using GimnasioJena.Abstracciones.Modelos.Membresias;
using GimnasioJena.AccesoADatos.Membresias.EditarPrecioPlan;

namespace GimnasioJena.LogicaDeNegocio.Membresias.EditarPrecioPlan
{
    public class EditarPrecioPlanLN : IEditarPrecioPlanLN
    {
        private readonly IEditarPrecioPlanAD
            _editarPrecioPlanAD;

        public EditarPrecioPlanLN()
        {
            _editarPrecioPlanAD =
                new EditarPrecioPlanAD();
        }

        public bool EditarPrecioPlan(EditarPrecioPlanDto modelo)
        {
            if (modelo == null)
            {
                return false;
            }

            if (modelo.idPlanMembresia <= 0)
            {
                return false;
            }

            if (modelo.precio <= 0)
            {
                return false;
            }

            return _editarPrecioPlanAD
                .EditarPrecioPlan(modelo);
        }
    }
}