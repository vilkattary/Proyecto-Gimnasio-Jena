using GimnasioJena.Abstracciones.AccesoADatos.Membresias.RegistrarMembresia;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.RegistrarMembresia;
using GimnasioJena.Abstracciones.Modelos.Membresias;
using GimnasioJena.AccesoADatos.Membresias.RegistrarMembresia;

namespace GimnasioJena.LogicaDeNegocio.Membresias.RegistrarMembresia
{
    public class RegistrarMembresiaLN : IRegistrarMembresiaLN
    {
        private readonly IRegistrarMembresiaAD _registrarMembresiaAD;

        public RegistrarMembresiaLN()
        {
            _registrarMembresiaAD = new RegistrarMembresiaAD();
        }

        public bool RegistrarMembresia(MembresiaCrearDto membresia)
        {
            if (membresia == null)
                return false;

            if (membresia.idUsuario <= 0)
                return false;

            if (membresia.idPlanMembresia <= 0)
                return false;

            if (membresia.idEstadoMembresia <= 0)
                membresia.idEstadoMembresia = 1;

            if (membresia.fechaInicio == default || membresia.fechaFin == default)
                return false;

            if (membresia.fechaFin < membresia.fechaInicio)
                return false;

            bool yaTieneActiva = _registrarMembresiaAD.UsuarioTieneMembresiaActiva(membresia.idUsuario);

            if (yaTieneActiva)
                return false;

            return _registrarMembresiaAD.RegistrarMembresia(membresia) > 0;
        }
    }
}