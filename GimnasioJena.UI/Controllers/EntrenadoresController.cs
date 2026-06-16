using GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenadores.ObtenerEntrenadorPorId;
using GimnasioJena.Abstracciones.Modelos.Entrenadores;
using GimnasioJena.AccesoADatos.Entrenadores.ObtenerEntrenadorPorId;
using GimnasioJena.LogicaDeNegocio.Entrenadores.ObtenerEntrenadorPorId;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace GimnasioJena.UI.Controllers
{
    [Authorize(Roles = "ENTRENADOR")]
    public class EntrenadoresController : Controller
    {
        private readonly IObtenerEntrenadorPorIdLN _obtenerEntrenadorServicio;

        public EntrenadoresController()
        {
            var repositorio = new ObtenerEntrenadorPorIdAD();
            _obtenerEntrenadorServicio = new ObtenerEntrenadorPorIdLN(repositorio);

        }

        public async Task<ActionResult> Index()
        {
            var identityUserId = User.Identity.GetUserId();

            var perfil = await _obtenerEntrenadorServicio.ObtenerEntrenadorPorId(identityUserId);
            return View(perfil);

        }

    }
}