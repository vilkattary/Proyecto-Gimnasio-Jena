using GimnasioJena.Abstracciones.LogicaDeNegocio.Home.ObtenerSeccionesHome;
using GimnasioJena.LogicaDeNegocio.Home.ObtenerSeccionesHome;
using GimnasioJena.UI.Filters;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GimnasioJena.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IObtenerSeccionesHomeLN _obtenerSeccionesHome;

        public HomeController()
        {
            _obtenerSeccionesHome = new ObtenerSeccionesHomeLN();
        }

        public async Task<ActionResult> Index()
        {
            var modelo = await _obtenerSeccionesHome.ObtenerSeccionesHome();
            return View(modelo);
        }

        [SoloAdministrador]
        public async Task<ActionResult> AdminIndex()
        {
            var modelo = await _obtenerSeccionesHome.ObtenerSeccionesHome();
            return View(modelo);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Planes()
        {
            return View();
        }
    }
}