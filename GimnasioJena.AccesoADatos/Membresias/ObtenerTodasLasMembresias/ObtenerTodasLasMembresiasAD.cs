using GimnasioJena.Abstracciones.AccesoADatos.Membresias.ObtenerTodasLasMembresias;
using GimnasioJena.Abstracciones.Modelos.Membresias;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Membresias.ObtenerTodasLasMembresias
{
    public class ObtenerTodasLasMembresiasAD : IObtenerTodasLasMembresiasAD
    {
        private readonly Contexto _contexto;

        public ObtenerTodasLasMembresiasAD()
        {
            _contexto = new Contexto();
        }

        public List<MembresiaListadoDto> ObtenerTodasLasMembresias()
        {
            DateTime hoy = DateTime.Today;

            var membresias =
                (from m in _contexto.Membresias
                 join u in _contexto.Usuarios
                    on m.idUsuario equals u.idUsuario
                 join p in _contexto.PlanesMembresia
                    on m.idPlanMembresia equals p.idPlanMembresia
                 join e in _contexto.EstadoMembresias
                    on m.idEstadoMembresia equals e.idEstadoMembresia
                 select new MembresiaListadoDto
                 {
                     idMembresiaCliente = m.idMembresiaCliente,

                     idUsuario = m.idUsuario,

                     nombreCliente =
                         u.nombre + " " +
                         u.apellido1 + " " +
                         u.apellido2,

                     nombrePlan = p.nombrePlan,

                     /*
                      * Si está registrada como activa,
                      * pero su fecha final ya pasó,
                      * se muestra como vencida.
                      *
                      * Si vence hoy, continúa activa
                      * durante todo el día.
                      *
                      * Los estados Inactiva y Suspendida
                      * se respetan.
                      */
                     estadoMembresia =
                         m.idEstadoMembresia == 1 &&
                         m.fechaFin < hoy
                             ? "Vencida"
                             : e.nombreEstado,

                     fechaInicio = m.fechaInicio,

                     fechaFin = m.fechaFin,

                     clasesDisponibles =
                         m.clasesDisponibles,

                     precio = p.precio
                 })
                .OrderByDescending(m => m.fechaFin)
                .ToList();

            return membresias;
        }
    }
}