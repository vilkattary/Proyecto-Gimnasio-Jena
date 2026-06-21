using GimnasioJena.Abstracciones.Modelos.Reservas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.EstadoReservas.ObtenerEstadoReservas
{
    public interface IObtenerEstadoReservasLN
    {
        EstadoReservasDto ObtenerEstadoReservaPorId(int id);
        IEnumerable<EstadoReservasDto> ObtenerTodosLosEstadosReservas();
    }
}
