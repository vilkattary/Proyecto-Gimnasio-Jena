using GimnasioJena.Abstracciones.Modelos.Reservas;
using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.AccesoADatos.Reservas.ObtenerTodasLasReservas
{
    public interface IObtenerTodasLasReservasAD
    {
        List<ReservaListadoDto> ObtenerTodasLasReservas();
    }
}