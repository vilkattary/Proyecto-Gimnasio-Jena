using GimnasioJena.Abstracciones.AccesoADatos.Membresias.RegistrarMembresia;
using GimnasioJena.Abstracciones.Modelos.Membresias;
using GimnasioJena.AccesoADatos.Entidades.Membresias;
using System;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Membresias.RegistrarMembresia
{
    public class RegistrarMembresiaAD : IRegistrarMembresiaAD
    {
        private readonly Contexto _contexto;

        public RegistrarMembresiaAD()
        {
            _contexto = new Contexto();
        }

        public int RegistrarMembresia(MembresiaCrearDto membresia)
        {
            var membresiaAGuardar = new MembresiaEntidad
            {
                idUsuario = membresia.idUsuario,
                idPlanMembresia = membresia.idPlanMembresia,
                idEstadoMembresia = membresia.idEstadoMembresia,
                fechaInicio = membresia.fechaInicio,
                fechaFin = membresia.fechaFin,
                clasesDisponibles = membresia.clasesDisponibles,
                observaciones = membresia.observaciones,
                fechaCreacion = DateTime.Now
            };

            _contexto.Membresias.Add(membresiaAGuardar);
            return _contexto.SaveChanges();
        }

        public bool UsuarioTieneMembresiaActiva(int idUsuario)
        {
            DateTime hoy = DateTime.Today;

            return _contexto.Membresias.Any(m =>
                m.idUsuario == idUsuario &&
                m.idEstadoMembresia == 1 &&
                m.fechaInicio <= hoy &&
                m.fechaFin >= hoy);
        }
    }
}