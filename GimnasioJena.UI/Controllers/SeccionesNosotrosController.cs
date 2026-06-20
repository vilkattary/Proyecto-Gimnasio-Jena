using GimnasioJena.Abstracciones.LogicaDeNegocio.Home.ModificarSeccionHome;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Home.ObtenerSeccionesHome;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Nosotros.AgregarSeccionNosotros;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Nosotros.EliminarSeccionNosotros;
using GimnasioJena.Abstracciones.Modelos.Home;
using GimnasioJena.LogicaDeNegocio.Nosotros.AgregarSeccionNosotros;
using GimnasioJena.LogicaDeNegocio.Nosotros.EliminarSeccionNosotros;
using GimnasioJena.LogicaDeNegocio.Nosotros.ModificarSeccionNosotros;
using GimnasioJena.LogicaDeNegocio.Nosotros.ObtenerSeccionesNosotros;
using GimnasioJena.UI.Filters;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GimnasioJena.UI.Controllers
{
    [SoloAdministrador]
    public class SeccionesNosotrosController : Controller
    {
        private readonly IObtenerContenidoWebLN   _obtenerContenidoWeb;
        private readonly IModificarContenidoWebLN  _modificarContenidoWeb;
        private readonly IAgregarSeccionNosotrosLN _agregarSeccion;
        private readonly IEliminarSeccionNosotrosLN _eliminarSeccion;

        public SeccionesNosotrosController()
        {
            _obtenerContenidoWeb   = new ObtenerContenidoNosotrosLN();
            _modificarContenidoWeb = new ModificarContenidoNosotrosLN();
            _agregarSeccion        = new AgregarSeccionNosotrosLN();
            _eliminarSeccion       = new EliminarSeccionNosotrosLN();
        }

        public async Task<ActionResult> Index()
        {
            var modelo = await _obtenerContenidoWeb.EjecutarAsync("Nosotros");
            return View("~/Views/Nosotros/AdminNosotros.cshtml", modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GuardarSeccion(ModificarContenidoWebDto dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["MensajeError"] = "Datos inválidos. Por favor verifique el formulario.";
                return RedirectToAction("Index");
            }

            try
            {
                bool resultado = await _modificarContenidoWeb.EjecutarAsync(dto);

                if (resultado)
                    TempData["MensajeExito"] = "La sección se actualizó correctamente.";
                else
                    TempData["MensajeError"] = "No se encontró la sección a actualizar.";
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = "Error al guardar: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AgregarSeccion(string seccion)
        {
            if (string.IsNullOrWhiteSpace(seccion))
            {
                TempData["MensajeError"] = "Sección no válida.";
                return RedirectToAction("Index");
            }

            var dto = new AgregarSeccionHomeDto { Seccion = seccion };
            bool resultado = await _agregarSeccion.AgregarSeccionNosotros(dto);

            if (resultado)
                TempData["MensajeExito"] = "Nueva tarjeta agregada. Complete los datos y guarde.";
            else
                TempData["MensajeError"] = "No se puede agregar más tarjetas en esta sección (límite alcanzado).";

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EliminarSeccion(int id)
        {
            bool resultado = await _eliminarSeccion.EliminarSeccionNosotros(id);

            if (resultado)
                TempData["MensajeExito"] = "La tarjeta se eliminó correctamente.";
            else
                TempData["MensajeError"] = "No se pudo eliminar la tarjeta.";

            return RedirectToAction("Index");
        }
    }
}

