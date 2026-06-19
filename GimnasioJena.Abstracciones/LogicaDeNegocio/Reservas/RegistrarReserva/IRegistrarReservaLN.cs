using GimnasioJena.Abstracciones.Modelos.Reservas;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.RegistrarReserva
{
    public interface IRegistrarReservaLN
    {
        bool RegistrarReserva(ReservaCrearDto reserva);
    }
}