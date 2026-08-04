using GimnasioJena.Abstracciones.AccesoADatos.Membresias.RegistrarPlanMembresia;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.RegistrarPlanMembresia;
using GimnasioJena.Abstracciones.Modelos.Membresias;
using GimnasioJena.AccesoADatos.Membresias.RegistrarPlanMembresia;

namespace GimnasioJena.LogicaDeNegocio.Membresias.RegistrarPlanMembresia
{
    public class RegistrarPlanMembresiaLN : IRegistrarPlanMembresiaLN
    {
        private const int LimitePlanes = 5;

        private readonly IRegistrarPlanMembresiaAD
            _registrarPlanMembresiaAD;

        public RegistrarPlanMembresiaLN()
        {
            _registrarPlanMembresiaAD =
                new RegistrarPlanMembresiaAD();
        }

        public int LimiteMaximoPlanes
        {
            get { return LimitePlanes; }
        }

        public bool SePuedeRegistrarNuevoPlan()
        {
            return _registrarPlanMembresiaAD
                .ContarPlanes() < LimitePlanes;
        }

        public bool RegistrarPlanMembresia(RegistrarPlanMembresiaDto modelo)
        {
            if (modelo == null)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(modelo.nombrePlan))
            {
                return false;
            }

            if (modelo.precio <= 0)
            {
                return false;
            }

            if (modelo.duracionDias <= 0)
            {
                return false;
            }

            if (modelo.cantidadClases.HasValue &&
                modelo.cantidadClases.Value < 0)
            {
                return false;
            }

            if (!SePuedeRegistrarNuevoPlan())
            {
                return false;
            }

            return _registrarPlanMembresiaAD
                .RegistrarPlanMembresia(modelo);
        }
    }
}
