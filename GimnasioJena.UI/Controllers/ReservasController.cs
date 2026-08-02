using GimnasioJena.Abstracciones.LogicaDeNegocio.Bitacora;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.CancelarReserva;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.ObtenerTodasLasReservas;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.ObtenerReservasPorUsuario;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.RegistrarReserva;
using GimnasioJena.Abstracciones.Modelos.Bitacora;
using GimnasioJena.Abstracciones.Modelos.Reservas;
using GimnasioJena.AccesoADatos;
using GimnasioJena.LogicaDeNegocio.Bitacora;
using GimnasioJena.LogicaDeNegocio.Reservas.CancelarReserva;
using GimnasioJena.LogicaDeNegocio.Reservas.ObtenerTodasLasReservas;
using GimnasioJena.LogicaDeNegocio.Reservas.ObtenerReservasPorUsuario;
using GimnasioJena.LogicaDeNegocio.Reservas.RegistrarReserva;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.Mvc;

namespace GimnasioJena.UI.Controllers
{
    [Authorize]
    public class ReservasController : Controller
    {
        private readonly IRegistrarReservaLN _registrarReservaServicio;
        private readonly IObtenerTodasLasReservasLN _obtenerTodasLasReservasServicio;
        private readonly IRegistrarBitacoraLN _registrarBitacoraLN;
        private readonly ICancelarReservaLN _cancelarReservaServicio;
        private readonly IObtenerReservasPorUsuarioLN _obtenerReservasPorUsuarioServicio;

        public ReservasController()
        {
            _registrarReservaServicio = new RegistrarReservaLN();
            _obtenerTodasLasReservasServicio = new ObtenerTodasLasReservasLN();
            _registrarBitacoraLN = new RegistrarBitacoraLN();
            _cancelarReservaServicio = new CancelarReservaLN();
            _obtenerReservasPorUsuarioServicio = new ObtenerReservasPorUsuarioLN();
        }

        [Authorize(Roles = "ADMINISTRADOR")]
        public ActionResult Administrar()
        {
            var reservas = _obtenerTodasLasReservasServicio.ObtenerTodasLasReservas();
            return View(reservas);
        }

