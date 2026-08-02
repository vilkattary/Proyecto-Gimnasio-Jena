using GimnasioJena.Abstracciones.LogicaDeNegocio.Bitacora;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Clases.ObtenerTodasLasClases;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Clases.RegistrarClase;
using GimnasioJena.Abstracciones.LogicaDeNegocio.HorariosSemanales.CambiarEstadoHorarioSemanal;
using GimnasioJena.Abstracciones.LogicaDeNegocio.HorariosSemanales.EditarHorarioSemanal;
using GimnasioJena.Abstracciones.LogicaDeNegocio.HorariosSemanales.GenerarClasesProgramadas;
using GimnasioJena.Abstracciones.LogicaDeNegocio.HorariosSemanales.ObtenerHorarioSemanalPorId;
using GimnasioJena.Abstracciones.LogicaDeNegocio.HorariosSemanales.ObtenerHorariosSemanales;
using GimnasioJena.Abstracciones.LogicaDeNegocio.HorariosSemanales.RegistrarHorariosSemanales;
using GimnasioJena.Abstracciones.Modelos.Bitacora;
using GimnasioJena.Abstracciones.Modelos.Clases;
using GimnasioJena.Abstracciones.Modelos.HorariosSemanales;
using GimnasioJena.AccesoADatos;
using GimnasioJena.LogicaDeNegocio.Bitacora;
using GimnasioJena.LogicaDeNegocio.Clases.ObtenerTodasLasClases;
using GimnasioJena.LogicaDeNegocio.Clases.RegistrarClase;
using GimnasioJena.LogicaDeNegocio.HorariosSemanales.CambiarEstadoHorarioSemanal;
using GimnasioJena.LogicaDeNegocio.HorariosSemanales.EditarHorarioSemanal;
using GimnasioJena.LogicaDeNegocio.HorariosSemanales.GenerarClasesProgramadas;
using GimnasioJena.LogicaDeNegocio.HorariosSemanales.ObtenerHorarioSemanalPorId;
using GimnasioJena.LogicaDeNegocio.HorariosSemanales.ObtenerHorariosSemanales;
using GimnasioJena.LogicaDeNegocio.HorariosSemanales.RegistrarHorariosSemanales;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;

namespace GimnasioJena.UI.Controllers
{
    [Authorize(Roles = "ADMINISTRADOR")]
    public class HorariosSemanalesController : Controller
    {
        private readonly IObtenerHorariosSemanalesLN
            _obtenerHorariosSemanalesLN;

        private readonly IRegistrarHorariosSemanalesLN
            _registrarHorariosSemanalesLN;

        private readonly IRegistrarBitacoraLN
            _registrarBitacoraLN;

        private readonly IGenerarClasesProgramadasLN
             _generarClasesProgramadasLN;

        private readonly IObtenerHorarioSemanalPorIdLN
             _obtenerHorarioSemanalPorIdLN;

        private readonly IEditarHorarioSemanalLN
             _editarHorarioSemanalLN;

        private readonly ICambiarEstadoHorarioSemanalLN
             _cambiarEstadoHorarioSemanalLN;

        private readonly IRegistrarClaseLN
             _registrarClaseLN;

        private readonly IObtenerTodasLasClasesLN
             _obtenerTodasLasClasesLN;

        public HorariosSemanalesController()
        {
            _obtenerHorariosSemanalesLN =
                new ObtenerHorariosSemanalesLN();

            _registrarHorariosSemanalesLN =
                new RegistrarHorariosSemanalesLN();

            _registrarBitacoraLN =
                new RegistrarBitacoraLN();

            _generarClasesProgramadasLN =
                new GenerarClasesProgramadasLN();

            _obtenerHorarioSemanalPorIdLN =
                new ObtenerHorarioSemanalPorIdLN();

            _editarHorarioSemanalLN =
                 new EditarHorarioSemanalLN();

            _cambiarEstadoHorarioSemanalLN =
                new CambiarEstadoHorarioSemanalLN();

            _registrarClaseLN =
                new RegistrarClaseLN();

            _obtenerTodasLasClasesLN =
                new ObtenerTodasLasClasesLN();
        }

        // GET: HorariosSemanales
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                List<HorarioSemanalListadoDto> horarios =
                    _obtenerHorariosSemanalesLN
                        .ObtenerHorariosSemanales();

                DateTime hoy = DateTime.Today;

                ViewBag.ClasesUnicas =
                    _obtenerTodasLasClasesLN
                        .ObtenerTodasLasClases()
                        .Where(c =>
                            !c.idHorario.HasValue &&
                            c.fechaClase >= hoy)
                        .OrderBy(c => c.fechaHoraInicio)
                        .ToList();

