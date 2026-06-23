using GimnasioJena.Abstracciones.AccesoADatos.Reservas.ObtenerTodasLasReservas;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.ObtenerTodasLasReservas;
using GimnasioJena.Abstracciones.Modelos.Reservas;
using GimnasioJena.AccesoADatos.Reservas.ObtenerTodasLasReservas;
using System.Collections.Generic;

namespace GimnasioJena.LogicaDeNegocio.Reservas.ObtenerTodasLasReservas
{
    public class ObtenerTodasLasReservasLN : IObtenerTodasLasReservasLN
    {
        private readonly IObtenerTodasLasReservasAD _obtenerTodasLasReservasAD;

        public ObtenerTodasLasReservasLN()
        {
            _obtenerTodasLasReservasAD = new ObtenerTodasLasReservasAD();
        }

        public List<ReservaListadoDto> ObtenerTodasLasReservas()
        {
            return _obtenerTodasLasReservasAD.ObtenerTodasLasReservas();
        }
    }
}