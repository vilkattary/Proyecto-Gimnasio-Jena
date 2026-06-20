using GimnasioJena.Abstracciones.AccesoADatos.EstadoReservas.ObtenerEstadoReservas;
using GimnasioJena.Abstracciones.LogicaDeNegocio.EstadoReservas.ObtenerEstadoReservas;
using GimnasioJena.Abstracciones.Modelos.Reservas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GimnasioJena.LogicaDeNegocio.EstadoReservas.ObtenerEstadoReservas
{
    public class ObtenerEstadoReservaLN : IObtenerEstadoReservasLN
    {
        private readonly IObtenerEstadoReservaAD _obtenerEstadoReservaAD;

        public ObtenerEstadoReservaLN(IObtenerEstadoReservaAD obtenerEstadoReservaAD)
        {
            _obtenerEstadoReservaAD = obtenerEstadoReservaAD;
        }

        public EstadoReservasDto ObtenerEstadoReservaPorId(int id)
        {
            return _obtenerEstadoReservaAD.ObtenerEstadoReservaPorId(id);
        }

        public IEnumerable<EstadoReservasDto> ObtenerTodosLosEstadosReservas()
        {
            return _obtenerEstadoReservaAD.ObtenerTodosLosEstadosReservas();
        }
    }

}
