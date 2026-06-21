using GimnasioJena.Abstracciones.AccesoADatos.Home.ObtenerSeccionesHome;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Home.ObtenerSeccionesHome;
using GimnasioJena.Abstracciones.Modelos.Home;
using GimnasioJena.AccesoADatos.Nosotros.ObtenerSeccionesNosotros;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GimnasioJena.LogicaDeNegocio.Nosotros.ObtenerSeccionesNosotros
{
    public class ObtenerContenidoNosotrosLN : IObtenerContenidoWebLN
    {
        private readonly IObtenerContenidoWebAD _repositorio;

        public ObtenerContenidoNosotrosLN()
        {
            _repositorio = new ObtenerContenidoNosotrosAD();
        }

        public async Task<IEnumerable<ContenidoWebDto>> EjecutarAsync(string pagina)
        {
            return await _repositorio.ObtenerPorPaginaAsync(pagina);
        }

        public async Task<IEnumerable<ContenidoWebDto>> EjecutarTodosAsync(string pagina)
        {
            return await EjecutarAsync(pagina);
        }
    }
}
