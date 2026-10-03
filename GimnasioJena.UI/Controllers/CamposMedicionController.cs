using GimnasioJena.Abstracciones.LogicaDeNegocio.Progreso;
using GimnasioJena.Abstracciones.Modelos.Progreso;
using GimnasioJena.LogicaDeNegocio.Progreso;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GimnasioJena.UI.Controllers
{
    [Authorize(Roles = "ADMINISTRADOR")]
    public class CamposMedicionController : Controller
    {
        private readonly ICamposMedicionLN _camposMedicionLN;

        public CamposMedicionController()
        {
            _camposMedicionLN = new CamposMedicionLN();
        }

        // GET: CamposMedicion
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            try
            {
                var campos = await _camposMedicionLN.ObtenerTodosLN();
                return View(campos);
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = ex.ToString();
                return View(new List<CampoMedicionDto>());
            }
        }

        // POST: CamposMedicion/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Crear(CampoMedicionDto dto)
        {
            try
            {
                await _camposMedicionLN.CrearLN(dto);
                TempData["MensajeExito"] = "El campo de medición se creó correctamente.";
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = ex.ToString();
            }

            return RedirectToAction("Index");
        }

        // POST: CamposMedicion/CambiarEstado
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CambiarEstado(int idCampo)
        {
            try
            {
                await _camposMedicionLN.CambiarEstadoLN(idCampo);
                TempData["MensajeExito"] = "El estado del campo se actualizó correctamente.";
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = ex.ToString();
            }

            return RedirectToAction("Index");
        }
    }
}
