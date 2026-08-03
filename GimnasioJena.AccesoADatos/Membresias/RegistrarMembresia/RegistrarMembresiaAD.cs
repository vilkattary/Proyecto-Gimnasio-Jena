using GimnasioJena.Abstracciones.AccesoADatos.Membresias.RegistrarMembresia;
using GimnasioJena.Abstracciones.Modelos.Membresias;
using GimnasioJena.AccesoADatos.Entidades.Membresias;
using System;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Membresias.RegistrarMembresia
{
    public class RegistrarMembresiaAD : IRegistrarMembresiaAD
    {
        private const int ESTADO_MEMBRESIA_NO_ACTIVA = 2;

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

        public int RegistrarMembresiaPendiente(
            int idUsuario,
            int idPlanMembresia)
        {
            if (idUsuario <= 0 || idPlanMembresia <= 0)
                return 0;

            var plan = ObtenerDatosPlan(idPlanMembresia);

            if (plan == null)
                return 0;

            /*
             * Se crea la membresía en estado NO activo (vencido),
             * con vigencia en el pasado, para que el registro del pago
             * confirmado sea quien la active/renueve (mismo flujo del admin).
             */
            DateTime ayer = DateTime.Today.AddDays(-1);

            var membresiaAGuardar = new MembresiaEntidad
            {
                idUsuario = idUsuario,
                idPlanMembresia = idPlanMembresia,
                idEstadoMembresia = ESTADO_MEMBRESIA_NO_ACTIVA,
                fechaInicio = ayer,
                fechaFin = ayer,
                clasesDisponibles = 0,
                observaciones =
                    "Membresía creada pendiente de pago (Tilopay).",
                fechaCreacion = DateTime.Now
            };

            _contexto.Membresias.Add(membresiaAGuardar);
            _contexto.SaveChanges();

            return membresiaAGuardar.idMembresiaCliente;
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

        public PlanMembresiaDatosDto ObtenerDatosPlan(int idPlanMembresia)
        {
            return _contexto.PlanesMembresia
                .Where(p =>
                    p.idPlanMembresia == idPlanMembresia &&
                    p.estado)
                .Select(p => new PlanMembresiaDatosDto
                {
                    idPlanMembresia = p.idPlanMembresia,
                    cantidadClases = p.cantidadClases,
                    duracionDias = p.duracionDias,
                    precio = p.precio,
                    estado = p.estado
                })
                .FirstOrDefault();
        }
    }
}