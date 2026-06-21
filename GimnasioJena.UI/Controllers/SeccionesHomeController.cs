using GimnasioJena.Abstracciones.LogicaDeNegocio.Home.AgregarSeccionHome;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Home.EliminarSeccionHome;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Home.ModificarSeccionHome;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Home.ObtenerSeccionesHome;
using GimnasioJena.Abstracciones.Modelos.Home;
using GimnasioJena.LogicaDeNegocio.Home.AgregarSeccionHome;
using GimnasioJena.LogicaDeNegocio.Home.EliminarSeccionHome;
using GimnasioJena.LogicaDeNegocio.Home.ModificarSeccionHome;
using GimnasioJena.LogicaDeNegocio.Home.ObtenerSeccionesHome;
using GimnasioJena.UI.Filters;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GimnasioJena.UI.Controllers
{
    [SoloAdministrador]
    public class SeccionesHomeController : Controller
    {
        private readonly IObtenerContenidoWebLN  _obtenerContenidoWeb;
        private readonly IModificarContenidoWebLN _modificarContenidoWeb;
        private readonly IEliminarSeccionHomeLN   _eliminarSeccionHome;
        private readonly IAgregarSeccionHomeLN    _agregarSeccionHome;

        public SeccionesHomeController()
        {
            _obtenerContenidoWeb   = new ObtenerContenidoWebLN();
            _modificarContenidoWeb = new ModificarContenidoWebLN();
            _eliminarSeccionHome   = new EliminarSeccionHomeLN();
            _agregarSeccionHome    = new AgregarSeccionHomeLN();
        }

        public async Task<ActionResult> Index()
        {
            var modelo = await _obtenerContenidoWeb.EjecutarTodosAsync("Home");
            return View("~/Views/Home/AdminIndex.cshtml", modelo);
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
            catch (System.Exception ex)
            {
                TempData["MensajeError"] = "Error al guardar: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EliminarSeccion(int id)
        {
            bool resultado = await _eliminarSeccionHome.EliminarSeccionHome(id);

            if (resultado)
                TempData["MensajeExito"] = "La tarjeta se desactivó correctamente.";
            else
                TempData["MensajeError"] = "No se pudo desactivar la tarjeta.";

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
            bool resultado = await _agregarSeccionHome.AgregarSeccionHome(dto);

            if (resultado)
                TempData["MensajeExito"] = "Nueva tarjeta agregada. Complete los datos y guarde.";
            else
                TempData["MensajeError"] = "No se puede agregar más de 3 tarjetas en esta sección.";

            return RedirectToAction("Index");
        }
    }
}
