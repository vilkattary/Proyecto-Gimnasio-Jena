using GimnasioJena.Abstracciones.LogicaDeNegocio.Bitacora;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Clases.CambiarEstadoClase;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Clases.EditarClase;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Clases.ObtenerClasePorId;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Clases.ObtenerTodasLasClases;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Clases.RegistrarClase;
using GimnasioJena.Abstracciones.LogicaDeNegocio.HorariosSemanales.ObtenerHorariosSemanales;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.ObtenerReservasPorUsuario;
using GimnasioJena.Abstracciones.Modelos.Bitacora;
using GimnasioJena.Abstracciones.Modelos.Clases;
using GimnasioJena.Abstracciones.Modelos.HorariosSemanales;
using GimnasioJena.AccesoADatos;
using GimnasioJena.LogicaDeNegocio.Clases.CambiarEstadoClase;
using GimnasioJena.LogicaDeNegocio.Clases.EditarClase;
using GimnasioJena.LogicaDeNegocio.Clases.ObtenerClasePorId;
using GimnasioJena.LogicaDeNegocio.Clases.ObtenerTodasLasClases;
using GimnasioJena.LogicaDeNegocio.Clases.RegistrarClase;
using GimnasioJena.LogicaDeNegocio.Bitacora;
using GimnasioJena.LogicaDeNegocio.HorariosSemanales.ObtenerHorariosSemanales;
using GimnasioJena.LogicaDeNegocio.Reservas.ObtenerReservasPorUsuario;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GimnasioJena.UI.Controllers
{
    public class ClasesController : Controller
    {
        private readonly IObtenerTodasLasClasesLN _obtenerTodasLasClases;
        private readonly IObtenerClasePorIdLN _obtenerClasePorId;
        private readonly IRegistrarClaseLN _registrarClase;
        private readonly IEditarClaseLN _editarClase;
        private readonly ICambiarEstadoClaseLN _cambiarEstadoClase;
        private readonly IRegistrarBitacoraLN _registrarBitacoraLN;
        private readonly IObtenerHorariosSemanalesLN _obtenerHorariosSemanales;
        private readonly IObtenerReservasPorUsuarioLN _obtenerReservasPorUsuario;

        public ClasesController()
        {
            _obtenerTodasLasClases = new ObtenerTodasLasClasesLN();
            _obtenerClasePorId = new ObtenerClasePorIdLN();
            _registrarClase = new RegistrarClaseLN();
            _editarClase = new EditarClaseLN();
            _cambiarEstadoClase = new CambiarEstadoClaseLN();
            _registrarBitacoraLN = new RegistrarBitacoraLN();
            _obtenerHorariosSemanales = new ObtenerHorariosSemanalesLN();
            _obtenerReservasPorUsuario = new ObtenerReservasPorUsuarioLN();
        }

        public ActionResult ObtenerTodasLasClases()
        {
            List<ClaseListadoDto> listaDeClases;

            if (!User.Identity.IsAuthenticated)
            {
                listaDeClases =
                    _obtenerTodasLasClases.ObtenerProximasClasesParaCliente();

                ViewBag.EsVistaCliente = false;
                ViewBag.EsVistaPublica = true;

                List<HorarioSemanalListadoDto> horarios =
                    _obtenerHorariosSemanales.ObtenerHorariosSemanales();

                ViewBag.HorariosSemanales = horarios != null
                    ? horarios.Where(h => h.estado).ToList()
                    : new List<HorarioSemanalListadoDto>();
            }
            else if (User.IsInRole("CLIENTE"))
            {
                listaDeClases =
                    _obtenerTodasLasClases.ObtenerProximasClasesParaCliente();

                ViewBag.EsVistaCliente = true;
                ViewBag.EsVistaPublica = false;

                List<HorarioSemanalListadoDto> horarios =
                    _obtenerHorariosSemanales.ObtenerHorariosSemanales();

                ViewBag.HorariosSemanales = horarios != null
                    ? horarios.Where(h => h.estado).ToList()
                    : new List<HorarioSemanalListadoDto>();

                // Diccionario idClaseProgramada -> idReserva para el cliente actual
                using (var ctx = new Contexto())
                {
                    var identityId = User.Identity.GetUserId();
                    var usuario = ctx.Usuarios
                        .FirstOrDefault(u => u.identityUserId == identityId);

                    if (usuario != null)
                    {
                        var reservasActivas = ctx.Reservas
                            .Where(r => r.idUsuario == usuario.idUsuario && r.idEstadoReserva == 1)
                            .ToDictionary(r => r.idClaseProgramada, r => r.idReserva);

                        ViewBag.ReservasActivas = reservasActivas;
                    }
                    else
                    {
                        ViewBag.ReservasActivas = new System.Collections.Generic.Dictionary<int, int>();
                    }
                }
            }
            else
            {
                listaDeClases =
                    _obtenerTodasLasClases.ObtenerTodasLasClases();

                ViewBag.EsVistaCliente = false;
                ViewBag.EsVistaPublica = false;
            }

            return View(listaDeClases);
        }

        // Devuelve los eventos del calendario (cliente y publico) en formato JSON
        // para ser consumidos por FullCalendar via AJAX.
        [HttpGet]
        public JsonResult ObtenerEventosCalendario()
        {
            List<ClaseListadoDto> listaDeClases;
            bool esVistaCliente = User.Identity.IsAuthenticated && User.IsInRole("CLIENTE");

            listaDeClases = _obtenerTodasLasClases.ObtenerProximasClasesParaCliente();

            Dictionary<int, int> reservasActivas = new Dictionary<int, int>();

            if (esVistaCliente)
            {
                using (var ctx = new Contexto())
                {
                    var identityId = User.Identity.GetUserId();
                    var usuario = ctx.Usuarios
                        .FirstOrDefault(u => u.identityUserId == identityId);

                    if (usuario != null)
                    {
                        reservasActivas = ctx.Reservas
                            .Where(r => r.idUsuario == usuario.idUsuario && r.idEstadoReserva == 1)
                            .ToDictionary(r => r.idClaseProgramada, r => r.idReserva);
                    }
                }
            }

            var eventos = listaDeClases.Select(c =>
            {
                bool yaReservada = reservasActivas.ContainsKey(c.idClaseProgramada);
                int idReservaActiva = yaReservada ? reservasActivas[c.idClaseProgramada] : 0;

                return new
                {
                    id = c.idClaseProgramada,
                    title = c.nombreClase,
                    start = c.fechaClase.Date.Add(c.horaInicio).ToString("s"),
                    end = c.fechaClase.Date.Add(c.horaFin).ToString("s"),
                    nombreClase = c.nombreClase,
                    nombreEntrenador = c.nombreEntrenador,
                    ubicacion = c.ubicacion,
                    cupoMaximo = c.cupoMaximo,
                    cuposDisponibles = c.cuposDisponibles,
                    reservaHabilitada = c.reservaHabilitada,
                    mensajeReserva = c.mensajeReserva,
                    yaReservada = yaReservada,
                    idReservaActiva = idReservaActiva
                };
            }).ToList();

            return Json(eventos, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DetalleDeLaClase(int id)
        {
            ClaseListadoDto laClase = _obtenerClasePorId.ObtenerClasePorId(id);

            if (laClase == null)
            {
                return HttpNotFound();
            }

            return View(laClase);
        }

        [HttpGet]
        public ActionResult RegistrarClase()
        {
            CargarCatalogos();

            return View(new ClaseCrearDto
            {
                idEstadoClase = 1,
                fechaClase = DateTime.Today,
                horaInicio = new TimeSpan(6, 0, 0),
                horaFin = new TimeSpan(7, 0, 0),
                cupoMaximo = 30,
                fechaCreacion = DateTime.Now
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarClase(ClaseCrearDto claseAGuardar)
        {
            try
            {
                if (claseAGuardar.idTipoClase <= 0)
                {
                    ModelState.AddModelError("idTipoClase", "Debe seleccionar un tipo de clase.");
                }

                if (claseAGuardar.idUsuarioEntrenador <= 0)
                {
                    ModelState.AddModelError("idUsuarioEntrenador", "Debe seleccionar un entrenador.");
                }

                if (claseAGuardar.idEstadoClase <= 0)
                {
                    claseAGuardar.idEstadoClase = 1;
                }

                if (claseAGuardar.cupoMaximo < 1 || claseAGuardar.cupoMaximo > 30)
                {
                    ModelState.AddModelError("cupoMaximo", "El cupo máximo debe estar entre 1 y 30.");
                }

                if (claseAGuardar.horaFin <= claseAGuardar.horaInicio)
                {
                    ModelState.AddModelError("horaFin", "La hora de finalización debe ser mayor que la hora de inicio.");
                }

                if (!ModelState.IsValid)
                {
                    CargarCatalogos();
                    return View(claseAGuardar);
                }

                claseAGuardar.fechaCreacion = DateTime.Now;

                bool seAgrego = _registrarClase.RegistrarClase(claseAGuardar);

                if (seAgrego)
                {
                    RegistrarBitacora(
                        "ClaseProgramada",
                        "INSERT",
                        null,
                        "Se registró una nueva clase."
                    );

                    TempData["MensajeExito"] = "La clase se registró correctamente.";
                    return RedirectToAction("ObtenerTodasLasClases");
                }

                TempData["MensajeError"] = "No se pudo registrar la clase.";
                CargarCatalogos();
                return View(claseAGuardar);
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = "Ocurrió un error al registrar la clase: " + ex.Message;
                CargarCatalogos();
                return View(claseAGuardar);
            }
        }

        [HttpGet]
        public ActionResult EditarClase(int id)
        {
            ClaseListadoDto claseListado = _obtenerClasePorId.ObtenerClasePorId(id);

            if (claseListado == null)
            {
                return HttpNotFound();
            }

            ClaseEditarDto laClase = new ClaseEditarDto
            {
                idClaseProgramada = claseListado.idClaseProgramada,
                idTipoClase = claseListado.idTipoClase,
                idUsuarioEntrenador = claseListado.idUsuarioEntrenador,
                idEstadoClase = claseListado.idEstadoClase,
                fechaClase = claseListado.fechaClase,
                horaInicio = claseListado.horaInicio,
                horaFin = claseListado.horaFin,
                cupoMaximo = claseListado.cupoMaximo,
                ubicacion = claseListado.ubicacion,
                observaciones = claseListado.observaciones,
                fechaModificacion = DateTime.Now
            };

            CargarCatalogos(
                laClase.idTipoClase,
                laClase.idUsuarioEntrenador,
                laClase.idEstadoClase
            );

            return View(laClase);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarClase(ClaseEditarDto claseAEditar)
        {
            try
            {
                if (claseAEditar.idTipoClase <= 0)
                {
                    ModelState.AddModelError("idTipoClase", "Debe seleccionar un tipo de clase.");
                }

                if (claseAEditar.idUsuarioEntrenador <= 0)
                {
                    ModelState.AddModelError("idUsuarioEntrenador", "Debe seleccionar un entrenador.");
                }

                if (claseAEditar.idEstadoClase <= 0)
                {
                    ModelState.AddModelError("idEstadoClase", "Debe seleccionar un estado.");
                }

                if (claseAEditar.cupoMaximo < 1 || claseAEditar.cupoMaximo > 30)
                {
                    ModelState.AddModelError("cupoMaximo", "El cupo máximo debe estar entre 1 y 30.");
                }

                if (claseAEditar.horaFin <= claseAEditar.horaInicio)
                {
                    ModelState.AddModelError("horaFin", "La hora de finalización debe ser mayor que la hora de inicio.");
                }

                if (!ModelState.IsValid)
                {
                    CargarCatalogos(
                        claseAEditar.idTipoClase,
                        claseAEditar.idUsuarioEntrenador,
                        claseAEditar.idEstadoClase
                    );

                    return View(claseAEditar);
                }

                claseAEditar.fechaModificacion = DateTime.Now;

                bool seActualizo = _editarClase.EditarClase(claseAEditar);

                if (seActualizo)
                {
                    RegistrarBitacora(
                        "ClaseProgramada",
                        "UPDATE",
                        claseAEditar.idClaseProgramada,
                        "Se actualizó la clase programada."
                    );

                    TempData["MensajeExito"] = "La clase se actualizó correctamente.";
                    return RedirectToAction("ObtenerTodasLasClases");
                }

                TempData["MensajeError"] = "No se pudo actualizar la clase.";

                CargarCatalogos(
                    claseAEditar.idTipoClase,
                    claseAEditar.idUsuarioEntrenador,
                    claseAEditar.idEstadoClase
                );

                return View(claseAEditar);
            }
            catch
            {
                TempData["MensajeError"] = "Ocurrió un error al actualizar la clase.";

                CargarCatalogos(
                    claseAEditar.idTipoClase,
                    claseAEditar.idUsuarioEntrenador,
                    claseAEditar.idEstadoClase
                );

                return View(claseAEditar);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CambiarEstado(int idClaseProgramada, string returnUrl = null)
        {
            try
            {
                bool nuevoEstadoActivo =
                    _cambiarEstadoClase.CambiarEstadoClase(idClaseProgramada);

                RegistrarBitacora(
                    "ClaseProgramada",
                    "CAMBIO_ESTADO",
                    idClaseProgramada,
                    nuevoEstadoActivo
                        ? "Se activó una clase."
                        : "Se desactivó una clase."
                );

                TempData["MensajeExito"] = nuevoEstadoActivo
                    ? "La clase fue activada correctamente."
                    : "La clase fue desactivada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] =
                    "No fue posible cambiar el estado de la clase: " +
                    ObtenerMensajeCompleto(ex);
            }

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("ObtenerTodasLasClases");
        }

        public ActionResult ObtenerClasesDisponibles()
        {
            List<ClaseListadoDto> todasLasClases = _obtenerTodasLasClases.ObtenerTodasLasClases();

            List<ClaseListadoDto> disponibles = todasLasClases
                .Where(c => c.estadoClase == "Activo" && c.cuposDisponibles > 0)
                .OrderBy(c => c.fechaClase)
                .ThenBy(c => c.horaInicio)
                .ToList();

            return View(disponibles);
        }

        private void CargarCatalogos(
            int? idTipoClaseSeleccionado = null,
            int? idUsuarioEntrenadorSeleccionado = null,
            int? idEstadoClaseSeleccionado = null)
        {
            using (var contexto = new Contexto())
            {
                ViewBag.TiposClase = new SelectList(
                    contexto.TiposClase
                        .Where(t => t.estado)
                        .OrderBy(t => t.nombreClase)
                        .ToList(),
                    "idTipoClase",
                    "nombreClase",
                    idTipoClaseSeleccionado
                );

                ViewBag.Entrenadores = new SelectList(
                    contexto.Entrenadores
                        .Where(e => e.estado)
                        .Join(
                            contexto.Usuarios,
                            e => e.idUsuario,
                            u => u.idUsuario,
                            (e, u) => new
                            {
                                idUsuario = u.idUsuario,
                                nombreCompleto = u.nombre + " " + u.apellido1 + " " + u.apellido2
                            })
                        .OrderBy(e => e.nombreCompleto)
                        .ToList(),
                    "idUsuario",
                    "nombreCompleto",
                    idUsuarioEntrenadorSeleccionado
                );

                ViewBag.EstadosClase = new SelectList(
                    contexto.EstadoClases
                        .Where(e => e.estado)
                        .OrderBy(e => e.nombreEstado)
                        .ToList(),
                    "idEstadoClase",
                    "nombreEstado",
                    idEstadoClaseSeleccionado
                );
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

        private static string ObtenerMensajeCompleto(Exception ex)
        {
            var mensajes = new List<string>();
            Exception actual = ex;

            while (actual != null)
            {
                if (!string.IsNullOrWhiteSpace(actual.Message) &&
                    !mensajes.Contains(actual.Message))
                {
                    mensajes.Add(actual.Message);
                }

                actual = actual.InnerException;
            }

            return string.Join(" | ", mensajes);
        }
    }
}