using GimnasioJena.Abstracciones.LogicaDeNegocio.Clases.ObtenerTodasLasClases;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Clases.ObtenerClasePorId;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Clases.RegistrarClase;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Clases.EditarClase;
using GimnasioJena.Abstracciones.Modelos.Clases;
using GimnasioJena.LogicaDeNegocio.Clases.ObtenerTodasLasClases;
using GimnasioJena.LogicaDeNegocio.Clases.ObtenerClasePorId;
using GimnasioJena.LogicaDeNegocio.Clases.RegistrarClase;
using GimnasioJena.LogicaDeNegocio.Clases.EditarClase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GimnasioJena.UI.Controllers
{
    public class ClasesController : Controller
    {
        private IObtenerTodasLasClasesLN _obtenerTodasLasClases;
        private IObtenerClasePorIdLN _obtenerClasePorId;
        private IRegistrarClaseLN _registrarClase;
        private IEditarClaseLN _editarClase;

        public ClasesController()
        {
            _obtenerTodasLasClases = new ObtenerTodasLasClasesLN();
            _obtenerClasePorId = new ObtenerClasePorIdLN();
            _registrarClase = new RegistrarClaseLN();
            _editarClase = new EditarClaseLN();
        }

        // GET: OBTENER TODAS LAS CLASES
        public ActionResult ObtenerTodasLasClases()
        {
            List<ClaseListadoDto> listaDeClases = _obtenerTodasLasClases.ObtenerTodasLasClases();
            return View(listaDeClases);
        }

        // GET: DETALLE DE UNA CLASE
        public ActionResult DetalleDeLaClase(int id)
        {
            ClaseListadoDto laClase = _obtenerClasePorId.ObtenerClasePorId(id);
            return View(laClase);
        }

        // GET: CREAR UNA CLASE
        public ActionResult RegistrarClase()
        {
            return View();
        }

        // POST: CREAR UNA CLASE
        [HttpPost]
        public ActionResult RegistrarClase(string nombreClase, ClaseCrearDto claseAGuardar)
        {
            try
            {
                List<ClaseListadoDto> todasLasClases = _obtenerTodasLasClases.ObtenerTodasLasClases();
                bool existeClase = todasLasClases.Any(c => c.nombreClase == nombreClase);

                if (existeClase)
                {
                    ModelState.AddModelError("nombreClase", "Ya existe una clase con este nombre.");
                    return View(claseAGuardar);
                }

                if (ModelState.IsValid)
                {
                    // Forzar que se cree activa y con fecha de creación
                    claseAGuardar.estadoClase = true;
                    claseAGuardar.fechaCreacion = DateTime.Now;

                    bool seAgrego = _registrarClase.RegistrarClase(claseAGuardar);
                    if (seAgrego)
                    {
                        return RedirectToAction("ObtenerTodasLasClases");
                    }
                }

                return View(claseAGuardar);
            }
            catch
            {
                return View(claseAGuardar);
            }
        }

        // GET: EDITAR UNA CLASE
        public ActionResult EditarClase(int id)
        {
            ClaseListadoDto claseListado = _obtenerClasePorId.ObtenerClasePorId(id);

            ClaseEditarDto laClase = new ClaseEditarDto
            {
                idClaseProgramada = claseListado.idClaseProgramada, 
                nombreClase = claseListado.nombreClase,
                tipoClase = claseListado.TipoClase,
                nombreEntrenador = claseListado.nombreEntrenador,
                fechaClase = claseListado.fechaClase,
                horaInicio = claseListado.horaInicio,
                horaFin = claseListado.horaFin,
                cupoMaximo = claseListado.cupoMaximo,
                ubicacion = claseListado.ubicacion,
                estadoClase = claseListado.estadoClase
            };

            return View(laClase);
        }

        // POST: EDITAR UNA CLASE
        [HttpPost]
        public ActionResult EditarClase(int id, ClaseEditarDto claseAEditar)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                    claseAEditar.fechaModificacion = DateTime.Now;

                    bool seActualizo = _editarClase.EditarClase(claseAEditar);

                    if (seActualizo)
                    {
                        return RedirectToAction("ObtenerTodasLasClases");
                    }
                }

                return View(claseAEditar);
            }
            catch
            {
                return View(claseAEditar);
            }
        }

        // GET: OBTENER CLASES DISPONIBLES
        public ActionResult ObtenerClasesDisponibles()
        {
            List<ClaseListadoDto> todasLasClases = _obtenerTodasLasClases.ObtenerTodasLasClases();

            List<ClaseListadoDto> disponibles = todasLasClases
                .Where(c => c.estadoClase == true && c.cuposDisponibles > 0)
                .OrderBy(c => c.fechaClase)
                .ThenBy(c => c.horaInicio)
                .ToList();

            return View(disponibles);
        }
    }
}
