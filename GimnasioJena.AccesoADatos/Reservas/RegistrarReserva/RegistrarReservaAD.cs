using GimnasioJena.Abstracciones.AccesoADatos.Reservas.RegistrarReserva;
using GimnasioJena.Abstracciones.Modelos.Reservas;
using GimnasioJena.AccesoADatos.Entidades.Reservas;
using System;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Reservas.RegistrarReserva
{
    public class RegistrarReservaAD : IRegistrarReservaAD
    {
        private readonly Contexto _elContexto;

        public RegistrarReservaAD()
        {
            _elContexto = new Contexto();
        }

        public int RegistrarReserva(ReservaCrearDto reserva)
        {
            var reservaExistente = _elContexto.Reservas.FirstOrDefault(r =>
                r.idUsuario == reserva.idUsuario &&
                r.idClaseProgramada == reserva.idClaseProgramada);

            if (reservaExistente != null)
            {
                if (reservaExistente.idEstadoReserva == 1)
                {
                    return 0;
                }

                reservaExistente.idEstadoReserva = 1;
                reservaExistente.fechaReserva = DateTime.Now;
                reservaExistente.observaciones = reserva.observaciones;

                return _elContexto.SaveChanges();
            }

            var reservaAGuardar = new ReservaEntidad
            {
                idUsuario = reserva.idUsuario,
                idClaseProgramada = reserva.idClaseProgramada,
                idEstadoReserva = reserva.idEstadoReserva,
                fechaReserva = DateTime.Now,
                observaciones = reserva.observaciones
            };

            _elContexto.Reservas.Add(reservaAGuardar);
            return _elContexto.SaveChanges();
        }
    }
}