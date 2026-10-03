using GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenamientos.EditarMesociclo;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenamientos.EliminarMesociclo;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenamientos.GuardarPlantillaDiaEntrenamiento;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenamientos.ObtenerMesocicloPorId;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenamientos.ObtenerMesociclos;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenamientos.ObtenerPlantillaDiaPorId;
using GimnasioJena.Abstracciones.Modelos.Entrenamientos;
using GimnasioJena.LogicaDeNegocio.Entrenamientos.EditarMesociclo;
using GimnasioJena.LogicaDeNegocio.Entrenamientos.EliminarMesociclo;
using GimnasioJena.LogicaDeNegocio.Entrenamientos.GuardarPlantillaDiaEntrenamiento;
using GimnasioJena.LogicaDeNegocio.Entrenamientos.ObtenerMesocicloPorId;
using GimnasioJena.LogicaDeNegocio.Entrenamientos.ObtenerMesociclos;
using GimnasioJena.LogicaDeNegocio.Entrenamientos.ObtenerPlantillaDiaPorId;
using GimnasioJena.LogicaDeNegocio.Entrenamientos.RegistrarMesociclo;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenamientos.RegistrarMesociclo;
using System;
using System.Linq;
using System.Web.Mvc;

namespace GimnasioJena.UI.Controllers
{
    [Authorize(Roles = "ADMINISTRADOR,ENTRENADOR")]
    public class MesocicloController : Controller
    {
        private readonly IObtenerMesociclosLN _obtenerMesociclosLN;
        private readonly IObtenerMesocicloPorIdLN _obtenerMesocicloPorIdLN;
        private readonly IRegistrarMesocicloLN _registrarMesocicloLN;
        private readonly IEditarMesocicloLN _editarMesocicloLN;
        private readonly IEliminarMesocicloLN _eliminarMesocicloLN;
        private readonly IObtenerPlantillaDiaPorIdLN _obtenerPlantillaDiaPorIdLN;
        private readonly IGuardarPlantillaDiaEntrenamientoLN _guardarPlantillaDiaEntrenamientoLN;

        public MesocicloController()
        {
            _obtenerMesociclosLN = new ObtenerMesociclosLN();
            _obtenerMesocicloPorIdLN = new ObtenerMesocicloPorIdLN();
            _registrarMesocicloLN = new RegistrarMesocicloLN();
            _editarMesocicloLN = new EditarMesocicloLN();
            _eliminarMesocicloLN = new EliminarMesocicloLN();
            _obtenerPlantillaDiaPorIdLN = new ObtenerPlantillaDiaPorIdLN();
            _guardarPlantillaDiaEntrenamientoLN = new GuardarPlantillaDiaEntrenamientoLN();
        }

        // ------------------------------------------------------------------
        // CRUD tradicional de mesociclos
        // ------------------------------------------------------------------

        public ActionResult Index()
        {
            return View(_obtenerMesociclosLN.ObtenerMesociclos());
        }

        public ActionResult Details(int id)
        {
            MesocicloDto mesociclo = _obtenerMesocicloPorIdLN.ObtenerMesocicloPorId(id);

            if (mesociclo == null)
            {
                TempData["MensajeError"] = "No se encontró el mesociclo solicitado.";
                return RedirectToAction("Index");
            }

            return View(mesociclo);
        }

        public ActionResult Create()
        {
            return View(new MesocicloDto
            {
                FechaInicio = DateTime.Today,
                FechaFin = DateTime.Today.AddDays(28),
                EsActivo = true
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MesocicloDto modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            try
            {
                _registrarMesocicloLN.RegistrarMesociclo(modelo);
                TempData["MensajeExito"] = "El mesociclo se registró correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = ex.Message;
                return View(modelo);
            }
        }

        public ActionResult Edit(int id)
        {
            MesocicloDto mesociclo = _obtenerMesocicloPorIdLN.ObtenerMesocicloPorId(id);

            if (mesociclo == null)
            {
                TempData["MensajeError"] = "No se encontró el mesociclo solicitado.";
                return RedirectToAction("Index");
            }

            return View(mesociclo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MesocicloDto modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            try
            {
                bool actualizado = _editarMesocicloLN.EditarMesociclo(modelo);

                if (!actualizado)
                {
                    TempData["MensajeError"] = "No se encontró el mesociclo a actualizar.";
                    return RedirectToAction("Index");
                }

                TempData["MensajeExito"] = "El mesociclo se actualizó correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = ex.Message;
                return View(modelo);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                bool eliminado = _eliminarMesocicloLN.EliminarMesociclo(id);

                TempData[eliminado ? "MensajeExito" : "MensajeError"] = eliminado
                    ? "El mesociclo se eliminó correctamente."
                    : "No se encontró el mesociclo a eliminar.";
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        // ------------------------------------------------------------------
        // Endpoints asíncronos (consumo AJAX desde las pantallas de clases)
        // ------------------------------------------------------------------

        [HttpGet]
        public ActionResult GetDayPreviewPartial(int templateId)
        {
            PlantillaDiaDto plantilla =
                _obtenerPlantillaDiaPorIdLN.ObtenerPlantillaDiaPorId(templateId);

            if (plantilla == null)
            {
                return HttpNotFound();
            }

            return PartialView("_DayPreviewPartial", plantilla);
        }

        [HttpGet]
        public ActionResult GetWorkoutEditorPartial(
            int templateId,
            byte dayOfWeek = 1,
            int mesocicloId = 0
        )
        {
            PlantillaDiaDto plantilla = templateId > 0
                ? _obtenerPlantillaDiaPorIdLN.ObtenerPlantillaDiaPorId(templateId)
                : null;

            if (plantilla == null)
            {
                if (mesocicloId <= 0)
                {
                    // Al crear una rutina nueva desde las pantallas de clases,
                    // se vincula automáticamente al mesociclo activo vigente.
                    var mesociclos = _obtenerMesociclosLN.ObtenerMesociclos();
                    var mesocicloActivo = mesociclos
                        .FirstOrDefault(m => m.EsActivo)
                        ?? mesociclos.FirstOrDefault();

                    if (mesocicloActivo != null)
                    {
                        mesocicloId = mesocicloActivo.idMesociclo;
                    }
                }

                plantilla = new PlantillaDiaDto
                {
                    idPlantillaDia = 0,
                    idMesociclo = mesocicloId,
                    DiaSemana = dayOfWeek
                };
            }

            return PartialView("_WorkoutDayEditorPartial", plantilla);
        }

        [HttpPost]
        public ActionResult SaveWorkoutDayTemplateAjax(GuardarPlantillaDiaDto modelo)
        {
            try
            {
                System.Web.Helpers.AntiForgery.Validate(
                    Request.Cookies[System.Web.Helpers.AntiForgeryConfig.CookieName]?.Value,
                    Request.Headers["RequestVerificationToken"]);

                ResultadoGuardarPlantillaDto resultado =
                    _guardarPlantillaDiaEntrenamientoLN
                        .GuardarPlantillaDiaEntrenamiento(modelo);

                if (!resultado.Exito)
                {
                    return Json(new { success = false, message = resultado.Mensaje });
                }

                return Json(new
                {
                    success = true,
                    templateId = resultado.idPlantillaDia,
                    templateName = resultado.NombrePlantilla,
                    message = resultado.Mensaje
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
