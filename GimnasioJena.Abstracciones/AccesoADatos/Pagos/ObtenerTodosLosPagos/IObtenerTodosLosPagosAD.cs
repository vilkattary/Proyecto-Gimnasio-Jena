using GimnasioJena.Abstracciones.Modelos.Pagos;
using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.AccesoADatos.Pagos.ObtenerTodosLosPagos
{
    public interface IObtenerTodosLosPagosAD
    {
        List<PagoListadoDto> ObtenerTodosLosPagos();
    }
}