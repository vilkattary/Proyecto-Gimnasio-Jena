using GimnasioJena.Abstracciones.AccesoADatos.Home.ObtenerSeccionesHome;
using GimnasioJena.Abstracciones.Modelos.Home;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Home.ObtenerSeccionesHome;
using GimnasioJena.AccesoADatos.Home.ObtenerSeccionesHome;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GimnasioJena.LogicaDeNegocio.Home.ObtenerSeccionesHome
{
    public class ObtenerContenidoWebLN : IObtenerContenidoWebLN
    {
        private readonly IObtenerContenidoWebAD _repositorio;

        public ObtenerContenidoWebLN()
        {
            _repositorio = new ObtenerContenidoWebAD();
        }

        public async Task<IEnumerable<ContenidoWebDto>> EjecutarAsync(string pagina)
        {
            return await _repositorio.ObtenerPorPaginaAsync(pagina);
        }
    }
}
