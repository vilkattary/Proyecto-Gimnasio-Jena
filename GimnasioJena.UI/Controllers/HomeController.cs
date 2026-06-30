using GimnasioJena.Abstracciones.LogicaDeNegocio.Home.ObtenerSeccionesHome;
using GimnasioJena.LogicaDeNegocio.Home.ObtenerSeccionesHome;
using GimnasioJena.UI.Filters;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GimnasioJena.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IObtenerContenidoWebLN _obtenerContenidoWeb;

        public HomeController()
        {
            _obtenerContenidoWeb = new ObtenerContenidoWebLN();
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
            return View();
        }
    }
}