using GimnasioJena.Abstracciones.LogicaDeNegocio.Bitacora;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Home.ModificarSeccionHome;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Home.ObtenerSeccionesHome;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Nosotros.AgregarSeccionNosotros;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Nosotros.CambiarEstadoSeccionNosotros;
using GimnasioJena.Abstracciones.Modelos.Home;
using GimnasioJena.LogicaDeNegocio.Bitacora;
using GimnasioJena.LogicaDeNegocio.Nosotros.AgregarSeccionNosotros;
using GimnasioJena.LogicaDeNegocio.Nosotros.CambiarEstadoSeccionNosotros;
using GimnasioJena.LogicaDeNegocio.Nosotros.ModificarSeccionNosotros;
using GimnasioJena.LogicaDeNegocio.Nosotros.ObtenerSeccionesNosotros;
using GimnasioJena.UI.Filters;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using GimnasioJena.Abstracciones.Modelos.Bitacora;
using GimnasioJena.AccesoADatos;
using Microsoft.AspNet.Identity;
using System.Diagnostics;
using System.Linq;

namespace GimnasioJena.UI.Controllers
{
    [SoloAdministrador]
    public class SeccionesNosotrosController : Controller
    {
        private readonly IObtenerContenidoWebLN        _obtenerContenidoWeb;
        private readonly IModificarContenidoWebLN       _modificarContenidoWeb;
        private readonly IAgregarSeccionNosotrosLN      _agregarSeccion;
        private readonly ICambiarEstadoSeccionNosotrosLN _cambiarEstado;
        private readonly IRegistrarBitacoraLN _registrarBitacoraLN;

        public SeccionesNosotrosController()
        {
            _obtenerContenidoWeb  = new ObtenerContenidoNosotrosLN();
            _modificarContenidoWeb = new ModificarContenidoNosotrosLN();
            _agregarSeccion        = new AgregarSeccionNosotrosLN();
            _cambiarEstado         = new CambiarEstadoSeccionNosotrosLN();
            _registrarBitacoraLN   = new RegistrarBitacoraLN();
        }

        public async Task<ActionResult> Index()
        {
            var modelo = await _obtenerContenidoWeb.EjecutarTodosAsync("Nosotros");
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
                {
                    RegistrarBitacoraSegura(
                        "ContenidoWeb",
                        "UPDATE",
                        dto.Id,
                        "Se modificó una sección de Nosotros."
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
        public async Task<ActionResult> AgregarSeccion(string seccion)
        {
            if (string.IsNullOrWhiteSpace(seccion))
            {
                TempData["MensajeError"] = "Sección no válida.";
                return RedirectToAction("Index");
            }

            try
            {
                var dto = new AgregarSeccionHomeDto { Seccion = seccion };
                bool resultado = await _agregarSeccion.AgregarSeccionNosotros(dto);

                if (resultado)
                {
                    RegistrarBitacoraSegura(
                        "ContenidoWeb",
                        "INSERT",
                        null,
                        "Se agregó una tarjeta a Nosotros."
                    );

                    TempData["MensajeExito"] =
                        "Nueva tarjeta agregada. Complete los datos y guarde.";
                }
                else
                {
                    TempData["MensajeError"] =
                        "No se puede agregar más tarjetas en esta sección (límite alcanzado).";
                }
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = ex.ToString();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ToggleEstado(int id)
        {
            try
            {
                bool resultado = await _cambiarEstado.EjecutarAsync(id);

                if (resultado)
                {
                    RegistrarBitacoraSegura(
                        "ContenidoWeb",
                        "CAMBIO_ESTADO",
                        id,
                        "Se cambió el estado de una tarjeta de Nosotros."
                    );

                    TempData["MensajeExito"] =
                        "Estado de la tarjeta actualizado correctamente.";
                }
                else
                {
                    TempData["MensajeError"] =
                        "No se pudo cambiar el estado de la tarjeta.";
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
                    "No fue posible registrar la bitácora del CMS Nosotros: " + ex
                );
            }
        }
    }
}

