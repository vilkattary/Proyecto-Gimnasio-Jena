using GimnasioJena.Abstracciones.Modelos.Reservas;

namespace GimnasioJena.Abstracciones.AccesoADatos.Reservas.CancelarReserva
{
    public interface ICancelarReservaAD
    {
        bool CancelarReserva(ReservaCancelarDto reserva, int idUsuario);
    }
}