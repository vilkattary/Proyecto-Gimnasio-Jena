using GimnasioJena.Abstracciones.AccesoADatos.Membresias.RenovarMembresia;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.RenovarMembresia;
using GimnasioJena.Abstracciones.Modelos.Membresias;
using GimnasioJena.AccesoADatos.Membresias.RenovarMembresia;

namespace GimnasioJena.LogicaDeNegocio.Membresias.RenovarMembresia
{
    public class RenovarMembresiaLN : IRenovarMembresiaLN
    {
        private readonly IRenovarMembresiaAD _renovarMembresiaAD;

        public RenovarMembresiaLN()
        {
            _renovarMembresiaAD = new RenovarMembresiaAD();
        }

        public bool RenovarMembresia(MembresiaRenovarDto modelo)
        {
            if (modelo == null)
                return false;

            if (modelo.idMembresiaCliente <= 0)
                return false;

            return _renovarMembresiaAD.RenovarMembresia(modelo);
        }
    }
}