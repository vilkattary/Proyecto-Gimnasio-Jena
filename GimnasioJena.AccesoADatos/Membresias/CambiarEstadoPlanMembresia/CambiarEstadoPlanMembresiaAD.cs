using GimnasioJena.Abstracciones.AccesoADatos.Membresias.CambiarEstadoPlanMembresia;
using GimnasioJena.Abstracciones.Modelos.Membresias;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Membresias.CambiarEstadoPlanMembresia
{
    public class CambiarEstadoPlanMembresiaAD : ICambiarEstadoPlanMembresiaAD
    {
        private readonly Contexto _contexto;

        public CambiarEstadoPlanMembresiaAD()
        {
            _contexto = new Contexto();
        }

        public bool CambiarEstadoPlanMembresia(CambiarEstadoPlanMembresiaDto modelo)
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

            plan.estado = !plan.estado;

            _contexto.SaveChanges();

            return true;
        }
    }
}
