using System.Collections.Generic;
using GimnasioJena.Abstracciones.Modelos.Reservas;

namespace GimnasioJena.Abstracciones.AccesoADatos.Reservas.ObtenerReservasPorUsuario
{
    public interface IObtenerReservasPorUsuarioAD
    {
        List<ReservaListadoDto> ObtenerReservasPorUsuario(int idUsuario);
    }
}