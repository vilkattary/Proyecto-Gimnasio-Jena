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
                .FirstOrDefault(m =>
                    m.idMembresiaCliente ==
                    modelo.idMembresiaCliente);

            if (membresia == null)
                return false;

            var plan = _contexto.PlanesMembresia
                .FirstOrDefault(p =>
                    p.idPlanMembresia ==
                    membresia.idPlanMembresia);

            if (plan == null)
                return false;

            if (plan.duracionDias <= 0)
                return false;

            DateTime hoy = DateTime.Today;

            /*
             * Solo se permite renovar
             * el día del vencimiento
             * o después.
             */
            if (membresia.fechaFin > hoy)
            {
                return false;
            }

            DateTime nuevaFechaInicio = hoy;

            DateTime nuevaFechaFin =
                nuevaFechaInicio
                    .AddDays(plan.duracionDias - 1);

            membresia.fechaInicio =
                nuevaFechaInicio;

            membresia.fechaFin =
                nuevaFechaFin;

            membresia.clasesDisponibles =
                plan.cantidadClases;

            membresia.idEstadoMembresia = 1;

            string notaRenovacion =
                $"Membresía renovada el {DateTime.Now:dd/MM/yyyy HH:mm}. " +
                $"Nuevo periodo: {nuevaFechaInicio:dd/MM/yyyy} " +
                $"al {nuevaFechaFin:dd/MM/yyyy}.";

            if (string.IsNullOrWhiteSpace(membresia.observaciones))
            {
                membresia.observaciones =
                    notaRenovacion;
            }
            else
            {
                membresia.observaciones =
                    membresia.observaciones.Trim() +
                    Environment.NewLine +
                    Environment.NewLine +
                    notaRenovacion;
            }

            return _contexto.SaveChanges() > 0;
        }
    }
}