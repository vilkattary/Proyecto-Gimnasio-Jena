using GimnasioJena.Abstracciones.AccesoADatos.Pagos.ObtenerTodosLosPagos;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Pagos.ObtenerTodosLosPagos;
using GimnasioJena.Abstracciones.Modelos.Pagos;
using GimnasioJena.AccesoADatos.Pagos.ObtenerTodosLosPagos;
using System.Collections.Generic;

namespace GimnasioJena.LogicaDeNegocio.Pagos.ObtenerTodosLosPagos
{
    public class ObtenerTodosLosPagosLN : IObtenerTodosLosPagosLN
    {
        private readonly IObtenerTodosLosPagosAD _obtenerTodosLosPagosAD;

        public ObtenerTodosLosPagosLN()
        {
            _obtenerTodosLosPagosAD = new ObtenerTodosLosPagosAD();
        }

        public List<PagoListadoDto> ObtenerTodosLosPagos()
        {
            return _obtenerTodosLosPagosAD.ObtenerTodosLosPagos();
        }
    }
}