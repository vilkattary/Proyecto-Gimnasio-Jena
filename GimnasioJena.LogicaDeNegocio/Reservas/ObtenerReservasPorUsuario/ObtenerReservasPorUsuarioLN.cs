using GimnasioJena.Abstracciones.AccesoADatos.Reservas.ObtenerReservasPorUsuario;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.ObtenerReservasPorUsuario;
using GimnasioJena.Abstracciones.Modelos.Reservas;
using GimnasioJena.AccesoADatos.Reservas.ObtenerReservasPorUsuario;
using System.Collections.Generic;

namespace GimnasioJena.LogicaDeNegocio.Reservas.ObtenerReservasPorUsuario
{
    public class ObtenerReservasPorUsuarioLN : IObtenerReservasPorUsuarioLN
    {
        private readonly IObtenerReservasPorUsuarioAD _ad;

        public ObtenerReservasPorUsuarioLN()
        {
            _ad = new ObtenerReservasPorUsuarioAD();
        }

        public List<ReservaListadoDto> ObtenerReservasPorUsuario(int idUsuario)
        {
            if (idUsuario <= 0) return new List<ReservaListadoDto>();
            return _ad.ObtenerReservasPorUsuario(idUsuario);
        }
    }
}