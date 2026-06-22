using GimnasioJena.Abstracciones.Modelos.Reservas;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.ObtenerReservaPorId
{
    public interface IObtenerReservaPorIdLN
    {
        ReservaListadoDto ObtenerReservaPorId(int idReserva);
    }
}