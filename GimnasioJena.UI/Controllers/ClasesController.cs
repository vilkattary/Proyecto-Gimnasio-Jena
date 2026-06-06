using GimnasioJena.Abstracciones.LogicaDeNegocio.Clases.EditarClase;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Clases.ObtenerClasePorId;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Clases.ObtenerTodasLasClases;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Clases.RegistrarClase;
using GimnasioJena.Abstracciones.Modelos.Clases;
using GimnasioJena.AccesoADatos;
using GimnasioJena.LogicaDeNegocio.Clases.EditarClase;
using GimnasioJena.LogicaDeNegocio.Clases.ObtenerClasePorId;
using GimnasioJena.LogicaDeNegocio.Clases.ObtenerTodasLasClases;
using GimnasioJena.LogicaDeNegocio.Clases.RegistrarClase;
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

        public ClasesController()
        {
            _obtenerTodasLasClases = new ObtenerTodasLasClasesLN();
            _obtenerClasePorId = new ObtenerClasePorIdLN();
            _registrarClase = new RegistrarClaseLN();
            _editarClase = new EditarClaseLN();
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

            return View(new ClaseCrearDto
            {
                idEstadoClase = 1,
                fechaClase = DateTime.Today,
                horaInicio = new TimeSpan(6, 0, 0),
                horaFin = new TimeSpan(7, 0, 0),
                cupoMaximo = 12,
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

                if (claseAGuardar.cupoMaximo < 1 || claseAGuardar.cupoMaximo > 12)
                {
                    ModelState.AddModelError("cupoMaximo", "El cupo máximo debe estar entre 1 y 12.");
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
                    TempData["MensajeExito"] = "La clase se registró correctamente.";
                    return RedirectToAction("ObtenerTodasLasClases");
                }

                TempData["MensajeError"] = "No se pudo registrar la clase.";
                CargarCatalogos();
                return View(claseAGuardar);
            }
            catch
            {
                TempData["MensajeError"] = "Ocurrió un error al registrar la clase.";
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
    }
}