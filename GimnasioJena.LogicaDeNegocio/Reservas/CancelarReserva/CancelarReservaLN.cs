using GimnasioJena.Abstracciones.AccesoADatos.Reservas.CancelarReserva;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.CancelarReserva;
using GimnasioJena.Abstracciones.Modelos.Reservas;
using GimnasioJena.AccesoADatos.Reservas.CancelarReserva;

namespace GimnasioJena.LogicaDeNegocio.Reservas.CancelarReserva
{
    public class CancelarReservaLN : ICancelarReservaLN
    {
        private readonly ICancelarReservaAD _cancelarReservaAD;

        public CancelarReservaLN()
        {
            _cancelarReservaAD = new CancelarReservaAD();
        }

        public bool CancelarReserva(ReservaCancelarDto reserva, int idUsuario)
        {
            if (reserva == null)
                return false;

            if (reserva.idReserva <= 0)
                return false;

            if (idUsuario <= 0)
                return false;

            return _cancelarReservaAD.CancelarReserva(reserva, idUsuario);
        }
    }
}