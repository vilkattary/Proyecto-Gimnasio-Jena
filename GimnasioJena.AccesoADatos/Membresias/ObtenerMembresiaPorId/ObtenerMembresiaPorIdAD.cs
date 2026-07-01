using GimnasioJena.Abstracciones.AccesoADatos.Membresias.ObtenerMembresiaPorId;
using GimnasioJena.Abstracciones.Modelos.Membresias;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Membresias.ObtenerMembresiaPorId
{
    public class ObtenerMembresiaPorIdAD : IObtenerMembresiaPorIdAD
    {
        private readonly Contexto _contexto;

        public ObtenerMembresiaPorIdAD()
        {
            _contexto = new Contexto();
        }

        public MembresiaEditarDto ObtenerMembresiaPorId(int idMembresiaCliente)
        {
            return _contexto.Membresias
                .Where(m => m.idMembresiaCliente == idMembresiaCliente)
                .Select(m => new MembresiaEditarDto
                {
                    idMembresiaCliente = m.idMembresiaCliente,
                    idUsuario = m.idUsuario,
                    idPlanMembresia = m.idPlanMembresia,
                    idEstadoMembresia = m.idEstadoMembresia,
                    fechaInicio = m.fechaInicio,
                    fechaFin = m.fechaFin,
                    clasesDisponibles = m.clasesDisponibles,
                    observaciones = m.observaciones
                })
                .FirstOrDefault();
        }
        public MembresiaClienteDto ObtenerDetalleMembresiaPorId(int idMembresiaCliente)
        {
            var membresia =
                (from m in _contexto.Membresias
                 join u in _contexto.Usuarios
                    on m.idUsuario equals u.idUsuario
                 join p in _contexto.PlanesMembresia
                    on m.idPlanMembresia equals p.idPlanMembresia
                 join e in _contexto.EstadoMembresias
                    on m.idEstadoMembresia equals e.idEstadoMembresia
                 where m.idMembresiaCliente == idMembresiaCliente
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