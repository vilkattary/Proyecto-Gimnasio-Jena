using GimnasioJena.Abstracciones.LogicaDeNegocio.Progreso;
using GimnasioJena.Abstracciones.Modelos.Progreso;
using GimnasioJena.LogicaDeNegocio.Progreso;
using Microsoft.AspNet.Identity;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GimnasioJena.UI.Controllers
{
    [Authorize(Roles = "ADMINISTRADOR,ENTRENADOR,CLIENTE")]
    public class MedicionesController : Controller
    {
        private readonly IMedicionesLN _medicionesLN;

        public MedicionesController()
        {
            _medicionesLN = new MedicionesLN();
        }

        // GET: Mediciones/Registrar
        [HttpGet]
        public async Task<ActionResult> Registrar(string idUsuario = "")
        {
            try
            {
                // Seguridad: el Cliente solo puede registrar sus propias medidas.
                if (User.IsInRole("CLIENTE"))
                {
                    idUsuario = User.Identity.GetUserId();
                }

                if (string.IsNullOrWhiteSpace(idUsuario))
                {
                    throw new ArgumentException("Debe indicar el usuario para registrar sus medidas.");
                }

                var dto = await _medicionesLN.GenerarFormularioLN(idUsuario);
                return View(dto);
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = ex.ToString();
                return View(new RegistroMedicionMultipleDto());
            }
        }

        // POST: Mediciones/Registrar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Registrar(RegistroMedicionMultipleDto dto)
        {
            bool esCliente = User.IsInRole("CLIENTE");

            try
            {
                // Seguridad: se vuelve a forzar el Id del Cliente autenticado.
                if (esCliente)
                {
                    dto.idUsuario = User.Identity.GetUserId();
                }

                await _medicionesLN.GuardarMedicionesLN(dto);

                TempData["MensajeExito"] = "Las mediciones se guardaron correctamente.";

                if (esCliente)
                {
                    return RedirectToAction("Evolucion", "Progreso");
                }

                return RedirectToAction("Registrar", new { idUsuario = dto.idUsuario });
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = ex.ToString();
                return RedirectToAction("Registrar", esCliente ? null : new { idUsuario = dto.idUsuario });
            }
        }
    }
}
