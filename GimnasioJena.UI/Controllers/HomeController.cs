using GimnasioJena.Abstracciones.LogicaDeNegocio.Home.ObtenerSeccionesHome;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.ObtenerPlanesMembresia;
using GimnasioJena.LogicaDeNegocio.Home.ObtenerSeccionesHome;
using GimnasioJena.LogicaDeNegocio.Membresias.ObtenerPlanesMembresia;
using GimnasioJena.UI.Filters;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GimnasioJena.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IObtenerContenidoWebLN _obtenerContenidoWeb;
        private readonly IObtenerPlanesMembresiaLN _obtenerPlanesMembresiaServicio;

        public HomeController()
        {
            _obtenerContenidoWeb = new ObtenerContenidoWebLN();
            _obtenerPlanesMembresiaServicio = new ObtenerPlanesMembresiaLN();
        }

        public async Task<ActionResult> Index()
        {
            var modelo = await _obtenerContenidoWeb.EjecutarAsync("Home");
            return View(modelo);
        }

        [SoloAdministrador]
        public async Task<ActionResult> AdminIndex()
        {
            var modelo = await _obtenerContenidoWeb.EjecutarTodosAsync("Home");
            return View(modelo);
        }

        public ActionResult About()
        {
            return RedirectToAction("Index", "Nosotros");
        }

        public ActionResult Contact()
        {
            return RedirectToAction("Index", "Contacto");
        }

        public ActionResult Planes()
        {
            var planes = _obtenerPlanesMembresiaServicio.ObtenerPlanesActivos();
            return View(planes);
        }
    }
}