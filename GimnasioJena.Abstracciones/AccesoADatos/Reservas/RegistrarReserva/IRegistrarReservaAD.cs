using GimnasioJena.Abstracciones.Modelos.Reservas;

namespace GimnasioJena.Abstracciones.AccesoADatos.Reservas.RegistrarReserva
{
    public interface IRegistrarReservaAD
    {
        int RegistrarReserva(ReservaCrearDto reserva);

        ReservaClaseValidacionDto ObtenerClaseParaValidacion(int idClaseProgramada);

        bool UsuarioTieneReservaActiva(int idUsuario, int idClaseProgramada);

        int ContarReservasActivasPorClase(int idClaseProgramada);
        bool DescontarClaseDisponible(int idMembresiaCliente);

        int RegistrarReservaYDescontarClase(ReservaCrearDto reserva, int idMembresiaCliente);
    }
}