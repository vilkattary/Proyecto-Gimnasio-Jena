using GimnasioJena.Abstracciones.LogicaDeNegocio.Asistencias.ObtenerAsistenciasPorClase;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Asistencias.RegistrarAsistencia;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Clases.ObtenerClasesPorEntrenador;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenadores.EditarEntrenador;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenadores.ObtenerEntrenadorPorId;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenadores.ObtenerTodosLosEntrenadores;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.EditarReserva;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.ObtenerReservasPorClase;
using GimnasioJena.Abstracciones.Modelos.Asistencias;
using GimnasioJena.Abstracciones.Modelos.Entrenadores;
using GimnasioJena.Abstracciones.Modelos.Reservas;
using GimnasioJena.AccesoADatos;
using GimnasioJena.AccesoADatos.Entrenadores.ObtenerEntrenadorPorId;
using GimnasioJena.LogicaDeNegocio.Asistencias.ObtenerAsistenciasPorClase;
using GimnasioJena.LogicaDeNegocio.Asistencias.RegistrarAsistencia;
using GimnasioJena.LogicaDeNegocio.Clases.ObtenerClasesPorEntrenador;
using GimnasioJena.LogicaDeNegocio.Entrenadores.EditarEntrenador;
using GimnasioJena.LogicaDeNegocio.Entrenadores.ObtenerEntrenadorPorId;
using GimnasioJena.LogicaDeNegocio.Entrenadores.ObtenerTodosLosEntrenadores;
using GimnasioJena.LogicaDeNegocio.Reservas.EditarReserva;
using GimnasioJena.LogicaDeNegocio.Reservas.ObtenerReservasPorClase;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GimnasioJena.UI.Controllers
{
    [Authorize]
    public class EntrenadoresController : Controller
    {
        private readonly IObtenerEntrenadorPorIdLN _obtenerEntrenadorServicio;
        private readonly IObtenerTodosLosEntrenadoresLN _obtenerTodosLosEntrenadoresServicio;
        private readonly IEditarEntrenadorLN _editarEntrenadorServicio;
        private readonly IObtenerClasesPorEntrenadorLN _obtenerClasesPorEntrenadorServicio;
        private readonly IObtenerReservasPorClaseLN _obtenerReservasPorClaseServicio;
        private readonly IObtenerAsistenciasPorClaseLN _obtenerAsistenciasPorClaseServicio;
        private readonly IRegistrarAsistenciaLN _registrarAsistenciaServicio;
        private readonly IEditarReservaLN _editarReservaServicio;

        public EntrenadoresController()
        {
            var repositorio = new ObtenerEntrenadorPorIdAD();
            _obtenerEntrenadorServicio = new ObtenerEntrenadorPorIdLN(repositorio);
            _obtenerTodosLosEntrenadoresServicio = new ObtenerTodosLosEntrenadoresLN();
            _editarEntrenadorServicio = new EditarEntrenadorLN();
            _obtenerClasesPorEntrenadorServicio = new ObtenerClasesPorEntrenadorLN();
            _obtenerReservasPorClaseServicio = new ObtenerReservasPorClaseLN();
            _obtenerAsistenciasPorClaseServicio = new ObtenerAsistenciasPorClaseLN();
            _registrarAsistenciaServicio = new RegistrarAsistenciaLN();
            _editarReservaServicio = new EditarReservaLN();
        }

        [Authorize(Roles = "ENTRENADOR")]
        public async Task<ActionResult> Index()
        {
            var identityUserId = User.Identity.GetUserId();

            var perfil = await _obtenerEntrenadorServicio.ObtenerEntrenadorPorId(identityUserId);

            if (perfil == null)
            {
                TempData["MensajeError"] = "No se encontró información del entrenador asociado a este usuario.";
                return RedirectToAction("Index", "Home");
            }

            return View(perfil);
        }
        [Authorize(Roles = "ENTRENADOR")]
        public ActionResult MisClases()
        {
            var identityUserId = User.Identity.GetUserId();

            var clases = _obtenerClasesPorEntrenadorServicio
                .ObtenerClasesPorEntrenador(identityUserId);

            return View(clases);
        }

        [Authorize(Roles = "ENTRENADOR")]
        public ActionResult InscritosClase(int id)
        {
            var reservas = _obtenerReservasPorClaseServicio.ObtenerReservasPorClase(id);

            using (var contexto = new Contexto())
            {
                var estados = contexto.EstadoReservas
                    .Where(e => e.estado) 
                    .Select(e => new { e.idEstadoReserva, e.nombreEstado })
                    .ToList();

                ViewBag.EstadosReserva = new SelectList(estados, "idEstadoReserva", "nombreEstado");
            }

            ViewBag.IdClaseProgramada = id;
            return View(reservas);
        }

        [Authorize(Roles = "ENTRENADOR")]
        public ActionResult AsistenciaClase(int id)
        {
            var asistencias = _obtenerAsistenciasPorClaseServicio.ObtenerAsistenciasPorClase(id);
            ViewBag.IdClaseProgramada = id;

            return View(asistencias);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ENTRENADOR")]
        public ActionResult RegistrarAsistencia(AsistenciaCrearDto modelo, int idClaseProgramada)
        {
            bool resultado = _registrarAsistenciaServicio.RegistrarAsistencia(modelo);

            if (resultado)
            {
                TempData["MensajeExito"] = "La asistencia se registró correctamente.";
            }
            else
            {
                TempData["MensajeError"] = "No se pudo registrar la asistencia.";
            }

            return RedirectToAction("AsistenciaClase", new { id = idClaseProgramada });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ENTRENADOR")]
        public ActionResult CambiarEstadoReserva(int idReserva, int idEstadoReserva, int idClaseProgramada)
        {
            var ok = _editarReservaServicio.CambiarEstadoReserva(idReserva, idEstadoReserva);

            if (ok)
                TempData["MensajeExito"] = "Estado de la reserva actualizado.";
            else
                TempData["MensajeError"] = "No se pudo actualizar el estado de la reserva.";

            return RedirectToAction("InscritosClase", new { id = idClaseProgramada });
        }
        // GET
        [Authorize(Roles = "ENTRENADOR")]
        public ActionResult CambiarEstadoReserva(int id)
        {
            using (var contexto = new Contexto())
            {
                var reserva = contexto.Reservas.FirstOrDefault(r => r.idReserva == id);
                if (reserva == null) return HttpNotFound();

                var usuario = contexto.Usuarios.FirstOrDefault(u => u.idUsuario == reserva.idUsuario);

                var dto = new ReservaClaseDto
                {
                    idReserva = reserva.idReserva,
                    idClaseProgramada = reserva.idClaseProgramada,
                    nombreCliente = usuario != null ? usuario.nombre + " " + usuario.apellido1 + " " + usuario.apellido2 : string.Empty,
                    correoCliente = usuario?.correo,
                    telefonoCliente = usuario?.telefono,
                    idEstadoReserva = reserva.idEstadoReserva,
                    estadoReserva = contexto.EstadoReservas.FirstOrDefault(e => e.idEstadoReserva == reserva.idEstadoReserva)?.nombreEstado
                };

                var estados = contexto.EstadoReservas
                    .Where(e => e.estado)
                    .Select(e => new { e.idEstadoReserva, e.nombreEstado })
                    .ToList();

                ViewBag.EstadosReserva = new SelectList(estados, "idEstadoReserva", "nombreEstado", dto.idEstadoReserva);

                return View(dto);
            }
        }

        [Authorize(Roles = "ADMINISTRADOR")]
        public ActionResult Administrar()
        {
            var entrenadores = _obtenerTodosLosEntrenadoresServicio.ObtenerTodosLosEntrenadores();
            return View(entrenadores);
        }
        [HttpGet]
        [Authorize(Roles = "ADMINISTRADOR")]
        public ActionResult Editar(int id)
        {
            var entrenador = _obtenerEntrenadorServicio.ObtenerEntrenadorPorId(id);

            if (entrenador == null)
            {
                TempData["MensajeError"] = "No se encontró el entrenador solicitado.";
                return RedirectToAction("Administrar");
            }

            var modelo = new EntrenadorEditarDto
            {
                idEntrenador = entrenador.idEntrenador,
                idUsuario = entrenador.idUsuario,
                especialidad = entrenador.especialidad,
                descripcion = entrenador.descripcion,
                fechaContratacion = entrenador.fechaContratacion,
                estado = entrenador.estado
            };

            return View(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMINISTRADOR")]
        public ActionResult Editar(EntrenadorEditarDto modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            bool resultado = _editarEntrenadorServicio.EditarEntrenador(modelo);

            if (resultado)
            {
                TempData["MensajeExito"] = "La información del entrenador se actualizó correctamente.";
                return RedirectToAction("Administrar");
            }

            TempData["MensajeError"] = "No se pudo actualizar la información del entrenador.";
            return View(modelo);
        }
    }
}