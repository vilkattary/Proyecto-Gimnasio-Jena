using GimnasioJena.Abstracciones.AccesoADatos.Reservas.RegistrarReserva;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.RegistrarReserva;
using GimnasioJena.Abstracciones.Modelos.Reservas;
using GimnasioJena.AccesoADatos.Reservas.RegistrarReserva;

namespace GimnasioJena.LogicaDeNegocio.Reservas.RegistrarReserva
{
    public class RegistrarReservaLN : IRegistrarReservaLN
    {
        private readonly IRegistrarReservaAD _registrarReservaAD;

        public RegistrarReservaLN()
        {
            _registrarReservaAD = new RegistrarReservaAD();
        }

        public bool RegistrarReserva(ReservaCrearDto reserva)
        {
            if (reserva == null)
                return false;

            if (reserva.idUsuario <= 0)
                return false;

            if (reserva.idClaseProgramada <= 0)
                return false;

            if (reserva.idEstadoReserva <= 0)
                reserva.idEstadoReserva = 1;

            return _registrarReservaAD.RegistrarReserva(reserva) > 0;
        }
    }
}