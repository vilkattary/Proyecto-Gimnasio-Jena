namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.CancelarReserva
{
    public interface ICancelarReservaLN
    {
        bool CancelarReserva(int idReserva, int idUsuario);
    }
}