using GimnasioJena.Abstracciones.Modelos.Reservas;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.RegistrarReserva
{
    public interface IRegistrarReservaLN
    {
        ResultadoReservaDto RegistrarReserva(
            ReservaCrearDto reserva
        );
    }
}