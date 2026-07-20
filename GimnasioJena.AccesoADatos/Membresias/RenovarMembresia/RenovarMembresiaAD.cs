using GimnasioJena.Abstracciones.AccesoADatos.Membresias.RenovarMembresia;
using GimnasioJena.Abstracciones.Modelos.Membresias;
using System;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Membresias.RenovarMembresia
{
    public class RenovarMembresiaAD : IRenovarMembresiaAD
    {
        private readonly Contexto _contexto;

        public RenovarMembresiaAD()
        {
            _contexto = new Contexto();
        }

        public bool RenovarMembresia(MembresiaRenovarDto modelo)
        {
            var membresia = _contexto.Membresias
                .FirstOrDefault(m => m.idMembresiaCliente == modelo.idMembresiaCliente);

            if (membresia == null)
                return false;

            var plan = _contexto.PlanesMembresia
                .FirstOrDefault(p => p.idPlanMembresia == membresia.idPlanMembresia);

            if (plan == null)
                return false;

            membresia.fechaInicio = DateTime.Today;

            membresia.fechaFin = DateTime.Today.AddDays(plan.duracionDias);

            membresia.clasesDisponibles = plan.cantidadClases;

            membresia.idEstadoMembresia = 1;

            membresia.observaciones =
                "Membresía renovada el " +
                DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            return _contexto.SaveChanges() > 0;
        }
    }
}