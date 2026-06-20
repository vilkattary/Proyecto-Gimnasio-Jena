using System.Collections.Generic;
using GimnasioJena.Abstracciones.Modelos.Reservas;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.ObtenerReservasPorUsuario
{
    public interface IObtenerReservasPorUsuarioLN
    {
        List<ReservaListadoDto> ObtenerReservasPorUsuario(int idUsuario);
    }
}