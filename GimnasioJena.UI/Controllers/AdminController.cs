using GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.EditarPrecioPlan;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.ObtenerPlanesMembresia;
using GimnasioJena.Abstracciones.Modelos.Membresias;
using GimnasioJena.LogicaDeNegocio.Membresias.EditarPrecioPlan;
using GimnasioJena.LogicaDeNegocio.Membresias.ObtenerPlanesMembresia;
using GimnasioJena.UI.Filters;
using System.Linq;
using System.Web.Mvc;

namespace GimnasioJena.UI.Controllers
{
    [SoloAdministrador]
    public class AdminController : Controller
    {
        private readonly IObtenerPlanesMembresiaLN
            _obtenerPlanesMembresiaLN;

        private readonly IEditarPrecioPlanLN
            _editarPrecioPlanLN;

        public AdminController()
        {
            _obtenerPlanesMembresiaLN =
                new ObtenerPlanesMembresiaLN();

            _editarPrecioPlanLN =
                new EditarPrecioPlanLN();
        }

        // GET: /Admin/Dashboard
        public ActionResult Dashboard()
        {
            ViewBag.Title = "Panel de Administración – Jéna";

            return View();
        }

        public ActionResult PlanesMembresia()
        {
            var planes =
                _obtenerPlanesMembresiaLN
                    .ObtenerTodosLosPlanes();

            return View(planes);
        }

        [HttpGet]
        public ActionResult EditarPrecioPlan(int id)
        {
            var plan = _obtenerPlanesMembresiaLN
                .ObtenerTodosLosPlanes()
                .FirstOrDefault(p => p.idPlanMembresia == id);

            if (plan == null)
            {
                TempData["MensajeError"] = "No se encontró el plan solicitado.";
                return RedirectToAction("PlanesMembresia");
            }

            var modelo = new EditarPrecioPlanDto
            {
                idPlanMembresia = plan.idPlanMembresia,
                nombrePlan = plan.nombrePlan,
                precio = plan.precio
            };

            return View(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarPrecioPlan(EditarPrecioPlanDto modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            bool resultado =
                _editarPrecioPlanLN
                    .EditarPrecioPlan(modelo);

            if (resultado)
            {
                TempData["MensajeExito"] =
                    "El precio del plan se actualizó correctamente.";

                return RedirectToAction("PlanesMembresia");
            }

            TempData["MensajeError"] =
                "No se pudo actualizar el precio del plan.";

            return View(modelo);
        }
    }
}