using GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenadores.EditarEntrenador;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenadores.ObtenerEntrenadorPorId;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenadores.ObtenerTodosLosEntrenadores;
using GimnasioJena.Abstracciones.Modelos.Entrenadores;
using GimnasioJena.AccesoADatos.Entrenadores.ObtenerEntrenadorPorId;
using GimnasioJena.LogicaDeNegocio.Entrenadores.EditarEntrenador;
using GimnasioJena.LogicaDeNegocio.Entrenadores.ObtenerEntrenadorPorId;
using GimnasioJena.LogicaDeNegocio.Entrenadores.ObtenerTodosLosEntrenadores;
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

        public EntrenadoresController()
        {
            var repositorio = new ObtenerEntrenadorPorIdAD();
            _obtenerEntrenadorServicio = new ObtenerEntrenadorPorIdLN(repositorio);
            _obtenerTodosLosEntrenadoresServicio = new ObtenerTodosLosEntrenadoresLN();
            _editarEntrenadorServicio = new EditarEntrenadorLN();
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