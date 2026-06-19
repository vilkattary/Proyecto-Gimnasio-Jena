using GimnasioJena.Abstracciones.Modelos.Reservas;
using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.ObtenerReservasPorClase
{
    public interface IObtenerReservasPorClaseLN
    {
        List<ReservaClaseDto> ObtenerReservasPorClase(int idClaseProgramada);
    }
}