using GimnasioJena.Abstracciones.AccesoADatos.Reservas.CancelarReserva;
using GimnasioJena.Abstracciones.Modelos.Reservas;
using System;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Reservas.CancelarReserva
{
    public class CancelarReservaAD : ICancelarReservaAD
    {
        private readonly Contexto _contexto;

        public CancelarReservaAD()
        {
            _contexto = new Contexto();
        }

        public bool CancelarReserva(ReservaCancelarDto reservaCancelar, int idUsuario)
        {
            var reserva = _contexto.Reservas
                .FirstOrDefault(r =>
                    r.idReserva == reservaCancelar.idReserva &&
                    r.idUsuario == idUsuario);

            if (reserva == null)
                return false;

            if (reserva.idEstadoReserva != 1)
                return false;

            var clase = _contexto.Clases
                .FirstOrDefault(c => c.idClaseProgramada == reserva.idClaseProgramada);

            if (clase == null)
                return false;

            DateTime fechaHoraClase = clase.fechaClase.Date.Add(clase.horaInicio);

            if (fechaHoraClase <= DateTime.Now)
                return false;

            var hoy = DateTime.Today;

            var membresia = _contexto.Membresias
                .Where(m =>
                    m.idUsuario == idUsuario &&
                    m.idEstadoMembresia == 1 &&
                    m.fechaInicio <= hoy &&
                    m.fechaFin >= hoy)
                .OrderByDescending(m => m.fechaFin)
                .FirstOrDefault();

            reserva.idEstadoReserva = 2;

            if (!string.IsNullOrWhiteSpace(reservaCancelar.motivoCancelacion))
            {
                reserva.observaciones = "Cancelada por el cliente. Motivo: " + reservaCancelar.motivoCancelacion;
            }
            else
            {
                reserva.observaciones = "Cancelada por el cliente.";
            }

            if (membresia != null && membresia.clasesDisponibles.HasValue)
            {
                membresia.clasesDisponibles = membresia.clasesDisponibles.Value + 1;
            }

            return _contexto.SaveChanges() > 0;
        }
    }
}