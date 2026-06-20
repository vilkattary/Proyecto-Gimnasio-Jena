namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.EditarReserva
{
    public interface IEditarReservaLN
    {
        bool CambiarEstadoReserva(int idReserva, int idEstadoReserva);
    }
}