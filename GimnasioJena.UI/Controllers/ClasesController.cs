using GimnasioJena.Abstracciones.LogicaDeNegocio.Bitacora;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Clases.EditarClase;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Clases.ObtenerClasePorId;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Clases.ObtenerTodasLasClases;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Clases.RegistrarClase;
using GimnasioJena.Abstracciones.Modelos.Bitacora;
using GimnasioJena.Abstracciones.Modelos.Clases;
using GimnasioJena.AccesoADatos;
using GimnasioJena.AccesoADatos.Entidades.Catalogos;
using GimnasioJena.LogicaDeNegocio.Clases.EditarClase;
using GimnasioJena.LogicaDeNegocio.Clases.ObtenerClasePorId;
using GimnasioJena.LogicaDeNegocio.Clases.ObtenerTodasLasClases;
using GimnasioJena.LogicaDeNegocio.Clases.RegistrarClase;
using GimnasioJena.LogicaDeNegocio.Bitacora;
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
        private readonly IRegistrarBitacoraLN _registrarBitacoraLN;

        public ClasesController()
        {
            _obtenerTodasLasClases = new ObtenerTodasLasClasesLN();
            _obtenerClasePorId = new ObtenerClasePorIdLN();
            _registrarClase = new RegistrarClaseLN();
            _editarClase = new EditarClaseLN();
            _registrarBitacoraLN = new RegistrarBitacoraLN();
        }

        public ActionResult ObtenerTodasLasClases()
        {
            List<ClaseListadoDto> listaDeClases = _obtenerTodasLasClases.ObtenerTodasLasClases();
            return View(listaDeClases);
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
            return View(new ClaseCrearDto { idEstadoClase = 1, cupoMaximo = 12 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarClase(ClaseCrearDto claseAGuardar)
        {
            try
            {
                claseAGuardar.idEstadoClase = 1;

                if (string.IsNullOrWhiteSpace(claseAGuardar.nombreTipoClase))
                    ModelState.AddModelError("nombreTipoClase", "El tipo de clase es obligatorio.");

                if (claseAGuardar.cupoMaximo < 1 || claseAGuardar.cupoMaximo > 12)
                    ModelState.AddModelError("cupoMaximo", "El cupo máximo debe estar entre 1 y 12.");

                if (claseAGuardar.Horarios == null || !claseAGuardar.Horarios.Any())
                    ModelState.AddModelError("", "Debe agregar al menos un horario.");

                if (claseAGuardar.Horarios != null)
                {
                    for (int i = 0; i < claseAGuardar.Horarios.Count; i++)
                    {
                        var h = claseAGuardar.Horarios[i];
                        if (h.idUsuarioEntrenador <= 0)
                            ModelState.AddModelError("", $"Horario {i + 1}: debe seleccionar un entrenador.");
                        if (!string.IsNullOrWhiteSpace(h.horaInicio) && !string.IsNullOrWhiteSpace(h.horaFin))
                        {
                            if (TimeSpan.Parse(h.horaFin) <= TimeSpan.Parse(h.horaInicio))
                                ModelState.AddModelError("", $"Horario {i + 1}: la hora de fin debe ser mayor a la de inicio.");
                        }
                    }
                }

                if (!ModelState.IsValid)
                {
                    CargarCatalogos();
                    return View(claseAGuardar);
                }

                claseAGuardar.idTipoClase = ObtenerOCrearTipoClase(claseAGuardar.nombreTipoClase.Trim());

                bool seAgrego = _registrarClase.RegistrarClase(claseAGuardar);

                if (seAgrego)
                {
                    RegistrarBitacora("HorarioClase", "INSERT", null,
                        $"Se registró la clase '{claseAGuardar.nombreTipoClase}' con {claseAGuardar.Horarios?.Count ?? 0} horario(s).");
                    TempData["MensajeExito"] = "La clase se registró correctamente.";
                    return RedirectToAction("ObtenerTodasLasClases");
                }

                TempData["MensajeError"] = "No fue posible registrar la clase. Verificá los datos e intentá de nuevo.";
                CargarCatalogos();
                return View(claseAGuardar);
            }
            catch (Exception ex)
            {
                var inner = ex;
                while (inner.InnerException != null) inner = inner.InnerException;
                System.Diagnostics.Trace.TraceError("[RegistrarClase] {0} | Inner: {1}", ex.Message, inner.Message);
#if DEBUG
                TempData["MensajeError"] = $"[DEBUG] {ex.GetType().Name}: {ex.Message} | Inner: {inner.Message}";
#else
                TempData["MensajeError"] = "No fue posible registrar la clase. Verificá los datos e intentá de nuevo.";
#endif
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
                nombreTipoClase = claseListado.nombreClase,
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
                if (string.IsNullOrWhiteSpace(claseAEditar.nombreTipoClase))
                {
                    ModelState.AddModelError("nombreTipoClase", "El tipo de clase es obligatorio.");
                }

                if (claseAEditar.idUsuarioEntrenador <= 0)
                {
                    ModelState.AddModelError("idUsuarioEntrenador", "Debe seleccionar un entrenador.");
                }

                if (claseAEditar.idEstadoClase <= 0)
                {
                    ModelState.AddModelError("idEstadoClase", "Debe seleccionar un estado.");
                }

                if (claseAEditar.cupoMaximo < 1 || claseAEditar.cupoMaximo > 12)
                {
                    ModelState.AddModelError("cupoMaximo", "El cupo máximo debe estar entre 1 y 12.");
                }

                if (claseAEditar.horaFin <= claseAEditar.horaInicio)
                {
                    ModelState.AddModelError("horaFin", "La hora de finalización debe ser mayor que la hora de inicio.");
                }

                if (!ModelState.IsValid)
                {
                    CargarCatalogos(
                        claseAEditar.idUsuarioEntrenador,
                        claseAEditar.idEstadoClase
                    );

                    return View(claseAEditar);
                }

                claseAEditar.fechaModificacion = DateTime.Now;
                claseAEditar.idTipoClase = ObtenerOCrearTipoClase(claseAEditar.nombreTipoClase.Trim());

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
                    claseAEditar.idUsuarioEntrenador,
                    claseAEditar.idEstadoClase
                );

                return View(claseAEditar);
            }
            catch
            {
                TempData["MensajeError"] = "Ocurrió un error al actualizar la clase.";

                CargarCatalogos(
                    claseAEditar.idUsuarioEntrenador,
                    claseAEditar.idEstadoClase
                );

                return View(claseAEditar);
            }
        }

        public ActionResult ObtenerClasesDisponibles()
        {
            List<ClaseListadoDto> todasLasClases = _obtenerTodasLasClases.ObtenerTodasLasClases();

            List<ClaseListadoDto> disponibles = todasLasClases
                .Where(c => c.estadoClase == "Activa" && c.cuposDisponibles > 0)
                .OrderBy(c => c.fechaClase)
                .ThenBy(c => c.horaInicio)
                .ToList();

            return View(disponibles);
        }

        private int ObtenerOCrearTipoClase(string nombreTipoClase)
        {
            using (var contexto = new Contexto())
            {
                var tipo = contexto.TiposClase
                    .FirstOrDefault(t => t.nombreClase.ToLower() == nombreTipoClase.ToLower());

                if (tipo == null)
                {
                    tipo = new TipoClaseEntidad
                    {
                        nombreClase = nombreTipoClase,
                        descripcion = string.Empty,
                        duracionMinutos = 40,
                        estado = true
                    };
                    contexto.TiposClase.Add(tipo);
                    contexto.SaveChanges();
                }

                return tipo.idTipoClase;
            }
        }

        private void CargarCatalogos(
            int? idUsuarioEntrenadorSeleccionado = null,
            int? idEstadoClaseSeleccionado = null)
        {
            using (var contexto = new Contexto())
            {
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
    }
}