        [HttpGet]
        [Authorize(Roles = "CLIENTE")]
        public ActionResult ReservarClase(int id)
        {
            using (var contexto = new Contexto())
            {
                var clase = contexto.Clases.FirstOrDefault(c => c.idClaseProgramada == id);

                if (clase == null)
                {
                    TempData["MensajeError"] = "No se encontró la clase seleccionada.";
                    return RedirectToAction("ObtenerTodasLasClases", "Clases");
                }

                var modelo = new ReservaCrearDto
                {
                    idClaseProgramada = id,
                    idEstadoReserva = 1
                };

                return View(modelo);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CLIENTE")]
        public ActionResult ReservarClase(ReservaCrearDto modelo)
        {
            if (modelo == null || modelo.idClaseProgramada <= 0)
            {
                TempData["MensajeError"] = "No se recibió información válida para reservar.";
                return RedirectToAction("ObtenerTodasLasClases", "Clases");
            }

            using (var contexto = new Contexto())
            {
                var identityUserId = User.Identity.GetUserId();

                var usuario = contexto.Usuarios
                    .FirstOrDefault(u => u.identityUserId == identityUserId);

                if (usuario == null)
                {
                    TempData["MensajeError"] = "No se encontró el usuario actual.";
                    return RedirectToAction("ObtenerTodasLasClases", "Clases");
                }

                modelo.idUsuario = usuario.idUsuario;
                modelo.idEstadoReserva = 1;

                var resultado = _registrarReservaServicio.RegistrarReserva(modelo);

                if (resultado.fueExitosa)
                {
                    RegistrarBitacora(
                        "Reserva",
                        "INSERT",
                        modelo.idClaseProgramada,
                        "El cliente con idUsuario " + modelo.idUsuario +
                        " registró una reserva para la clase con idClaseProgramada: " + modelo.idClaseProgramada
                    );

                    TempData["MensajeExito"] = resultado.mensaje;

                    return RedirectToAction(
                        "ObtenerTodasLasClases",
                        "Clases"
                    );
                }

                TempData["MensajeError"] = resultado.mensaje;

                return View(modelo);
            }
        }
        // Reserva directa desde el listado de clases (sin pantalla intermedia)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CLIENTE")]
        public ActionResult ReservarDirecta(int idClaseProgramada)
        {
            using (var contexto = new Contexto())
            {
                var identityUserId = User.Identity.GetUserId();
                var usuario = contexto.Usuarios
                    .FirstOrDefault(u => u.identityUserId == identityUserId);

                if (usuario == null)
                {
                    TempData["MensajeError"] = "No se encontró el usuario actual.";
                    return RedirectToAction("ObtenerTodasLasClases", "Clases");
                }

                var modelo = new ReservaCrearDto
                {
                    idClaseProgramada = idClaseProgramada,
                    idUsuario = usuario.idUsuario,
                    idEstadoReserva = 1
                };

                var resultado = _registrarReservaServicio.RegistrarReserva(modelo);

                if (resultado.fueExitosa)
                {
                    RegistrarBitacora(
                        "Reserva",
                        "INSERT",
                        idClaseProgramada,
                        "El cliente " + usuario.idUsuario + " reservó la clase " + idClaseProgramada
                    );
                    TempData["MensajeExito"] = resultado.mensaje;
                }
                else
                {
                    TempData["MensajeError"] = resultado.mensaje;
                }

                return RedirectToAction("ObtenerTodasLasClases", "Clases");
            }
        }

        // Cancelar reserva desde el listado de clases
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CLIENTE")]
        public ActionResult CancelarDesdeClases(int idReserva)
        {
            var identityUserId = User.Identity.GetUserId();

            using (var contexto = new Contexto())
            {
                var usuario = contexto.Usuarios
                    .FirstOrDefault(u => u.identityUserId == identityUserId);

                if (usuario == null)
                {
                    TempData["MensajeError"] = "No se encontró el usuario actual.";
                    return RedirectToAction("ObtenerTodasLasClases", "Clases");
                }

                var dto = new ReservaCancelarDto
                {
                    idReserva = idReserva,
                    motivoCancelacion = "Cancelación desde listado de clases"
                };

                bool ok = _cancelarReservaServicio.CancelarReserva(dto, usuario.idUsuario);

                if (ok)
                {
                    RegistrarBitacora(
                        "Reserva",
                        "UPDATE",
                        idReserva,
                        "El cliente " + usuario.idUsuario + " canceló la reserva " + idReserva
                    );
                    TempData["MensajeExito"] = "Reserva cancelada correctamente.";
                }
                else
                {
                    TempData["MensajeError"] = "No se pudo cancelar la reserva.";
                }

                return RedirectToAction("ObtenerTodasLasClases", "Clases");
            }
        }

        // Reserva desde el calendario (AJAX, sin recargar la pagina)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CLIENTE")]
        public JsonResult ReservarAjax(int idClaseProgramada)
        {
            using (var contexto = new Contexto())
            {
                var identityUserId = User.Identity.GetUserId();
                var usuario = contexto.Usuarios
                    .FirstOrDefault(u => u.identityUserId == identityUserId);

                if (usuario == null)
                {
                    return Json(new { exito = false, mensaje = "No se encontro el usuario actual." });
                }

                var modelo = new ReservaCrearDto
                {
                    idClaseProgramada = idClaseProgramada,
                    idUsuario = usuario.idUsuario,
                    idEstadoReserva = 1
                };

                var resultado = _registrarReservaServicio.RegistrarReserva(modelo);

                if (resultado.fueExitosa)
                {
                    RegistrarBitacora(
                        "Reserva",
                        "INSERT",
                        idClaseProgramada,
                        "El cliente " + usuario.idUsuario + " reservo la clase " + idClaseProgramada + " desde el calendario"
                    );
                }

                int idReservaActiva = 0;
                if (resultado.fueExitosa)
                {
                    idReservaActiva = contexto.Reservas
                        .Where(r => r.idClaseProgramada == idClaseProgramada
                            && r.idUsuario == usuario.idUsuario
                            && r.idEstadoReserva == 1)
                        .OrderByDescending(r => r.idReserva)
                        .Select(r => r.idReserva)
                        .FirstOrDefault();
                }

                return Json(new
                {
                    exito = resultado.fueExitosa,
                    mensaje = resultado.mensaje,
                    idReservaActiva
                });
            }
        }

        // Cancelacion desde el calendario (AJAX, sin recargar la pagina)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CLIENTE")]
        public JsonResult CancelarAjax(int idReserva)
        {
            var identityUserId = User.Identity.GetUserId();

            using (var contexto = new Contexto())
            {
                var usuario = contexto.Usuarios
                    .FirstOrDefault(u => u.identityUserId == identityUserId);

                if (usuario == null)
                {
                    return Json(new { exito = false, mensaje = "No se encontro el usuario actual." });
                }

                var dto = new ReservaCancelarDto
                {
                    idReserva = idReserva,
                    motivoCancelacion = "Cancelacion desde el calendario"
                };

                bool ok = _cancelarReservaServicio.CancelarReserva(dto, usuario.idUsuario);

                if (ok)
                {
                    RegistrarBitacora(
                        "Reserva",
                        "UPDATE",
                        idReserva,
                        "El cliente " + usuario.idUsuario + " cancelo la reserva " + idReserva + " desde el calendario"
                    );

                    return Json(new { exito = true, mensaje = "Reserva cancelada correctamente." });
                }

                return Json(new { exito = false, mensaje = "No se pudo cancelar la reserva." });
            }
        }

        private int? ObtenerIdUsuarioActual()
        {
            var identityUserId = User.Identity.GetUserId();

            using (var contexto = new Contexto())
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

        private void RegistrarBitacora(string tabla, string accion, int? idRegistro, string detalle)
        {
            _registrarBitacoraLN.RegistrarBitacora(new BitacoraDto
            {
                idUsuario = ObtenerIdUsuarioActual(),
                tablaAfectada = tabla,
                accionRealizada = accion,
                idRegistroAfectado = idRegistro,
                detalle = detalle,
                ipUsuario = ObtenerIpUsuario()
            });
        }

    }
}