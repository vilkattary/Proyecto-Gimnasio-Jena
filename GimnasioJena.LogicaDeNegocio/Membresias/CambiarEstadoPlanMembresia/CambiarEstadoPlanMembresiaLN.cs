using GimnasioJena.Abstracciones.AccesoADatos.Membresias.CambiarEstadoPlanMembresia;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.CambiarEstadoPlanMembresia;
using GimnasioJena.Abstracciones.Modelos.Membresias;
using GimnasioJena.AccesoADatos.Membresias.CambiarEstadoPlanMembresia;

namespace GimnasioJena.LogicaDeNegocio.Membresias.CambiarEstadoPlanMembresia
{
    public class CambiarEstadoPlanMembresiaLN : ICambiarEstadoPlanMembresiaLN
    {
        private readonly ICambiarEstadoPlanMembresiaAD
            _cambiarEstadoPlanMembresiaAD;

        public CambiarEstadoPlanMembresiaLN()
        {
            _cambiarEstadoPlanMembresiaAD =
                new CambiarEstadoPlanMembresiaAD();
        }

        public bool CambiarEstadoPlanMembresia(CambiarEstadoPlanMembresiaDto modelo)
        {
            if (modelo == null)
            {
                return false;
            }

            if (modelo.idPlanMembresia <= 0)
            {
                return false;
            }

            return _cambiarEstadoPlanMembresiaAD
                .CambiarEstadoPlanMembresia(modelo);
        }
    }
}
