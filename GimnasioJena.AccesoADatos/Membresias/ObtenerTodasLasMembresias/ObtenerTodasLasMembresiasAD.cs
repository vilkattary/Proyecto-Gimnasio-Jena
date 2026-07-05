using GimnasioJena.Abstracciones.AccesoADatos.Membresias.ObtenerTodasLasMembresias;
using GimnasioJena.Abstracciones.Modelos.Membresias;
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
                     nombreCliente = u.nombre + " " + u.apellido1 + " " + u.apellido2,
                     nombrePlan = p.nombrePlan,
                     estadoMembresia = e.nombreEstado,
                     fechaInicio = m.fechaInicio,
                     fechaFin = m.fechaFin,
                     clasesDisponibles = m.clasesDisponibles,
                     precio = p.precio
                 })
                .OrderByDescending(m => m.fechaFin)
                .ToList();

            return membresias;
        }
    }
}