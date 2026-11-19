using GimnasioJena.Abstracciones.LogicaDeNegocio.Bitacora;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Home.AgregarSeccionHome;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Home.CambiarEstadoSeccionHome;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Home.EliminarSeccionHome;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Home.ModificarSeccionHome;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Home.ObtenerSeccionesHome;
using GimnasioJena.Abstracciones.Modelos.Bitacora;
using GimnasioJena.Abstracciones.Modelos.Home;
using GimnasioJena.AccesoADatos;
using GimnasioJena.LogicaDeNegocio.Bitacora;
using GimnasioJena.LogicaDeNegocio.Home.AgregarSeccionHome;
using GimnasioJena.LogicaDeNegocio.Home.CambiarEstadoSeccionHome;
using GimnasioJena.LogicaDeNegocio.Home.EliminarSeccionHome;
using GimnasioJena.LogicaDeNegocio.Home.ModificarSeccionHome;
using GimnasioJena.LogicaDeNegocio.Home.ObtenerSeccionesHome;
using GimnasioJena.UI.Filters;
using Microsoft.AspNet.Identity;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GimnasioJena.UI.Controllers
{
    [SoloAdministrador]
    public class SeccionesHomeController : Controller
    {
        private readonly IObtenerContenidoWebLN      _obtenerContenidoWeb;
        private readonly IModificarContenidoWebLN     _modificarContenidoWeb;
        private readonly IEliminarSeccionHomeLN       _eliminarSeccionHome;
        private readonly IAgregarSeccionHomeLN        _agregarSeccionHome;
        private readonly ICambiarEstadoSeccionHomeLN  _cambiarEstadoSeccionHome;
        private readonly IRegistrarBitacoraLN _registrarBitacoraLN;

        public SeccionesHomeController()
        {
            _obtenerContenidoWeb      = new ObtenerContenidoWebLN();
            _modificarContenidoWeb    = new ModificarContenidoWebLN();
            _eliminarSeccionHome      = new EliminarSeccionHomeLN();
            _agregarSeccionHome       = new AgregarSeccionHomeLN();
            _cambiarEstadoSeccionHome = new CambiarEstadoSeccionHomeLN();
            _registrarBitacoraLN = new RegistrarBitacoraLN();
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
                {
                    RegistrarBitacoraSegura(
                        "ContenidoWeb",
                        "UPDATE",
                        dto.Id,
                        "Se modificó una sección del Home."
                    );

                    TempData["MensajeExito"] =
                        "La sección se actualizó correctamente.";
                }
                else
                {
                    TempData["MensajeError"] =
                        "No se encontró la sección a actualizar.";
                }
            }
            catch (ArgumentException ex)
            {
                TempData["MensajeError"] = ex.Message;
            }
            catch (Exception)
            {
                TempData["MensajeError"] =
                    "Ocurrió un error inesperado al guardar la sección. Inténtelo nuevamente.";
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
            {
                RegistrarBitacoraSegura(
                    "ContenidoWeb",
                    "INSERT",
                    null,
                    "Se agregó una tarjeta al Home."
                );

                TempData["MensajeExito"] =
                    "Nueva tarjeta agregada. Complete los datos y guarde.";
            }
            else
            {
                TempData["MensajeError"] =
                    "No se puede agregar más de 3 tarjetas en esta sección.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ToggleEstado(int id)
        {
            try
            {
                bool resultado = await _cambiarEstadoSeccionHome.EjecutarAsync(id);

                if (resultado)
                {
                    RegistrarBitacoraSegura(
                        "ContenidoWeb",
                        "CAMBIO_ESTADO",
                        id,
                        "Se cambió el estado de una tarjeta del Home."
                    );

                    TempData["MensajeExito"] =
                        "El estado de la tarjeta se cambió correctamente.";
                }
                else
                {
                    TempData["MensajeError"] =
                        "No se pudo cambiar el estado. Verifique el registro.";
                }
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = ex.ToString();
            }

            return RedirectToAction("Index");
        }
        private int? ObtenerIdUsuarioActual()
        {
            string identityUserId = User.Identity.GetUserId();

            using (Contexto contexto = new Contexto())
            {
                var usuario = contexto.Usuarios
                    .FirstOrDefault(u => u.identityUserId == identityUserId);

                return usuario?.idUsuario;
            }
        }

        private string ObtenerIpUsuario()
        {
            return Request.UserHostAddress;
        }

        private void RegistrarBitacora(
            string tabla,
            string accion,
            int? idRegistro,
            string detalle)
        {
            _registrarBitacoraLN.RegistrarBitacora(
                new BitacoraDto
                {
                    idUsuario = ObtenerIdUsuarioActual(),
                    tablaAfectada = tabla,
                    accionRealizada = accion,
                    idRegistroAfectado = idRegistro,
                    detalle = detalle,
                    ipUsuario = ObtenerIpUsuario()
                }
            );
        }

        private void RegistrarBitacoraSegura(
            string tabla,
            string accion,
            int? idRegistro,
            string detalle)
        {
            try
            {
                RegistrarBitacora(tabla, accion, idRegistro, detalle);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(
                    "No fue posible registrar la bitácora del CMS: " + ex
                );
            }
        }
    }
}
