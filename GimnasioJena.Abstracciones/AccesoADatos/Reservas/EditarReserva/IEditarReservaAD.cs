namespace GimnasioJena.Abstracciones.AccesoADatos.Reservas.EditarReserva
{
    public interface IEditarReservaAD
    {
        bool CambiarEstadoReserva(int idReserva, int idEstadoReserva);
    }
}