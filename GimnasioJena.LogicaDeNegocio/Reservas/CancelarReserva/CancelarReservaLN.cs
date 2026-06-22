using GimnasioJena.Abstracciones.AccesoADatos.Reservas.CancelarReserva;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.CancelarReserva;
using GimnasioJena.AccesoADatos.Reservas.CancelarReserva;

namespace GimnasioJena.LogicaDeNegocio.Reservas.CancelarReserva
{
    public class CancelarReservaLN : ICancelarReservaLN
    {
        private readonly ICancelarReservaAD _cancelarReservaAD;

        public CancelarReservaLN()
        {
            _cancelarReservaAD = new CancelarReservaAD();
        }

        public bool CancelarReserva(int idReserva, int idUsuario)
        {
            if (idReserva <= 0)
                return false;

            if (idUsuario <= 0)
                return false;

            return _cancelarReservaAD.CancelarReserva(idReserva, idUsuario);
        }
    }
}