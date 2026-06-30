using GimnasioJena.Abstracciones.Modelos.Reservas;
using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.ObtenerTodasLasReservas
{
    public interface IObtenerTodasLasReservasLN
    {
        List<ReservaListadoDto> ObtenerTodasLasReservas();
    }
}