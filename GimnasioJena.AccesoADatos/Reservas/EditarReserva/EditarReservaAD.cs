using GimnasioJena.Abstracciones.AccesoADatos.Reservas.EditarReserva;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Reservas.EditarReserva
{
    public class EditarReservaAD : IEditarReservaAD
    {
        private readonly Contexto _contexto;

        public EditarReservaAD()
        {
            _contexto = new Contexto();
        }

        public bool CambiarEstadoReserva(int idReserva, int idEstadoReserva)
        {
            var reserva = _contexto.Reservas.FirstOrDefault(r => r.idReserva == idReserva);
            if (reserva == null) return false;

            reserva.idEstadoReserva = idEstadoReserva;
            _contexto.SaveChanges();
            return true;
        }
    }
}