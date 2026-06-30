using GimnasioJena.Abstracciones.AccesoADatos.Membresias.ObtenerMembresiaPorCliente;
using GimnasioJena.Abstracciones.Modelos.Membresias;
using System;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Membresias.ObtenerMembresiaPorCliente
{
    public class ObtenerMembresiaPorClienteAD : IObtenerMembresiaPorClienteAD
    {
        private readonly Contexto _contexto;

        public ObtenerMembresiaPorClienteAD()
        {
            _contexto = new Contexto();
        }

        public MembresiaClienteDto ObtenerMembresiaActivaPorCliente(int idUsuario)
        {
            DateTime hoy = DateTime.Today;

            var membresia =
                (from m in _contexto.Membresias
                 join u in _contexto.Usuarios
                    on m.idUsuario equals u.idUsuario
                 join p in _contexto.PlanesMembresia
                    on m.idPlanMembresia equals p.idPlanMembresia
                 join e in _contexto.EstadoMembresias
                    on m.idEstadoMembresia equals e.idEstadoMembresia
                 where m.idUsuario == idUsuario
                    && m.idEstadoMembresia == 1
                    && m.fechaInicio <= hoy
                    && m.fechaFin >= hoy
                 orderby m.fechaFin descending
                 select new MembresiaClienteDto
                 {
                     idMembresiaCliente = m.idMembresiaCliente,
                     idUsuario = m.idUsuario,
                     nombreCliente = u.nombre + " " + u.apellido1 + " " + u.apellido2,
                     nombrePlan = p.nombrePlan,
                     estadoMembresia = e.nombreEstado,
                     fechaInicio = m.fechaInicio,
                     fechaFin = m.fechaFin,
                     clasesDisponibles = m.clasesDisponibles,
                     observaciones = m.observaciones,
                     precio = p.precio
                 })
                .FirstOrDefault();

            return membresia;
        }
    }
}