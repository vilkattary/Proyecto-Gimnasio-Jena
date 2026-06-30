using GimnasioJena.Abstracciones.AccesoADatos.Reservas.ObtenerReservaPorId;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.ObtenerReservaPorId;
using GimnasioJena.Abstracciones.Modelos.Reservas;
using GimnasioJena.AccesoADatos.Reservas.ObtenerReservaPorId;

namespace GimnasioJena.LogicaDeNegocio.Reservas.ObtenerReservaPorId
{
    public class ObtenerReservaPorIdLN : IObtenerReservaPorIdLN
    {
        private readonly IObtenerReservaPorIdAD _obtenerReservaPorIdAD;

        public ObtenerReservaPorIdLN()
        {
            _obtenerReservaPorIdAD = new ObtenerReservaPorIdAD();
        }

        public ReservaListadoDto ObtenerReservaPorId(int idReserva)
        {
            if (idReserva <= 0)
            {
                return null;
            }

            return _obtenerReservaPorIdAD.ObtenerReservaPorId(idReserva);
        }
    }
}