                return View(horarios);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(
                    "Error al cargar los horarios semanales: "
                    + ex
                );

                TempData["MensajeError"] =
                    "No fue posible cargar la programación semanal.";

                return View(
                    new List<HorarioSemanalListadoDto>()
                );
            }
        }

        // GET: HorariosSemanales/GenerarClases
        [HttpGet]
        public ActionResult GenerarClases()
        {
            DateTime fechaActual =
                DateTime.Now.Date;

            GenerarClasesProgramadasDto modelo =
                new GenerarClasesProgramadasDto
                {
                    fechaInicio =
                        fechaActual,

                    fechaFin =
                        fechaActual.AddDays(30)
                };

            return View(modelo);
        }

        // POST: HorariosSemanales/GenerarClases
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GenerarClases(
            GenerarClasesProgramadasDto modelo
        )
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(modelo);
                }

                ResultadoGeneracionClasesDto resultado =
                    _generarClasesProgramadasLN
                        .GenerarClasesProgramadas(modelo);

                if (!resultado.fueExitosa)
                {
                    ModelState.AddModelError(
                        "",
                        resultado.mensaje
                    );

                    return View(modelo);
                }

                RegistrarBitacoraSegura(
                    "ClaseProgramada",
                    "INSERT",
                    null,
                    ConstruirDetalleGeneracionBitacora(
                        resultado
                    )
                );

                TempData["MensajeExito"] =
                    resultado.mensaje;


                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(
                    "Error al generar las clases programadas: "
                    + ex
                );

                ModelState.AddModelError(
                    "",
                    "Ocurrió un error inesperado al generar las clases."
                );

                return View(modelo);
            }
        }
        // GET: HorariosSemanales/Registrar
        [HttpGet]
        public ActionResult Registrar()
        {
            CargarCatalogos();

            HorarioSemanalMultipleCrearDto modelo =
                new HorarioSemanalMultipleCrearDto
                {
                    diaSemana = 1,
                    cupoMaximo = 30,
                    ubicacion = null,

                    horarios =
                        new List<HorarioSemanalDetalleCrearDto>
                        {
                            new HorarioSemanalDetalleCrearDto
                            {
                                horaInicio =
                                    new TimeSpan(5, 0, 0),

                                horaFin =
                                    new TimeSpan(6, 0, 0)
                            }
                        }
                };

            return View(modelo);
        }

        // POST: HorariosSemanales/Registrar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrar(
            HorarioSemanalMultipleCrearDto modelo
        )
        {
            if (modelo == null)
            {
                modelo =
                    new HorarioSemanalMultipleCrearDto();
            }

            try
            {
                // ── Clase única (no recurrente) ──────────────────────────
                if (!modelo.esRecurrente)
                {
                    if (!modelo.fechaClase.HasValue)
                    {
                        ModelState.AddModelError(
                            "fechaClase",
                            "Debe indicar la fecha de la clase."
                        );
                    }

                    if (modelo.idTipoClase <= 0)
                        ModelState.AddModelError("idTipoClase", "Debe seleccionar el tipo de clase.");

                    if (modelo.idUsuarioEntrenador <= 0)
                        ModelState.AddModelError("idUsuarioEntrenador", "Debe seleccionar un entrenador.");

                    if (modelo.cupoMaximo < 1 || modelo.cupoMaximo > 30)
                        ModelState.AddModelError("cupoMaximo", "El cupo debe encontrarse entre 1 y 30.");

                    LimpiarFilasVacias(modelo);

                    if (modelo.horarios == null || modelo.horarios.Count == 0)
                    {
                        ModelState.AddModelError(
                            "",
                            "Debe agregar al menos un rango horario."
                        );
                    }
                    else if (modelo.horarios[0].horaFin <= modelo.horarios[0].horaInicio)
                    {
                        ModelState.AddModelError(
                            "",
                            "La hora de finalización debe ser mayor que la hora de inicio."
                        );
                    }

                    if (!ModelState.IsValid)
                    {
                        PrepararVistaRegistrar(modelo);
                        return View(modelo);
                    }

                    ClaseCrearDto claseUnica = new ClaseCrearDto
                    {
                        idTipoClase          = modelo.idTipoClase,
                        idUsuarioEntrenador  = modelo.idUsuarioEntrenador,
                        idEstadoClase        = 1,
                        fechaClase           = modelo.fechaClase.Value,
                        horaInicio           = modelo.horarios[0].horaInicio,
                        horaFin              = modelo.horarios[0].horaFin,
                        cupoMaximo           = modelo.cupoMaximo,
                        ubicacion            = modelo.ubicacion,
                        fechaCreacion        = DateTime.Now
                    };

                    bool seAgrego = _registrarClaseLN.RegistrarClase(claseUnica);

                    if (seAgrego)
                    {
                        RegistrarBitacoraSegura(
                            "ClaseProgramada",
                            "INSERT",
                            null,
                            "Se registró una clase única programada."
                        );

                        TempData["MensajeExito"] =
                            "La clase se registró correctamente.";

                        return RedirectToAction("Index");
                    }

                    ModelState.AddModelError(
                        "",
                        "No se pudo registrar la clase."
                    );

                    PrepararVistaRegistrar(modelo);
                    return View(modelo);
                }

                // ── Clase recurrente: crear HorarioSemanal + generar ─────

                if (!modelo.fechaFin.HasValue)
                {
                    ModelState.AddModelError(
                        "fechaFin",
                        "Para programación recurrente debe indicar una fecha hasta la que generar las clases."
                    );
                }

                ValidarDatosDelFormulario(modelo);

                if (!ModelState.IsValid)
                {
                    PrepararVistaRegistrar(modelo);

                    return View(modelo);
                }

                ResultadoRegistroHorariosDto resultado =
                    _registrarHorariosSemanalesLN
                        .RegistrarHorariosSemanales(modelo);

                if (!resultado.fueExitoso)
                {
                    ModelState.AddModelError(
                        "",
                        resultado.mensaje
                    );

                    PrepararVistaRegistrar(modelo);

                    return View(modelo);
                }

                // Generar instancias en el rango indicado
                GenerarClasesProgramadasDto generarDto =
                    new GenerarClasesProgramadasDto
                    {
                        fechaInicio = DateTime.Now.Date,
                        fechaFin    = modelo.fechaFin.Value
                    };

                ResultadoGeneracionClasesDto resultadoGenerar =
                    _generarClasesProgramadasLN
                        .GenerarClasesProgramadas(generarDto);

                RegistrarBitacoraSegura(
                    "HorarioSemanal",
                    "INSERT",
                    null,
                    ConstruirDetalleBitacora(
                        modelo,
                        resultado.cantidadRegistrada
                    )
                );

                TempData["MensajeExito"] = resultadoGenerar.fueExitosa
                    ? resultado.mensaje + " " + resultadoGenerar.mensaje
                    : resultado.mensaje;

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(
                    "Error al registrar la programación semanal: "
                    + ex
                );

                ModelState.AddModelError(
                    "",
                    ObtenerMensajeErrorRegistro(ex)
                );

                PrepararVistaRegistrar(modelo);

                return View(modelo);
            }
        }

        // GET: HorariosSemanales/Editar/5
        [HttpGet]
        public ActionResult Editar(int id)
        {
            try
            {
                HorarioSemanalEditarDto modelo =
                    _obtenerHorarioSemanalPorIdLN
                        .ObtenerHorarioSemanalPorId(id);

                if (modelo == null)
                {
                    return HttpNotFound();
                }

                CargarCatalogos(
                    modelo.idTipoClase,
                    modelo.idUsuarioEntrenador
                );

                return View(modelo);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(
                    "Error al cargar el horario semanal: "
                    + ex
                );

                TempData["MensajeError"] =
                    "No fue posible cargar el horario seleccionado.";

                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(
        HorarioSemanalEditarDto modelo
)
        {
            try
            {
                if (modelo.horaFin <= modelo.horaInicio)
                {
                    ModelState.AddModelError(
                        "horaFin",
                        "La hora de finalización debe ser mayor que la hora de inicio."
                    );
                }

                if (!ModelState.IsValid)
                {
                    CargarCatalogos(
                        modelo.idTipoClase,
                        modelo.idUsuarioEntrenador
                    );

                    return View(modelo);
                }

                _editarHorarioSemanalLN
                    .EditarHorarioSemanal(modelo);

                RegistrarBitacoraSegura(
                    "HorarioSemanal",
                    "UPDATE",
                    modelo.idHorario,
                    "Se modificó un horario semanal."
                );

                TempData["MensajeExito"] =
                    "La programación semanal fue actualizada correctamente.";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                Exception excepcionReal = ex;

                while (excepcionReal.InnerException != null)
                {
                    excepcionReal = excepcionReal.InnerException;
                }

                string mensajeError =
                    excepcionReal.Message;

                if (
                    mensajeError.Contains(
                        "UQ_HorarioSemanal"
                    )
                    ||
                    mensajeError.Contains(
                        "duplicate key"
                    )
                )
                {
                    ModelState.AddModelError(
                        "",
                        "La entrenadora ya tiene una clase programada en ese mismo día y hora."
                    );
                }
                else
                {
                    ModelState.AddModelError(
                        "",
                        "Ocurrió un error al actualizar la programación semanal."
                    );
                }

                CargarCatalogos(
                    modelo.idTipoClase,
                    modelo.idUsuarioEntrenador
                );

                return View(modelo);
            }
        }

        // POST: HorariosSemanales/CambiarEstado
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CambiarEstado(int idHorario)
        {
            try
            {
                bool nuevoEstado =
                    _cambiarEstadoHorarioSemanalLN
                        .CambiarEstadoHorarioSemanal(idHorario);

                RegistrarBitacoraSegura(
                    "HorarioSemanal",
                    "CAMBIO_ESTADO",
                    idHorario,
                    nuevoEstado
                        ? "Se activó un horario semanal."
                        : "Se desactivó un horario semanal."
                );

                TempData["MensajeExito"] =
                    nuevoEstado
                        ? "La programación semanal fue activada correctamente."
                        : "La programación semanal fue desactivada correctamente.";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(
                    "Error al cambiar el estado del horario semanal: "
                    + ex
                );

                TempData["MensajeError"] =
                    "No fue posible cambiar el estado de la programación semanal: " +
                    ObtenerMensajeCompleto(ex);

                return RedirectToAction("Index");
            }
        }
        private void ValidarDatosDelFormulario(
            HorarioSemanalMultipleCrearDto modelo
        )
        {
            if (modelo.idTipoClase <= 0)
            {
                ModelState.AddModelError(
                    "idTipoClase",
                    "Debe seleccionar un tipo de clase."
                );
            }

            if (modelo.idUsuarioEntrenador <= 0)
            {
                ModelState.AddModelError(
                    "idUsuarioEntrenador",
                    "Debe seleccionar un entrenador."
                );
            }

            if (
                modelo.diaSemana < 1 ||
                modelo.diaSemana > 7
            )
            {
                ModelState.AddModelError(
                    "diaSemana",
                    "Debe seleccionar un día válido."
                );
            }

            if (
                modelo.cupoMaximo < 1 ||
                modelo.cupoMaximo > 30
            )
            {
                ModelState.AddModelError(
                    "cupoMaximo",
                    "El cupo máximo debe estar entre 1 y 30."
                );
            }


            if (
                modelo.horarios == null ||
                !modelo.horarios.Any()
            )
            {
                ModelState.AddModelError(
                    "",
                    "Debe agregar al menos un rango horario."
                );

                return;
            }

            for (
                int i = 0;
                i < modelo.horarios.Count;
                i++
            )
            {
                HorarioSemanalDetalleCrearDto horario =
                    modelo.horarios[i];

                if (horario == null)
                {
                    ModelState.AddModelError(
                        "",
                        $"El rango horario número {i + 1} no es válido."
                    );

                    continue;
                }

                if (
                    horario.horaFin <=
                    horario.horaInicio
                )
                {
                    ModelState.AddModelError(
                        $"horarios[{i}].horaFin",
                        "La hora de finalización debe ser mayor que la hora de inicio."
                    );
                }
            }
        }

        private void LimpiarFilasVacias(
            HorarioSemanalMultipleCrearDto modelo
        )
        {
            if (modelo.horarios == null)
            {
                modelo.horarios =
                    new List<HorarioSemanalDetalleCrearDto>();

                return;
            }

            modelo.horarios =
                modelo.horarios
                    .Where(h =>
                        h != null &&
                        (
                            h.horaInicio != TimeSpan.Zero ||
                            h.horaFin != TimeSpan.Zero
                        )
                    )
                    .ToList();
        }

        private void AsegurarAlMenosUnHorario(
            HorarioSemanalMultipleCrearDto modelo
        )
        {
            if (modelo.horarios == null)
            {
                modelo.horarios =
                    new List<HorarioSemanalDetalleCrearDto>();
            }

            if (!modelo.horarios.Any())
            {
                modelo.horarios.Add(
                    new HorarioSemanalDetalleCrearDto
                    {
                        horaInicio =
                            new TimeSpan(5, 0, 0),

                        horaFin =
                            new TimeSpan(6, 0, 0)
                    }
                );
            }
        }

        private void PrepararVistaRegistrar(
            HorarioSemanalMultipleCrearDto modelo
        )
        {
            AsegurarAlMenosUnHorario(modelo);

            CargarCatalogos(
                modelo.idTipoClase,
                modelo.idUsuarioEntrenador
            );
        }

        private void CargarCatalogos(
            int? idTipoClaseSeleccionado = null,
            int? idUsuarioEntrenadorSeleccionado = null
        )
        {
            using (Contexto contexto = new Contexto())
            {
                ViewBag.TiposClase =
                    new SelectList(
                        contexto.TiposClase
                            .Where(t => t.estado)
                            .OrderBy(t => t.nombreClase)
                            .ToList(),

                        "idTipoClase",
                        "nombreClase",
                        idTipoClaseSeleccionado
                    );

                ViewBag.Entrenadores =
                    new SelectList(
                        contexto.Entrenadores
                            .Where(e => e.estado)
                            .Join(
                                contexto.Usuarios,

                                entrenador =>
                                    entrenador.idUsuario,

                                usuario =>
                                    usuario.idUsuario,

                                (entrenador, usuario) =>
                                    new
                                    {
                                        idUsuario =
                                            usuario.idUsuario,

                                        nombreCompleto =
                                            usuario.nombre
                                            + " "
                                            + usuario.apellido1
                                            + " "
                                            + usuario.apellido2
                                    }
                            )
                            .OrderBy(e =>
                                e.nombreCompleto
                            )
                            .ToList(),

                        "idUsuario",
                        "nombreCompleto",
                        idUsuarioEntrenadorSeleccionado
                    );
            }
        }

        private int? ObtenerIdUsuarioActual()
        {
            string identityUserId =
                User.Identity.GetUserId();

            using (Contexto contexto = new Contexto())
            {
                var usuario =
                    contexto.Usuarios
                        .FirstOrDefault(u =>
                            u.identityUserId ==
                            identityUserId
                        );

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
            string detalle
        )
        {
            _registrarBitacoraLN
                .RegistrarBitacora(
                    new BitacoraDto
                    {
                        idUsuario =
                            ObtenerIdUsuarioActual(),

                        tablaAfectada =
                            tabla,

                        accionRealizada =
                            accion,

                        idRegistroAfectado =
                            idRegistro,

                        detalle =
                            detalle,

                        ipUsuario =
                            ObtenerIpUsuario()
                    }
                );
        }

        private void RegistrarBitacoraSegura(
            string tabla,
            string accion,
            int? idRegistro,
            string detalle
        )
        {
            try
            {
                RegistrarBitacora(
                    tabla,
                    accion,
                    idRegistro,
                    detalle
                );
            }
            catch (Exception ex)
            {
                /*
                 * La programación ya fue guardada.
                 * Se registra el error para diagnóstico, pero no se
                 * interrumpe el flujo ni se muestra un falso fallo.
                 */
                Debug.WriteLine(
                    "No fue posible registrar la bitácora: "
                    + ex
                );
            }
        }

        private string ConstruirDetalleBitacora(
            HorarioSemanalMultipleCrearDto modelo,
            int cantidadRegistrada
        )
        {
            string nombreDia =
                ObtenerNombreDia(modelo.diaSemana);

            string textoHorario =
                cantidadRegistrada == 1
                    ? "horario semanal"
                    : "horarios semanales";

            return
                $"Se registraron {cantidadRegistrada} " +
                $"{textoHorario} para el día {nombreDia}.";
        }

        private string ObtenerMensajeErrorRegistro(
            Exception excepcion
        )
        {
            Exception excepcionReal =
                excepcion;

            while (
                excepcionReal.InnerException != null
            )
            {
                excepcionReal =
                    excepcionReal.InnerException;
            }

            /*
             * Durante el desarrollo se conserva el detalle real
             * para identificar rápidamente problemas de base de datos.
             */
            return
                "Ocurrió un error al registrar la programación semanal. " +
                "Detalle: " +
                excepcionReal.Message;
        }

        private string ObtenerNombreDia(
            byte diaSemana
        )
        {
            switch (diaSemana)
            {
                case 1:
                    return "Lunes";

                case 2:
                    return "Martes";

                case 3:
                    return "Miércoles";

                case 4:
                    return "Jueves";

                case 5:
                    return "Viernes";

                case 6:
                    return "Sábado";

                case 7:
                    return "Domingo";

                default:
                    return "No especificado";
            }
        }
        private string ConstruirDetalleGeneracionBitacora(
    ResultadoGeneracionClasesDto resultado
)
        {
            return
                $"Se procesaron {resultado.horariosProcesados} horarios semanales. " +
                $"Se generaron {resultado.clasesGeneradas} clases programadas " +
                $"entre el {resultado.fechaInicioGenerada:dd/MM/yyyy} " +
                $"y el {resultado.fechaFinGenerada:dd/MM/yyyy}. " +
                $"Se omitieron {resultado.clasesOmitidas} clases porque ya existían.";
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