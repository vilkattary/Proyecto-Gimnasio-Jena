using GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.CambiarEstadoPlanMembresia;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.EditarPrecioPlan;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.ObtenerPlanesMembresia;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.RegistrarPlanMembresia;
using GimnasioJena.Abstracciones.Modelos.Membresias;
using GimnasioJena.LogicaDeNegocio.Membresias.CambiarEstadoPlanMembresia;
using GimnasioJena.LogicaDeNegocio.Membresias.EditarPrecioPlan;
using GimnasioJena.LogicaDeNegocio.Membresias.ObtenerPlanesMembresia;
using GimnasioJena.LogicaDeNegocio.Membresias.RegistrarPlanMembresia;
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

        private readonly IRegistrarPlanMembresiaLN
            _registrarPlanMembresiaLN;

        private readonly ICambiarEstadoPlanMembresiaLN
            _cambiarEstadoPlanMembresiaLN;

        public AdminController()
        {
            _obtenerPlanesMembresiaLN =
                new ObtenerPlanesMembresiaLN();

            _editarPrecioPlanLN =
                new EditarPrecioPlanLN();

            _registrarPlanMembresiaLN =
                new RegistrarPlanMembresiaLN();

            _cambiarEstadoPlanMembresiaLN =
                new CambiarEstadoPlanMembresiaLN();
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

            ViewBag.LimiteMaximoPlanes =
                _registrarPlanMembresiaLN.LimiteMaximoPlanes;

            ViewBag.SePuedeRegistrarNuevoPlan =
                _registrarPlanMembresiaLN.SePuedeRegistrarNuevoPlan();

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
                precio = plan.precio,
                duracionDias = plan.duracionDias,
                cantidadClases = plan.cantidadClases
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
                    "El plan se actualizó correctamente.";

                return RedirectToAction("PlanesMembresia");
            }

            TempData["MensajeError"] =
                "No se pudo actualizar el plan.";

            return View(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearPlanMembresia(RegistrarPlanMembresiaDto modelo)
        {
            if (!ModelState.IsValid)
            {
                TempData["MensajeError"] =
                    "Revisa los datos del nuevo plan e inténtalo de nuevo.";

                return RedirectToAction("PlanesMembresia");
            }

            if (!_registrarPlanMembresiaLN.SePuedeRegistrarNuevoPlan())
            {
                TempData["MensajeError"] =
                    "No se pueden registrar más de " +
                    _registrarPlanMembresiaLN.LimiteMaximoPlanes +
                    " planes de membresía.";

                return RedirectToAction("PlanesMembresia");
            }

            bool resultado =
                _registrarPlanMembresiaLN
                    .RegistrarPlanMembresia(modelo);

            if (resultado)
            {
                TempData["MensajeExito"] =
                    "El plan de membresía se creó correctamente.";
            }
            else
            {
                TempData["MensajeError"] =
                    "No se pudo crear el plan de membresía.";
            }

            return RedirectToAction("PlanesMembresia");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CambiarEstadoPlan(CambiarEstadoPlanMembresiaDto modelo)
        {
            bool resultado =
                _cambiarEstadoPlanMembresiaLN
                    .CambiarEstadoPlanMembresia(modelo);

            if (resultado)
            {
                TempData["MensajeExito"] =
                    "El estado del plan se actualizó correctamente.";
            }
            else
            {
                TempData["MensajeError"] =
                    "No se pudo actualizar el estado del plan.";
            }

            return RedirectToAction("PlanesMembresia");
        }
    }
}