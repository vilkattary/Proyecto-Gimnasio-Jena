using GimnasioJena.Abstracciones.LogicaDeNegocio.Progreso;
using GimnasioJena.Abstracciones.Modelos.Progreso;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GimnasioJena.UI.Controllers
{
    [Authorize]
    public class ProgresoController : Controller
    {
        private readonly IProgresoLN _progresoLN;
        private readonly IEntrenamientoLN _entrenamientoLN;
        private readonly IEvolucionAvanzadaLN _evolucionAvanzadaLN;

        public ProgresoController(
            IProgresoLN progresoLN,
            IEntrenamientoLN entrenamientoLN,
            IEvolucionAvanzadaLN evolucionAvanzadaLN)
        {
            _progresoLN = progresoLN;
            _entrenamientoLN = entrenamientoLN;
            _evolucionAvanzadaLN = evolucionAvanzadaLN;
        }

        // GET: Progreso/Evolucion
        [HttpGet]
        public async Task<ActionResult> Evolucion(string filtroTiempo = "1Mes")
        {
            try
            {
                var filtrosValidos = new[] { "1Mes", "3Meses", "6Meses", "Anual" };
                if (string.IsNullOrWhiteSpace(filtroTiempo) || !filtrosValidos.Contains(filtroTiempo))
                {
                    filtroTiempo = "1Mes";
                }

                var identityUserId = User.Identity.GetUserId();
                var idUsuario = await _progresoLN.ObtenerIdUsuarioActualLN(identityUserId);

                var dto = await _progresoLN.GenerarResumenEvolucionLN(idUsuario, filtroTiempo);

                ViewBag.FiltroTiempo = filtroTiempo;
                ViewBag.FiltroSeleccionado = filtroTiempo;

                return View(dto);
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = ex.ToString();
                return View(new ResumenEvolucionDto());
            }
        }

        // GET: Progreso/Registrar
        [HttpGet]
        [Authorize(Roles = "CLIENTE")]
        public async Task<ActionResult> Registrar()
        {
            try
            {
                var entrenamientoHoy = await _entrenamientoLN.ObtenerEntrenamientoDeHoyLN();

                if (entrenamientoHoy != null)
                {
                    ViewBag.NombreRutina = entrenamientoHoy.NombreRutina;
                    ViewBag.EjerciciosDetalle = entrenamientoHoy.EjerciciosDetalle;
                    ViewBag.DiaSemana = entrenamientoHoy.DiaSemana;
                }

                var dto = new ProgresoDiaDto
                {
                    IdEntrenamiento = entrenamientoHoy != null ? entrenamientoHoy.IdEntrenamiento : 0,
                    FechaRegistro = DateTime.Now
                };

                return View(dto);
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = ex.ToString();
                return View(new ProgresoDiaDto());
            }
        }

        // POST: Progreso/GuardarProgreso
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GuardarProgreso(ProgresoDiaDto dto)
        {
            try
            {
                var identityUserId = User.Identity.GetUserId();
                dto.idUsuario = await _progresoLN.ObtenerIdUsuarioActualLN(identityUserId);

                await _progresoLN.GuardarProgresoDiarioLN(dto);

                TempData["MensajeExito"] = "El progreso se guardó correctamente.";
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = ex.ToString();
            }

            return RedirectToAction("Evolucion");
        }

        // GET: Progreso/RegistrarEnClase
        [HttpGet]
        [Authorize(Roles = "CLIENTE,ADMINISTRADOR,ENTRENADOR")]
        public async Task<ActionResult> RegistrarEnClase(int idClaseProgramada)
        {
            try
            {
                var identityUserId = User.Identity.GetUserId();
                var idUsuario = await _progresoLN.ObtenerIdUsuarioActualLN(identityUserId);

                var dto = await _evolucionAvanzadaLN.ObtenerHojaProgresoParaClienteLN(idClaseProgramada, idUsuario);

                return View(dto);
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = ex.ToString();
                return RedirectToAction("Evolucion");
            }
        }

        // POST: Progreso/RegistrarEnClase
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CLIENTE,ADMINISTRADOR,ENTRENADOR")]
        public async Task<ActionResult> RegistrarEnClase(RegistrarProgresoClaseDto dto)
        {
            try
            {
                var identityUserId = User.Identity.GetUserId();

                // Se impone la identidad del usuario autenticado: un cliente no puede
                // registrar progreso bajo el identificador de otro usuario.
                dto.idUsuario = await _progresoLN.ObtenerIdUsuarioActualLN(identityUserId);

                await _evolucionAvanzadaLN.GuardarProgresoClienteLN(dto);

                TempData["MensajeExito"] = "Tu progreso se guardó correctamente.";
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = ex.ToString();
            }

            return RedirectToAction("Evolucion");
        }
    }
}
