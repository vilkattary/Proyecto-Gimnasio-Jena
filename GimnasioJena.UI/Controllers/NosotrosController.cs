using GimnasioJena.Abstracciones.LogicaDeNegocio.Home.ObtenerSeccionesHome;
using GimnasioJena.LogicaDeNegocio.Nosotros.ObtenerSeccionesNosotros;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GimnasioJena.UI.Controllers
{
    public class NosotrosController : Controller
    {
        private readonly IObtenerContenidoWebLN _obtenerContenidoWeb;

        public NosotrosController()
        {
            _obtenerContenidoWeb = new ObtenerContenidoNosotrosLN();
        }

        public async Task<ActionResult> Index()
        {
            var modelo = await _obtenerContenidoWeb.EjecutarAsync("Nosotros");
            return View(modelo);
        }
    }
}
