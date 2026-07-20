using GimnasioJena.Abstracciones.LogicaDeNegocio.Bitacora;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.CancelarReserva;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.ObtenerTodasLasReservas;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.RegistrarReserva;
using GimnasioJena.Abstracciones.Modelos.Bitacora;
using GimnasioJena.Abstracciones.Modelos.Reservas;
using GimnasioJena.AccesoADatos;
using GimnasioJena.LogicaDeNegocio.Bitacora;
using GimnasioJena.LogicaDeNegocio.Reservas.CancelarReserva;
using GimnasioJena.LogicaDeNegocio.Reservas.ObtenerTodasLasReservas;
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
        private readonly ICancelarReservaLN _cancelarReservaServicio;
        private readonly IRegistrarBitacoraLN _registrarBitacoraLN;

        public ReservasController()
        {
            _registrarReservaServicio         = new RegistrarReservaLN();
            _obtenerTodasLasReservasServicio   = new ObtenerTodasLasReservasLN();
            _cancelarReservaServicio           = new CancelarReservaLN();
            _registrarBitacoraLN              = new RegistrarBitacoraLN();
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

                bool resultado = _registrarReservaServicio.RegistrarReserva(modelo);

                if (resultado)
                {
                    RegistrarBitacora(
                        "Reserva",
                        "CREATE",
                        modelo.idClaseProgramada,
                        "El cliente con idUsuario " + modelo.idUsuario +
                        " registró una reserva para la clase con idClaseProgramada: " + modelo.idClaseProgramada
                    );

                    TempData["MensajeExito"] = "La reserva se registró correctamente.";
                    return RedirectToAction("ObtenerTodasLasClases", "Clases");
                }

                TempData["MensajeError"] = "No se pudo registrar la reserva.";
                return View(modelo);
            }
        }
        // ── Endpoints JSON para el calendario ────────────────────────────────

        [HttpPost]
        [Authorize(Roles = "CLIENTE")]
        public JsonResult ReservarDesdeCalendario(int idClase)
        {
            try
            {
                var identityUserId = User.Identity.GetUserId();
                using (var ctx = new Contexto())
                {
                    var usuario = ctx.Usuarios.FirstOrDefault(u => u.identityUserId == identityUserId);
                    if (usuario == null)
                        return Json(new { ok = false, mensaje = "Usuario no encontrado." });

                    // Validaciones previas para dar mensaje claro al usuario
                    var claseAD = new GimnasioJena.AccesoADatos.Reservas.RegistrarReserva.RegistrarReservaAD();
                    var clase   = claseAD.ObtenerClaseParaValidacion(idClase);

                    if (clase == null)
                        return Json(new { ok = false, mensaje = "La clase no existe." });

                    if (clase.idEstadoClase != 1)
                        return Json(new { ok = false, mensaje = "La clase no está activa." });

                    var fechaHoraClase = clase.fechaClase.Date.Add(clase.horaInicio);
                    if (fechaHoraClase <= DateTime.Now)
                        return Json(new { ok = false, mensaje = "La clase ya comenzó o ya pasó, no es posible reservar." });

                    if (claseAD.UsuarioTieneReservaActiva(usuario.idUsuario, idClase))
                        return Json(new { ok = false, mensaje = "Ya tenés una reserva activa para esta clase." });

                    int reservasActivas = claseAD.ContarReservasActivasPorClase(idClase);
                    if (reservasActivas >= clase.cupoMaximo)
                        return Json(new { ok = false, mensaje = "La clase no tiene cupos disponibles." });

                    var modelo = new ReservaCrearDto
                    {
                        idClaseProgramada = idClase,
                        idUsuario         = usuario.idUsuario,
                        idEstadoReserva   = 1
                    };

                    bool resultado = _registrarReservaServicio.RegistrarReserva(modelo);
                    if (!resultado)
                        return Json(new { ok = false, mensaje = "No se pudo completar la reserva. Verificá que tengas membresía vigente con clases disponibles." });

                    var reserva = ctx.Reservas
                        .Where(r => r.idUsuario == usuario.idUsuario &&
                                    r.idClaseProgramada == idClase &&
                                    r.idEstadoReserva == 1)
                        .OrderByDescending(r => r.idReserva)
                        .FirstOrDefault();

                    RegistrarBitacora("Reserva", "INSERT", idClase,
                        "Cliente idUsuario " + usuario.idUsuario + " reservó clase " + idClase + " desde el calendario.");

                    return Json(new { ok = true, mensaje = "¡Reserva registrada con éxito!", idReserva = reserva?.idReserva ?? 0 });
                }
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException?.InnerException?.Message ?? ex.InnerException?.Message ?? ex.Message;
                System.Diagnostics.Trace.TraceError("[ReservarDesdeCalendario] {0} | Inner: {1}", ex.Message, inner);
                return Json(new { ok = false, mensaje = "Error inesperado al reservar. Intentá de nuevo." });
            }
        }

        [HttpPost]
        [Authorize(Roles = "CLIENTE")]
        public JsonResult CancelarDesdeCalendario(int idReserva)
        {
            try
            {
                var identityUserId = User.Identity.GetUserId();
                using (var ctx = new Contexto())
                {
                    var usuario = ctx.Usuarios.FirstOrDefault(u => u.identityUserId == identityUserId);
                    if (usuario == null)
                        return Json(new { ok = false, mensaje = "Usuario no encontrado." });

                    var dto = new ReservaCancelarDto
                    {
                        idReserva          = idReserva,
                        motivoCancelacion  = "Cancelación desde el calendario"
                    };

                    bool resultado = _cancelarReservaServicio.CancelarReserva(dto, usuario.idUsuario);
                    if (!resultado)
                        return Json(new { ok = false, mensaje = "No se pudo cancelar. La clase puede haber iniciado o la reserva ya no está activa." });

                    RegistrarBitacora("Reserva", "UPDATE", idReserva,
                        "Cliente idUsuario " + usuario.idUsuario + " canceló reserva " + idReserva + " desde el calendario.");

                    return Json(new { ok = true, mensaje = "Reserva cancelada." });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("[CancelarDesdeCalendario] {0}", ex.Message);
                return Json(new { ok = false, mensaje = "Error inesperado al cancelar." });
            }
        }

        // ─────────────────────────────────────────────────────────────────────

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