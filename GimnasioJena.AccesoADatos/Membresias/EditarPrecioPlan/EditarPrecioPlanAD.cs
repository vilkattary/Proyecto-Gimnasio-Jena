using GimnasioJena.Abstracciones.AccesoADatos.Membresias.EditarPrecioPlan;
using GimnasioJena.Abstracciones.Modelos.Membresias;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Membresias.EditarPrecioPlan
{
    public class EditarPrecioPlanAD : IEditarPrecioPlanAD
    {
        private readonly Contexto _contexto;

        public EditarPrecioPlanAD()
        {
            _contexto = new Contexto();
        }

        public bool EditarPrecioPlan(EditarPrecioPlanDto modelo)
        {
            var plan =
                _contexto.PlanesMembresia
                    .FirstOrDefault(
                        p => p.idPlanMembresia ==
                             modelo.idPlanMembresia);

            if (plan == null)
            {
                return false;
            }

            plan.nombrePlan = modelo.nombrePlan;
            plan.precio = modelo.precio;
            plan.duracionDias = modelo.duracionDias;
            plan.cantidadClases = modelo.cantidadClases;

            _contexto.SaveChanges();

            return true;
        }
    }
}