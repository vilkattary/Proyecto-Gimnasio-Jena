using GimnasioJena.Abstracciones.Modelos.Pagos;
using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Pagos.ObtenerTodosLosPagos
{
    public interface IObtenerTodosLosPagosLN
    {
        List<PagoListadoDto> ObtenerTodosLosPagos();
    }
}