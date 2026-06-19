using GimnasioJena.Abstracciones.Modelos.Reservas;

namespace GimnasioJena.Abstracciones.AccesoADatos.Reservas.RegistrarReserva
{
    public interface IRegistrarReservaAD
    {
        int RegistrarReserva(ReservaCrearDto reserva);
    }
}