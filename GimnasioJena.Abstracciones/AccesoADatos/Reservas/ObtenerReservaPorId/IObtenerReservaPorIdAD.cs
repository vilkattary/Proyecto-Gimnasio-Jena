using GimnasioJena.Abstracciones.Modelos.Reservas;

namespace GimnasioJena.Abstracciones.AccesoADatos.Reservas.ObtenerReservaPorId
{
    public interface IObtenerReservaPorIdAD
    {
        ReservaListadoDto ObtenerReservaPorId(int idReserva);
    }
}