namespace GimnasioJena.Abstracciones.AccesoADatos.Reservas.CancelarReserva
{
    public interface ICancelarReservaAD
    {
        bool CancelarReserva(int idReserva, int idUsuario);
    }
}