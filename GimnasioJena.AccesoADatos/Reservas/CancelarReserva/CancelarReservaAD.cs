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

            reserva.idEstadoReserva = 2;

            if (!string.IsNullOrWhiteSpace(reservaCancelar.motivoCancelacion))
            {
                reserva.observaciones = "Cancelada por el cliente. Motivo: " + reservaCancelar.motivoCancelacion;
            }
            else
            {
                reserva.observaciones = "Cancelada por el cliente.";
            }

            return _contexto.SaveChanges() > 0;
        }
    }
}