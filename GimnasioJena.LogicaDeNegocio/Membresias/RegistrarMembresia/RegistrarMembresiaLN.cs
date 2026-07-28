using GimnasioJena.Abstracciones.AccesoADatos.Membresias.RegistrarMembresia;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.RegistrarMembresia;
using GimnasioJena.Abstracciones.Modelos.Membresias;
using GimnasioJena.AccesoADatos.Membresias.RegistrarMembresia;
using System;

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

            if (membresia.fechaInicio == default(DateTime))
                return false;

            if (membresia.idEstadoMembresia <= 0)
                membresia.idEstadoMembresia = 1;

            var plan = _registrarMembresiaAD
                .ObtenerDatosPlan(membresia.idPlanMembresia);

            if (plan == null)
                return false;

            if (plan.duracionDias <= 0)
                return false;

            bool yaTieneActiva =
                _registrarMembresiaAD
                    .UsuarioTieneMembresiaActiva(membresia.idUsuario);

            if (yaTieneActiva)
                return false;

            membresia.fechaFin =
                membresia.fechaInicio
                    .AddDays(plan.duracionDias - 1);

            membresia.clasesDisponibles =
                plan.cantidadClases;

            return _registrarMembresiaAD
                .RegistrarMembresia(membresia) > 0;
        }
    }
}