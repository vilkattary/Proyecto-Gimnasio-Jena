using GimnasioJena.Abstracciones.LogicaDeNegocio.Usuarios.ObtenerUsuarioPorId;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GimnasioJena.UI.Controllers
{
    [Authorize(Roles = "CLIENTE")]
    public class ClientesController : Controller
    {
        private readonly IObtenerUsuarioPorIdLN _obtenerUsuarioServicio;

        public ClientesController(IObtenerUsuarioPorIdLN obtenerUsuarioServicio)
        {
            _obtenerUsuarioServicio = obtenerUsuarioServicio;
        }

        public async Task<ActionResult> MiPerfil()
        {
            var identityUserId = User.Identity.GetUserId();
            var perfil = await _obtenerUsuarioServicio.ObtenerUsuarioPorId(identityUserId);
            return View(perfil);
        }

        public async Task<ActionResult> MiMembresia()
        {
            var identityUserId = User.Identity.GetUserId();
            var perfil = await _obtenerUsuarioServicio.ObtenerUsuarioPorId(identityUserId);
            return View(perfil);
        }

        public ActionResult ReservarClases()
        {
            return View();
        }

        public ActionResult MisReservas()
        {
            return View();
        }
    }
}