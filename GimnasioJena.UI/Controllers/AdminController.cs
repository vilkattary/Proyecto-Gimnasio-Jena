using GimnasioJena.UI.Filters;
using System.Web.Mvc;

namespace GimnasioJena.UI.Controllers
{
    [SoloAdministrador]
    public class AdminController : Controller
    {
        // GET: /Admin/Dashboard
        public ActionResult Dashboard()
        {
            ViewBag.Title = "Panel de Administración – Jéna";
            return View();
        }
    }
}
