using GimnasioJena.Abstracciones.Modelos.Reservas;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.CancelarReserva
{
    public interface ICancelarReservaLN
    {
        bool CancelarReserva(ReservaCancelarDto reserva, int idUsuario);
    }
}