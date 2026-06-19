using GimnasioJena.Abstracciones.AccesoADatos.Reservas.RegistrarReserva;
using GimnasioJena.Abstracciones.Modelos.Reservas;
using GimnasioJena.AccesoADatos.Entidades.Reservas;
using System;

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