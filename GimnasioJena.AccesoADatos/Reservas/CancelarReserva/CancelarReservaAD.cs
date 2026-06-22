using GimnasioJena.Abstracciones.AccesoADatos.Reservas.CancelarReserva;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Reservas.CancelarReserva
{
    public class CancelarReservaAD : ICancelarReservaAD
    {
        private readonly Contexto _contexto;

        public CancelarReservaAD()
        {
            _contexto = new Contexto();
        }

        public bool CancelarReserva(int idReserva, int idUsuario)
        {
            var reserva = _contexto.Reservas
                .FirstOrDefault(r =>
                    r.idReserva == idReserva &&
                    r.idUsuario == idUsuario);

            if (reserva == null)
            {
                return false;
            }

            if (reserva.idEstadoReserva != 1)
            {
                return false;
            }

            reserva.idEstadoReserva = 2;

            return _contexto.SaveChanges() > 0;
        }
    }
}