using GimnasioJena.Abstracciones.Modelos.Reservas;
using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.AccesoADatos.Reservas.ObtenerReservasPorClase
{
    public interface IObtenerReservasPorClaseAD
    {
        List<ReservaClaseDto> ObtenerReservasPorClase(int idClaseProgramada);
    }
}